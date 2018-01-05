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

    /// <summary>
    /// Class for CalculationEngine
    /// </summary>
    public class CalculationEngine
    {
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
        /// <returns></returns>
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
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high optimized limit on gid: {0} - Sinal value: {1}"  , gid, value);
                Console.WriteLine("Alarm on high optimized limit on gid: {0}  - Sinal value: {1}", gid, value);
            }

            return retVal;
        }
    }
}