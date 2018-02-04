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
		/// <param name="measEnergyConsumers">list of measurements for energyConsumers</param>
		/// <param name="measGenerators">list of measurements for generators</param>
		/// <param name="windSpeed">speed of wind</param>
		/// <param name="sunlight">sunlight percent</param>
		/// <returns>returns true if optimization was successful</returns>
		[OperationContract]
		bool OptimisationAlgorithm(List<MeasurementUnit> measEnergyConsumers, List<MeasurementUnit> measGenerators, float windSpeed, float sunlight);
    }
}
