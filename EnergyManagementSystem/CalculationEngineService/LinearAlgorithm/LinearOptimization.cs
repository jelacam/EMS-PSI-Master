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

		public float OptimizedRenewable { get; set; }
		public float LACostRenewable { get; set; }
		public float LACostWithoutRenewable { get; set; }
		public float LAProfit { get; set; }
		public float LACO2Renewable { get; set; }
		public float LACO2WithoutRenewable { get; set; }
		public float LAWind { get; set; }
		public float LAWindPct { get; set; }

		#endregion

		#region Constructor

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

			OptimizedRenewable = 0;

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
					Dictionary<long, OptimisationModel> optModelMapWithoutWind = new Dictionary<long, OptimisationModel>();

					foreach (var item in optModelMap)
					{
						if (!item.Value.EmsFuel.FuelType.Equals(EmsFuelType.wind))
						{
							optModelMapWithoutWind.Add(item.Key, item.Value);
						}
						else
						{
							production -= item.Value.MaxPower;
						}
					}

					optModelMapWithoutWind = StartLinearOptimization(optModelMapWithoutWind, consumption, false, production);
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
		/// <param name="includeWindGenerators">should wind generators be included</param>
		/// <param name="maxProductionLimit">maximum limit of production</param>
		/// <returns>linear optimized model</returns>
		private Dictionary<long, OptimisationModel> StartLinearOptimization(Dictionary<long, OptimisationModel> optModelMap, float consumption, bool includeWindGenerators, float maxProductionLimit)
		{
			lock (lockObj)
			{
				if (optModelMap.Count() > 0 && consumption >= 0 && consumption <= maxProductionLimit)
				{
					Model model = context.CreateModel();
					string goalCost = string.Empty;
					string goalMaximizeRenewable = string.Empty;
					string goalMinimizeNonRenewable = string.Empty;
					string constraintLimit = "Limit";
					string constraintProduction = consumption.ToString() + "<=";
					string name = string.Empty;
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
							goalMaximizeRenewable += d.ToString() + "+";
						}
						else
						{
							goalMinimizeNonRenewable += d.ToString() + "+";
						}
					}

					constraintProduction = constraintProduction.Substring(0, constraintProduction.Length - 1);
					constraintProduction += "<=" + maxProductionLimit.ToString();
					model.AddConstraint("Production", constraintProduction);

					goalCost = goalCost.Substring(0, goalCost.Length - 1);
					model.AddGoal("Cost", GoalKind.Minimize, goalCost);

					goalMaximizeRenewable = goalMaximizeRenewable.Substring(0, goalMaximizeRenewable.Length - 1);
					model.AddGoal("MaximizeRenewable", GoalKind.Maximize, goalMaximizeRenewable);

					goalMinimizeNonRenewable = goalMinimizeNonRenewable.Substring(0, goalMinimizeNonRenewable.Length - 1);
					model.AddGoal("MinimizeNonRenewable", GoalKind.Minimize, goalMinimizeNonRenewable);

					try
					{
						Solution solution = context.Solve(new SimplexDirective());
						Report report = solution.GetReport();
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

							if (includeWindGenerators) // sa vetrogeneratorima
							{
								//Console.Write("{0}", report);

								OptimizedRenewable += optModel.LinearOptimizedValue;
								LACO2Renewable += optModel.LinearOptimizedValue * optModel.EmissionFactor;
								LACostRenewable += ((float)optModel.Curve.A * optModel.LinearOptimizedValue + (float)optModel.Curve.B) * optModel.EmsFuel.UnitPrice;

								if (optModel.EmsFuel.FuelType.Equals(EmsFuelType.wind))
								{
									LAWind += optModel.LinearOptimizedValue;
								}
							}
							else // bez vetrogeneratora
							{
								LACO2WithoutRenewable += optModel.LinearOptimizedValue * optModel.EmissionFactor;
								LACostWithoutRenewable += ((float)optModel.Curve.A * optModel.LinearOptimizedValue + (float)optModel.Curve.B) * optModel.EmsFuel.UnitPrice;
							}
						}
					}

					if (includeWindGenerators)
					{
						LAWindPct = 100 * LAWind / OptimizedRenewable;
					}

					context.ClearModel();
				}
			}

			return optModelMap;
		}
	}
}