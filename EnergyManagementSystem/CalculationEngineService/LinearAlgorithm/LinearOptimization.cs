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

		#endregion Fields

		/// <summary>
		/// Initializes a new instance of the <see cref="LinearOptimization" /> class
		/// </summary>
		/// <param name="minProduction">minimal production of generators</param>
		/// <param name="maxProduction">maximal production of generators</param>
		public LinearOptimization(float minProduction, float maxProduction)
		{
			TotalCost = 0;

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
		/// Starts linear optimization algorithm
		/// </summary>
		/// <param name="optModelMap">data about generators</param>
		/// <param name="consumption">total consumption of consumers</param>
		/// <param name="windSpeed">speed of wind for wind generators</param>
		/// <returns>optimized values for generators</returns>
		public Dictionary<long, OptimisationModel> Start(Dictionary<long, OptimisationModel> optModelMap, float consumption, float windSpeed)
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

					if (consumption >= 0 && consumption <= maxProduction)
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
						production += "<=" + maxProduction.ToString();
						model.AddConstraint("production", production);

						goal = goal.Substring(0, goal.Length - 1);
						model.AddGoal("cost", GoalKind.Minimize, goal);

						Solution solution = context.Solve(new SimplexDirective());
						Report report = solution.GetReport();
						Console.Write("{0}", report);

						TotalCost = float.Parse(model.Goals.FirstOrDefault().ToDouble().ToString());
						float optimized = 0;

						string name = string.Empty;
						foreach (var item in model.Decisions)
						{
							name = item.Name.Substring(1);
							OptimisationModel optModel = null;
							if (optModelMap.TryGetValue(long.Parse(name), out optModel))
							{
								optModel.LinearOptimizedValue = float.Parse(item.ToDouble().ToString());
								optimized += optModel.LinearOptimizedValue;
							}
						}

						Console.WriteLine("Linear optimization: {0}", optimized);
						Console.WriteLine("Wind: {0}", windSpeed);
					}

					context.ClearModel();
				}
			}

			return optModelMap;
		}
	}
}