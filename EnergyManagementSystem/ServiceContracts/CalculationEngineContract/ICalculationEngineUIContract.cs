//-----------------------------------------------------------------------
// <copyright file="ICalculationEngineUIContract.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.ServiceContracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Calculation engine interfaces for UI
	/// </summary>
	[ServiceContract]
	public interface ICalculationEngineUIContract
	{
		/// <summary>
		/// Get History Measurements from database
		/// </summary>
		/// <param name="gid">Object gid</param>
		/// <returns>return list of measurements value and measurement time</returns>
		[OperationContract]
		List<Tuple<double, DateTime>> GetHistoryMeasurements(long gid, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Get Total Production from database
        /// </summary>
        /// <param name="startTime">Start time of period</param>
        /// <param name="endTime">End time of period</param>
        /// <returns>returns list of double and time of calculation</returns>
        [OperationContract] 
        List<Tuple<double, DateTime>> GetTotalProduction(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Get CO2 Emission from database
        /// </summary>
        /// <param name="startTime">Start time of period</param>
        /// <param name="endTime">End time of period</param>
        /// <returns>returns list of nonrenewable, renewable and measurementtime</returns>
        [OperationContract]
        List<Tuple<double, double, DateTime>> GetCO2Emission(DateTime startTime, DateTime endTime);
	}
}
