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
		private float totalCostLinear = 0;

		/// <summary>
		/// minimal production of generators
		/// </summary>
		private float minProduction = 0;

		/// <summary>
		/// maximal production of generators
		/// </summary>
		private float maxProduction = 0;

		/// <summary>
		/// total cost of production without renewable generators
		/// </summary>
		private float totalCostNonRenewable;

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
		/// Gets or sets emission of CO2
		/// </summary>
		public float CO2Emission { get; set; }

		#endregion Fields

		/// <summary>
		/// Initializes a new instance of the <see cref="LinearOptimization" /> class
		/// </summary>
		/// <param name="minProduction">minimal production of generators</param>
		/// <param name="maxProduction">maximal production of generators</param>
		public LinearOptimization(float minProduction, float maxProduction)
		{
			TotalCost = 0;
			totalCostNonRenewable = 0;
			OptimizedLinear = 0;
			WindOptimizedLinear = 0;
			WindOptimizedPctLinear = 0;
			Profit = 0;
			CO2Emission = 0;

			this.minProduction = minProduction;
			this.maxProduction = maxProduction;

			lockObj = new object();
			context = SolverContext.GetContext();
		}

		/// <summary>
		/// Gets or sets total cost for linear optimization
		/// </summary>
		public float TotalCost
		{
			get
			{
				return totalCostLinear;
			}

			set
			{
				totalCostLinear = value;
			}
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

				optModelMap = StartLinearOptimization(optModelMap, consumption, true, maxProduction);
				optModelMapNonRenewable = StartLinearOptimization(optModelMapNonRenewable, consumption, false, maxProduction);

				Profit = totalCostNonRenewable - totalCostLinear;

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
		public Dictionary<long, OptimisationModel> StartLinearOptimization(Dictionary<long, OptimisationModel> optModelMap, float consumption, bool renewable, float maxProductionLimit)
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
							// Console.Write("{0}", report);

							string name = string.Empty;
							foreach (var item in model.Decisions)
							{
								name = item.Name.Substring(1);
								OptimisationModel optModel = null;
								if (optModelMap.TryGetValue(long.Parse(name), out optModel))
								{
									optModel.LinearOptimizedValue = float.Parse(item.ToDouble().ToString());
									OptimizedLinear += optModel.LinearOptimizedValue;
									CO2Emission += optModel.LinearOptimizedValue * optModel.EmissionFactor;
									if (optModel.EmsFuel.FuelType.Equals(EmsFuelType.wind))
									{
										WindOptimizedLinear += optModel.LinearOptimizedValue;
									}								
								}
							}

							totalCostLinear = float.Parse(model.Goals.FirstOrDefault().ToDouble().ToString());

							WindOptimizedPctLinear = 100 * WindOptimizedLinear / OptimizedLinear;
							Console.WriteLine("Linear optimization: {0}kW", OptimizedLinear);
							Console.WriteLine("Linear optimization wind: {0}kW ({1}%)", WindOptimizedLinear, WindOptimizedPctLinear);
							Console.WriteLine("Linear optimization CO2: {0}", CO2Emission);
						}
						else
						{
							totalCostNonRenewable = float.Parse(model.Goals.FirstOrDefault().ToDouble().ToString());
						}
					}

					context.ClearModel();
				}
			}

			return optModelMap;
		}
	}
}