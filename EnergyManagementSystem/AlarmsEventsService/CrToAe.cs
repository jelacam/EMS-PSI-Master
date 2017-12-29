//-----------------------------------------------------------------------
// <copyright file="CrToAe.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.AlarmsEventsService
{
	using System;
	using Common;
	using ServiceContracts;

	/// <summary>
	/// Class for ICalculationEngineContract implementation
	/// </summary>
	public class CrToAe : IAlarmsEventsContract
	{
		/// <summary>
		/// AlarmsEvents instance
		/// </summary>
		private static AlarmsEvents ae = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="CrToAe" /> class
		/// </summary>
		public CrToAe()
		{
		}

		/// <summary>
		/// Sets AlarmsEvents of the entity
		/// </summary>
		public static AlarmsEvents AlarmsEvents
		{
			set
			{
				ae = value;
			}
		}

		/// <summary>
		/// Test method
		/// </summary>
		public void Test()
		{
			try
			{
				ae.Test();
			}
			catch (Exception ex)
			{
				string message = string.Format("Greska", ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}
		}

		public bool CheckForEGUAlarms(float value, float min, float max)
		{
			return false;
		}

		public bool CheckForRawAlarms(float value, float min, float max)
		{
			return false;
		}
	}
}