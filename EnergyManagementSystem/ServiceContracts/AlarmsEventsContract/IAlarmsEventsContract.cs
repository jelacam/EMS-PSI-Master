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
	}
}
