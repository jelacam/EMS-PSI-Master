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
        /// <param name="measurements">list of measurements which should be optimized</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool OptimisationAlgorithm(List<MeasurementUnit> measurements)
        {
            // throw new NotImplementedException();
            try
            {
                bool retVal = ce.Optimize(measurements);
                return retVal;
            }
            catch (Exception ex)
            {
                // string message = string.Format("Getting values for resource with ID = 0x{0:x16} failed. {1}", resourceId, ex.Message);
                string message = string.Format("Greska", ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }
    }
}