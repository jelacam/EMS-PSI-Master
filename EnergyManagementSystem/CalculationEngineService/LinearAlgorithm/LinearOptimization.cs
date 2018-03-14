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
	using System.Threading.Tasks;

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
		/// minimal production of generators
		/// </summary>
		private float minProduction = 0;

		/// <summary>
		/// maximal production of generators
		/// </summary>
		private float maxProduction = 0;

		#endregion

		#region Properties

		public float Optimized { get; set; }
		public float Cost { get; set; }
		public float CostWithoutWindAndSolar { get; set; }
		public float Profit { get; set; }
		public float CO2 { get; set; }
		public float CO2WithoutWindAndSolar { get; set; }

		public float PowerOfWind { get; set; }
		public float PowerOfSolar { get; set; }
		public float PowerOfHydro { get; set; }
		public float PowerOfCoal { get; set; }
		public float PowerOfOil { get; set; }
		public float PowerOfWindPct { get; set; }
		public float PowerOfSolarPct { get; set; }
		public float PowerOfHydroPct { get; set; }
		public float PowerOfCoalPct { get; set; }
		public float PowerOfOilPct { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="LinearOptimization" /> class
		/// </summary>
		/// <param name="minProduction">minimal production of generators</param>
		/// <param name="maxProduction">maximal production of generators</param>
		public LinearOptimization(float minProduction, float maxProduction)
		{
			Cost = 0;
			CostWithoutWindAndSolar = 0;
			Profit = 0;
			CO2 = 0;
			CO2WithoutWindAndSolar = 0;

			PowerOfWind = 0;
			PowerOfSolar = 0;
			PowerOfHydro = 0;
			PowerOfCoal = 0;
			PowerOfOil = 0;
			PowerOfWindPct = 0;
			PowerOfSolarPct = 0;
			PowerOfHydroPct = 0;
			PowerOfCoalPct = 0;
			PowerOfOilPct = 0;

			Optimized = 0;

			this.minProduction = minProduction;
			this.maxProduction = maxProduction;

			lockObj = new object();
			context = SolverContext.GetContext();
		}

		#endregion Constructor

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
					Dictionary<long, OptimisationModel> optModelMapWithoutWindAndSolar = new Dictionary<long, OptimisationModel>();

					foreach (var item in optModelMap)
					{
						if (!item.Value.Renewable) // wind + solar
						{
							optModelMapWithoutWindAndSolar.Add(item.Key, item.Value);
						}
						else
						{
							production -= item.Value.MaxPower;
						}
					}

					optModelMapWithoutWindAndSolar = StartLinearOptimization(optModelMapWithoutWindAndSolar, consumption, false, production);
					optModelMap = StartLinearOptimization(optModelMap, consumption, true, maxProduction);

					Profit = CostWithoutWindAndSolar - Cost;

					PowerOfWindPct = 100 * PowerOfWind / Optimized;
					PowerOfSolarPct = 100 * PowerOfSolar / Optimized;
					PowerOfHydroPct = 100 * PowerOfHydro / Optimized;
					PowerOfCoalPct = 100 * PowerOfCoal / Optimized;
					PowerOfOilPct = 100 * PowerOfOil / Optimized;
				}

				return optModelMap;
			}
		}

		/// <summary>
		/// Linear optimization algorithm
		/// </summary>
		/// <param name="optModelMap">model for optimization</param>
		/// <param name="consumption">current consumption</param>
		/// <param name="includeWindGenerators">should wind generators be included</param>
		/// <param name="maxProductionLimit">maximum limit of production</param>
		/// <returns>linear optimized model</returns>
		private Dictionary<long, OptimisationModel> StartLinearOptimization(Dictionary<long, OptimisationModel> optModelMap, float consumption, bool includeWindAndSolarGenerators, float maxProductionLimit)
		{
			lock (lockObj)
			{
				if (optModelMap.Count() > 0 && consumption >= 0 && consumption <= maxProductionLimit)
				{
					Model model = context.CreateModel();
					string goalCost = string.Empty;
					string goalMaximizeWindAndSolar = string.Empty;
					string goalMinimize = string.Empty;
					string constraintLimit = "Limit";
					string constraintProduction = consumption.ToString() + "<=";
					string name = string.Empty;
					float price = 0;
					Term termLimit;

					foreach (var optModel in optModelMap)
					{
						Decision d = new Decision(Domain.RealNonnegative, "d" + optModel.Value.GlobalId.ToString());
						model.AddDecision(d);

						termLimit = optModel.Value.MinPower <= d <= optModel.Value.MaxPower;
						model.AddConstraint(constraintLimit + optModel.Value.GlobalId, termLimit);

						constraintProduction += d.ToString() + "+";
						goalCost += "(" + optModel.Value.Curve.A.ToString() + "*" + d.ToString() + "+" + optModel.Value.Curve.B.ToString() + ")*" + optModel.Value.EmsFuel.UnitPrice.ToString() + "+";

						if (optModel.Value.Renewable)
						{
							goalMaximizeWindAndSolar += d.ToString() + "+";
						}
						else
						{
							goalMinimize += d.ToString() + "+";
						}
					}

					constraintProduction = constraintProduction.Substring(0, constraintProduction.Length - 1);
					constraintProduction += "<=" + maxProductionLimit.ToString();
					model.AddConstraint("Production", constraintProduction);

					goalCost = goalCost.Substring(0, goalCost.Length - 1);
					model.AddGoal("Cost", GoalKind.Minimize, goalCost);

					goalMinimize = goalMinimize.Substring(0, goalMinimize.Length - 1);
					model.AddGoal("Minimize", GoalKind.Minimize, goalMinimize);

					if (includeWindAndSolarGenerators)
					{
						goalMaximizeWindAndSolar = goalMaximizeWindAndSolar.Substring(0, goalMaximizeWindAndSolar.Length - 1);
						model.AddGoal("MaximizeWindAndSolar", GoalKind.Maximize, goalMaximizeWindAndSolar);
					}

					Report report;

					try
					{
						Solution solution = context.Solve(new SimplexDirective());
						report = solution.GetReport();
					}
					catch(Exception ex)
					{
						throw new Exception("LinearOptimization] Exception = " + ex.Message);
					}

					foreach (Decision item in model.Decisions)
					{
						name = item.Name.Substring(1);
						OptimisationModel optModel = null;
						if (optModelMap.TryGetValue(long.Parse(name), out optModel))
						{
							optModel.LinearOptimizedValue = float.Parse(item.ToDouble().ToString());

							if (includeWindAndSolarGenerators) // sa vetrogeneratorima i solarima
							{
								// Console.Write("{0}", report);
								Optimized += optModel.LinearOptimizedValue;
								CO2 += optModel.LinearOptimizedValue * optModel.EmissionFactor;
								price = ((float)optModel.Curve.A * optModel.LinearOptimizedValue + (float)optModel.Curve.B) * optModel.EmsFuel.UnitPrice;
								optModel.Price = price;
								Cost += price;

								switch (optModel.EmsFuel.FuelType)
								{
									case EmsFuelType.coal:
										PowerOfCoal += optModel.LinearOptimizedValue;
										break;
									case EmsFuelType.hydro:
										PowerOfHydro += optModel.LinearOptimizedValue;
										break;
									case EmsFuelType.oil:
										PowerOfOil += optModel.LinearOptimizedValue;
										break;
									case EmsFuelType.solar:
										PowerOfSolar += optModel.LinearOptimizedValue;
										break;
									case EmsFuelType.wind:
										PowerOfWind += optModel.LinearOptimizedValue;
										break;
									default:
										break;
								}								
							}
							else // bez vetrogeneratora i solara
							{
								CO2WithoutWindAndSolar += optModel.LinearOptimizedValue * optModel.EmissionFactor;
								CostWithoutWindAndSolar += ((float)optModel.Curve.A * optModel.LinearOptimizedValue + (float)optModel.Curve.B) * optModel.EmsFuel.UnitPrice;
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