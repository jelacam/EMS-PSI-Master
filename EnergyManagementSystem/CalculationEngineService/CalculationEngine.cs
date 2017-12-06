//-----------------------------------------------------------------------
// <copyright file="CalculationEngine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{    
    using System.Collections.Generic;
    using CommonMeasurement;

    /// <summary>
    /// Class for CalculationEngine
    /// </summary>
    public class CalculationEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationEngine" /> class
        /// </summary>
        public CalculationEngine()
        {
        }

        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measurements">list of measurements which should be optimized</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool Optimize(List<MeasurementUnit> measurements)
        {
            for (int i = 0; i < measurements.Count; i++)
            {
                measurements[i].CurrentValue = measurements[i].CurrentValue * 2;
            }

            return true;
        }
    }
}