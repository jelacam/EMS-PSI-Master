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
	}
}
