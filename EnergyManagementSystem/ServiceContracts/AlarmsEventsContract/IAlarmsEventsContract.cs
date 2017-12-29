//-----------------------------------------------------------------------
// <copyright file="IAlarmsEventsContract.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
	using System;
	using System.ServiceModel;

	/// <summary>
	/// Contract for AlarmsEvents
	/// </summary>
	[ServiceContract]
	public interface IAlarmsEventsContract
	{
		/// <summary>
		/// Test method
		/// </summary>
		[OperationContract]
		void Test();

		/// <summary>
		/// Checking for low and high alarm on raw value
		/// </summary>
		/// <param name="value">value to check</param>
		/// <param name="min">low limit</param>
		/// <param name="max">high limit</param>
		/// <returns>True if alarm exist</returns>
		[OperationContract]
		bool CheckForRawAlarms(float value, float min,float max);

		/// <summary>
		/// Checking for low and high alarm on egu value
		/// </summary>
		/// <param name="value">value to check</param>
		/// <param name="min">low limit</param>
		/// <param name="max">high limit</param>
		/// <returns>True if alarm exist</returns>
		[OperationContract]
		bool CheckForEGUAlarms(float value, float min, float max);
		
		//alarmi kad se optimizuje
	}
}
