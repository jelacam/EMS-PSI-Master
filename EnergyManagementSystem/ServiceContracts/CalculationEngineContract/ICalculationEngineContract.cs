//-----------------------------------------------------------------------
// <copyright file="ICalculationEngineContract.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using CommonMeasurement;

    /// <summary>
    /// Contract for CalculationEngine
    /// </summary>
    [ServiceContract]
    public interface ICalculationEngineContract
    {
        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measurements">list of measurements which should be optimized</param>
        /// <returns>returns true if optimization was successful</returns>
        [OperationContract]
        bool OptimisationAlgorithm(List<MeasurementUnit> measurements);
    }
}
