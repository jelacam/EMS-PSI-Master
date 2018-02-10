//-----------------------------------------------------------------------
// <copyright file="LinearOptimization.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService.LinearAlgorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Microsoft.SolverFoundation.Services;

    /// <summary>
    /// Class for linear optimization
    /// </summary>
    public class LinearOptimization
    {
        #region Fields

        /// <summary>
        /// context for solver
        /// </summary>
        private SolverContext context;

        /// <summary>
        /// object for locking
        /// </summary>
        private object lockObj;

        /// <summary>
        /// total cost for linear optimization
        /// </summary>
        private float totalCostWithRenewable = 0;

        /// <summary>
        /// total cost of production without renewable generators
        /// </summary>
        private float totalCostNonRenewable=0;
        
        /// <summary>
        /// minimal production of generators
        /// </summary>
        private float minProduction = 0;

        /// <summary>
        /// maximal production of generators
        /// </summary>
        private float maxProduction = 0;

		private static Dictionary<long, OptimisationModel> old;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets total production
		/// </summary>
		public float OptimizedLinear { get; set; }

        /// <summary>
        /// Gets or sets total production of wind generators
        /// </summary>
        public float WindOptimizedLinear { get; set; }

        /// <summary>
        /// Gets or sets percentage of wind production
        /// </summary>
        public float WindOptimizedPctLinear { get; set; }

        /// <summary>
        /// Gets or sets profit ($) of using wind generators
        /// </summary>
        public float Profit { get; set; }

        /// <summary>
        /// Gets or sets emission of CO2 non renewable generator
        /// </summary>
        public float CO2EmissionNonRenewable { get; set; }

        /// <summary>
        /// Gets or sets emission of CO2 with renewable generator
        /// </summary>
        public float CO2EmmissionRenewable { get; set; }

        /// <summary>
		/// Gets or sets total cost for optimization with renewable generators
		/// </summary>
		public float TotalCostWithRenewable
        {
            get { return totalCostWithRenewable; }
            set { totalCostWithRenewable = value; }
        }

        /// <summary>
        /// Gets or sets total cost for optimization without renewable generators
        /// </summary>
        public float TotalCostNonRenewable
        {
            get { return totalCostNonRenewable; }
            set { totalCostNonRenewable = value; }
        }

        #endregion 

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearOptimization" /> class
        /// </summary>
        /// <param name="minProduction">minimal production of generators</param>
        /// <param name="maxProduction">maximal production of generators</param>
        public LinearOptimization(float minProduction, float maxProduction, Dictionary<long, OptimisationModel> oldOptModelMap)
        {
            totalCostWithRenewable = 0;
            totalCostNonRenewable = 0;
            OptimizedLinear = 0;
            WindOptimizedLinear = 0;
            WindOptimizedPctLinear = 0;
            Profit = 0;
            CO2EmissionNonRenewable = 0;
            CO2EmmissionRenewable = 0;

            this.minProduction = minProduction;
            this.maxProduction = maxProduction;
			old = oldOptModelMap;

			lockObj = new object();
            context = SolverContext.GetContext();
        }

		/// <summary>
		/// Starts calculation
		/// </summary>
		/// <param name="optModelMap">model for optimization</param>
		/// <param name="consumption">current consumption</param>
		/// <returns>linear optimized model</returns>
		public Dictionary<long, OptimisationModel> Start(Dictionary<long, OptimisationModel> optModelMap, float consumption)
		{
			lock (lockObj)
			{
				if (optModelMap.Count() > 0)
				{
					float production = maxProduction;
					Dictionary<long, OptimisationModel> optModelMapNonRenewable = new Dictionary<long, OptimisationModel>();

					foreach (var item in optModelMap)
					{
						if (!item.Value.EmsFuel.FuelType.Equals(EmsFuelType.wind))
						{
							optModelMapNonRenewable.Add(item.Key, item.Value);
						}
						else
						{
							production -= item.Value.MaxPower;
						}
					}

					//mora prvo optimizacija bez vetrogeneratora
					optModelMapNonRenewable = StartLinearOptimization(optModelMapNonRenewable, consumption, false, maxProduction);
					optModelMap = StartLinearOptimization(optModelMap, consumption, true, maxProduction);

					Profit = totalCostNonRenewable - totalCostWithRenewable;
				}

				return optModelMap;
			}
		}

        /// <summary>
        /// Linear optimization algorithm
        /// </summary>
        /// <param name="optModelMap">model for optimization</param>
        /// <param name="consumption">current consumption</param>
        /// <param name="renewable">should renewables be included</param>
        /// <param name="maxProductionLimit">maximum limit of production</param>
        /// <returns>linear optimized model</returns>
        private Dictionary<long, OptimisationModel> StartLinearOptimization(Dictionary<long, OptimisationModel> optModelMap, float consumption, bool renewable, float maxProductionLimit)
        {
            lock (lockObj)
            {
                if (optModelMap.Count() > 0)
                {
                    Model model = context.CreateModel();

                    Dictionary<long, Decision> decisions = new Dictionary<long, Decision>();
                    foreach (var om in optModelMap)
                    {
                        Decision d = new Decision(Domain.RealNonnegative, "d" + om.Value.GlobalId.ToString());
                        model.AddDecision(d);
                        decisions.Add(om.Value.GlobalId, d);
                    }

                    if (consumption >= 0 && consumption <= maxProductionLimit)
                    {
                        Decision help;
                        string goal = string.Empty;
                        string limit = "limit";
                        string production = consumption.ToString() + "<=";

						foreach (var optModel in optModelMap)
						{
							help = decisions[optModel.Value.GlobalId];							
							Term termLimit;
							termLimit = optModel.Value.MinPower <= help <= optModel.Value.MaxPower;

							if (old != null && old.Count > 0 && old.Count.Equals(optModelMap.Count()))
							{
								float oldValue = old[optModel.Value.GlobalId].LinearOptimizedValue;

								if (!optModel.Value.EmsFuel.FuelType.Equals(EmsFuelType.wind) && !optModel.Value.EmsFuel.FuelType.Equals(EmsFuelType.solar))
								{
									if (oldValue == optModel.Value.MinPower)
									{
										// termLimit = optModel.Value.MinPower <= help <= (float)((0.2 * (optModel.Value.MaxPower - optModel.Value.MinPower)) + optModel.Value.MinPower);
									}
									else if (oldValue == optModel.Value.MaxPower)
									{
										// termLimit = (float)(optModel.Value.MaxPower - (0.2 * (optModel.Value.MaxPower - optModel.Value.MinPower))) <= help <= optModel.Value.MaxPower;
									}
								}
							}

							model.AddConstraint(limit + optModel.Value.GlobalId, termLimit);

							production += help.ToString() + "+";
							goal += help.ToString() + "*" + optModel.Value.Price.ToString() + "+";
						}

                        production = production.Substring(0, production.Length - 1);
                        production += "<=" + maxProductionLimit.ToString();
                        model.AddConstraint("production", production);

                        goal = goal.Substring(0, goal.Length - 1);
                        model.AddGoal("cost", GoalKind.Minimize, goal);

                        Solution solution = context.Solve(new SimplexDirective());
                        Report report = solution.GetReport();

                        if (renewable)
                        {
                            Console.Write("{0}", report);

                            string name = string.Empty;
                            foreach (var item in model.Decisions)
                            {
                                name = item.Name.Substring(1);
                                OptimisationModel optModel = null;
                                if (optModelMap.TryGetValue(long.Parse(name), out optModel))
                                {
                                    optModel.LinearOptimizedValue = float.Parse(item.ToDouble().ToString());
                                    OptimizedLinear += optModel.LinearOptimizedValue;
                                    CO2EmmissionRenewable += optModel.LinearOptimizedValue * optModel.EmissionFactor;
                                    if (optModel.EmsFuel.FuelType.Equals(EmsFuelType.wind))
                                    {
                                        WindOptimizedLinear += optModel.LinearOptimizedValue;
                                    }
                                }
                            }

                            totalCostWithRenewable = float.Parse(model.Goals.FirstOrDefault().ToDouble().ToString());

                            WindOptimizedPctLinear = 100 * WindOptimizedLinear / OptimizedLinear;

                            Console.WriteLine("Linear optimization: {0}kW", OptimizedLinear);
                            Console.WriteLine("Linear optimization wind: {0}kW ({1}%)", WindOptimizedLinear, WindOptimizedPctLinear);
                            Console.WriteLine("Linear optimization CO2: {0}", CO2EmmissionRenewable);

                        }
                        else
                        {
                            totalCostNonRenewable = float.Parse(model.Goals.FirstOrDefault().ToDouble().ToString());

                            string name = string.Empty;
                            foreach (var item in model.Decisions)
                            {
                                name = item.Name.Substring(1);
                                OptimisationModel optModel = null;
                                if (optModelMap.TryGetValue(long.Parse(name), out optModel))
                                {
                                    optModel.LinearOptimizedValue = float.Parse(item.ToDouble().ToString());
                                    CO2EmissionNonRenewable += optModel.LinearOptimizedValue * optModel.EmissionFactor;
                                }
                            }
                        }
                    }

                    context.ClearModel();
                }
            }

            return optModelMap;
        }
    }
}