//-----------------------------------------------------------------------
// <copyright file="CalculationEngine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
	using System;
	using System.Collections.Generic;
	using CommonMeasurement;
	using EMS.Common;
	using EMS.ServiceContracts;
	using PubSub;
	using Microsoft.SolverFoundation.Common;
	using Microsoft.SolverFoundation.Services;
	using NetworkModelService.DataModel.Wires;

	/// <summary>
	/// Class for CalculationEngine
	/// </summary>
	public class CalculationEngine
	{
		private Dictionary<long, SynchronousMachine> generators = new Dictionary<long, SynchronousMachine>();
		private float powerOfConsumers = 0;
		private float totalCostLinear = 0;
		private SolverContext context = SolverContext.GetContext();
		private Model model;
		private List<OptimisationModel> loms;

		PublisherService publisher = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="CalculationEngine" /> class
		/// </summary>
		public CalculationEngine()
		{
			publisher = new PublisherService();
		}

		/// <summary>
		/// Optimization algorithm
		/// </summary>
		/// <param name="measurements">list of measurements which should be optimized</param>
		/// <returns>returns true if optimization was successful</returns>
		public bool Optimize(List<MeasurementUnit> measurements)
		{
			// List<MeasurementUnit> l = this.LinearOptimization(measurements);

			bool alarmOptimized = true;
			bool result = false;
			if (measurements != null)
			{
				if (measurements.Count > 0)
				{
					Console.WriteLine("CE: Optimize");
					for (int i = 0; i < measurements.Count; i++)
					{
						measurements[i].CurrentValue = measurements[i].CurrentValue * 2;

						alarmOptimized = this.CheckForOptimizedAlarms(measurements[i].CurrentValue, measurements[i].MinValue, measurements[i].MaxValue, measurements[i].Gid);
						if (alarmOptimized == false)
						{
							CommonTrace.WriteTrace(CommonTrace.TraceInfo, "gid: {0} value: {1}", measurements[i].Gid, measurements[i].CurrentValue);
							Console.WriteLine("gid: {0} value: {1}", measurements[i].Gid, measurements[i].CurrentValue);
						}
						else
						{
							CommonTrace.WriteTrace(CommonTrace.TraceInfo, "gid: {0} value: {1} ALARM!", measurements[i].Gid, measurements[i].CurrentValue);
							Console.WriteLine("gid: {0} value: {1} ALARM!", measurements[i].Gid, measurements[i].CurrentValue);
						}

						MeasurementUI measUI = new MeasurementUI()
						{
							Gid = measurements[i].Gid,
							AlarmType = alarmOptimized ? "Alarm while optimizing" : string.Empty,
							MeasurementValue = measurements[i].CurrentValue
						};

						try
						{
							publisher.PublishOptimizationResults(measUI);
						}
						catch (Exception ex)
						{

							throw ex;
						}
					}

					if (alarmOptimized == false)
					{
						result = true;
					}

					try
					{
						if (ScadaCMDProxy.Instance.SendDataToSimulator(measurements))
						{
							CommonTrace.WriteTrace(CommonTrace.TraceInfo, "CE sent {0} optimized MeasurementUnit(s) to SCADACommanding.", measurements.Count);
							Console.WriteLine("CE sent {0} optimized MeasurementUnit(s) to SCADACommanding.", measurements.Count);
						}
					}
					catch (System.Exception ex)
					{
						CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
						CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Checking for alarms on optimized value
		/// </summary>
		/// <param name="value">value to check</param>
		/// <param name="minOptimized">low limit</param>
		/// <param name="maxOptimized">high limit</param>
		/// <param name="gid">gid of measurement</param>
		/// <returns>True if alarm occures</returns>
		private bool CheckForOptimizedAlarms(float value, float minOptimized, float maxOptimized, long gid)
		{
			bool retVal = false;
			if (value < minOptimized)
			{
				retVal = true;
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low optimized limit on gid: {0}", gid);
				Console.WriteLine("Alarm on low optimized limit on gid: {0}", gid);
			}

			if (value > maxOptimized)
			{
				retVal = true;
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high optimized limit on gid: {0} - Sinal value: {1}", gid, value);
				Console.WriteLine("Alarm on high optimized limit on gid: {0}  - Sinal value: {1}", gid, value);
			}

			return retVal;
		}

		private List<MeasurementUnit> LinearOptimization(List<MeasurementUnit> measurements)
		{
			if (measurements.Count > 0)
			{
				model = context.CreateModel();

				bool alarmOptimized = false;
				loms = new List<OptimisationModel>();
				Dictionary<long, Decision> decisions = new Dictionary<long, Decision>();

				for (int i = 0; i < measurements.Count; i++)
				{
					OptimisationModel om = new OptimisationModel(generators[measurements[i].Gid], measurements[i]);
					loms.Add(om);

					Decision d = new Decision(Domain.RealNonnegative, om.GlobalId.ToString());
					decisions.Add(om.GlobalId, d);
					model.AddDecision(d);
				}

				string limitMax = "limitMax";
				string limitMin = "limitMin";
				string managable = "";
				string goal = "";
				for (int i = 0; i < loms.Count; i++)
				{
					Term tLimitMax = decisions[loms[i].GlobalId] + "<=" + loms[i].MaxPower;
					model.AddConstraint(limitMax + loms[i].GlobalId, tLimitMax);

					Term tLimitMin = decisions[loms[i].GlobalId] + ">=" + loms[i].MinPower;
					model.AddConstraint(limitMin + loms[i].GlobalId, tLimitMin);

					managable += decisions[loms[i].GlobalId].ToString() + "*" + loms[i].Managable.ToString();

					goal += decisions[loms[i].GlobalId].ToString() + "*" + loms[i].Price.ToString();
				}
				managable += "<=" + this.powerOfConsumers.ToString();
				Term tManagable = managable;
				model.AddConstraint("managable", tManagable);

				Term tGoal = goal;
				model.AddGoal("cost", GoalKind.Minimize, tGoal);

				Solution solution = context.Solve(new SimplexDirective());
				Report report = solution.GetReport();
				Console.Write("{0}", report);

				foreach (var item in model.Decisions)
				{
					for (int i = 0; i < measurements.Count; i++)
					{
						if (Int64.Parse(item.Name) == (measurements[i].Gid))
						{
							measurements[i].OptimizedLinear = float.Parse(item.ToString());						
						}
					}
				}

				context.ClearModel();
			}

			return measurements;
		}
	}
}