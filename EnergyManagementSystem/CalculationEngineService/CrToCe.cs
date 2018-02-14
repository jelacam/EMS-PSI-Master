//-----------------------------------------------------------------------
// <copyright file="CrToCe.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
    using System;
    using System.Collections.Generic;
    using Common;
    using CommonMeasurement;
    using ServiceContracts;

    /// <summary>
    /// Class for ICalculationEngineContract implementation
    /// </summary>
    public class CrToCe : ICalculationEngineContract
    {
        /// <summary>
        /// CalculationEngine instance
        /// </summary>
        private static CalculationEngine ce = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrToCe" /> class
        /// </summary>
        public CrToCe()
        {
        }

        /// <summary>
        /// Sets CalculationEngine of the entity
        /// </summary>
        public static CalculationEngine CalculationEngine
        {
            set
            {
                ce = value;
            }
        }

        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measEnergyConsumers">list of measurements for energyConsumers</param>
        /// <param name="measGenerators">list of measurements for generators</param>
        /// <param name="windSpeed">speed of wind</param>
        /// <param name="sunlight">sunlight percent</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool OptimisationAlgorithm(List<MeasurementUnit> measEnergyConsumers, List<MeasurementUnit> measGenerators, float windSpeed, float sunlight)
        {
            bool retVal = false;
            try
            {
                retVal = ce.Optimize(measEnergyConsumers, measGenerators, windSpeed, sunlight);
            }
            catch (Exception ex)
            {
                string message = string.Format("Error: {0}", ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            return retVal;
        }
    }
}