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
		private float totalCostNonRenewable = 0;

		/// <summary>
		/// minimal production of generators
		/// </summary>
		private float minProduction = 0;

		/// <summary>
		/// maximal production of generators
		/// </summary>
		private float maxProduction = 0;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets total production
		/// </summary>
		public float OptimizedLinear { get; set; }		

		public float LACostRenewable { get; set; }
		public float LACostWithoutRenewable { get; set; }
		public float LAProfit { get; set; }
		public float LACO2Renewable { get; set; }
		public float LACO2WithoutRenewable { get; set; }
		public float LAWind { get; set; }
		public float LAWindPct { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="LinearOptimization" /> class
		/// </summary>
		/// <param name="minProduction">minimal production of generators</param>
		/// <param name="maxProduction">maximal production of generators</param>
		public LinearOptimization(float minProduction, float maxProduction)
		{
			LACostRenewable = 0;
			LACostWithoutRenewable = 0;
			LAProfit = 0;
			LACO2Renewable = 0;
			LACO2WithoutRenewable = 0;
			LAWind = 0;
			LAWindPct = 0;
			
			OptimizedLinear = 0;

			this.minProduction = minProduction;
			this.maxProduction = maxProduction;

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

					optModelMapNonRenewable = StartLinearOptimization(optModelMapNonRenewable, consumption, false, maxProduction);
					optModelMap = StartLinearOptimization(optModelMap, consumption, true, maxProduction);

					LAProfit = LACostWithoutRenewable - LACostRenewable;
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
						string goalRenewable = string.Empty;
						string goalNonRenewable = string.Empty;
						string limit = "limit";
						string production = consumption.ToString() + "<=";
						Term termLimit;

						foreach (var optModel in optModelMap)
						{
							help = decisions[optModel.Value.GlobalId];

							termLimit = optModel.Value.MinPower <= help <= optModel.Value.MaxPower;
							model.AddConstraint(limit + optModel.Value.GlobalId, termLimit);

							production += help.ToString() + "+";
							goal += "(" + optModel.Value.Curve.A.ToString() + "*" + help.ToString() + "+" + optModel.Value.Curve.B.ToString() + ")*" + optModel.Value.EmsFuel.UnitPrice.ToString() + "+";

							if (optModel.Value.Renewable)
							{
								goalRenewable += help.ToString() + "+";
							}
							else
							{
								goalNonRenewable += help.ToString() + "+";
							}
						}

						production = production.Substring(0, production.Length - 1);
						production += "<=" + maxProductionLimit.ToString();
						model.AddConstraint("production", production);

						goal = goal.Substring(0, goal.Length - 1);
						model.AddGoal("cost", GoalKind.Minimize, goal);

						goalRenewable = goalRenewable.Substring(0, goalRenewable.Length - 1);
						model.AddGoal("renewableMax", GoalKind.Maximize, goalRenewable);

						goalNonRenewable = goalNonRenewable.Substring(0, goalNonRenewable.Length - 1);
						model.AddGoal("nonRenewableMin", GoalKind.Minimize, goalNonRenewable);

						Solution solution = context.Solve(new SimplexDirective());
						Report report = solution.GetReport();

						if (renewable) //sa vetrogeneratorima
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
									LACO2Renewable += optModel.LinearOptimizedValue * optModel.EmissionFactor;
									LACostRenewable += ((float)optModel.Curve.A * optModel.LinearOptimizedValue + (float)optModel.Curve.B) * optModel.EmsFuel.UnitPrice;

									if (optModel.EmsFuel.FuelType.Equals(EmsFuelType.wind))
									{
										LAWind += optModel.LinearOptimizedValue;
									}
								}
							}

							LAWindPct = 100 * LAWind / OptimizedLinear;

							//Console.WriteLine("Linear optimization: {0}kW", OptimizedLinear);
							//Console.WriteLine("Linear optimization wind: {0}kW ({1}%)", LAWind, LAWindPct);
							//Console.WriteLine("Linear optimization with renewable CO2: {0}", LACO2Renewable);

						}
						else //bez vetrogeneratora
						{
							string name = string.Empty;
							foreach (var item in model.Decisions)
							{
								name = item.Name.Substring(1);
								OptimisationModel optModel = null;
								if (optModelMap.TryGetValue(long.Parse(name), out optModel))
								{
									optModel.LinearOptimizedValue = float.Parse(item.ToDouble().ToString());
									LACO2WithoutRenewable += optModel.LinearOptimizedValue * optModel.EmissionFactor;
									LACostWithoutRenewable += ((float)optModel.Curve.A * optModel.LinearOptimizedValue + (float)optModel.Curve.B) * optModel.EmsFuel.UnitPrice;
								}
							}

							//Console.WriteLine("Linear optimization without renewable CO2: {0}", LACO2WithoutRenewable);
						}
					}

					context.ClearModel();
				}
			}

			return optModelMap;
		}
	}
}