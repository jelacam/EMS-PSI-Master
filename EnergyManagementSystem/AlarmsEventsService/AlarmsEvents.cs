//-----------------------------------------------------------------------
// <copyright file="AlarmsEvents.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.AlarmsEventsService
{
    using System;
    using Common;
    using ServiceContracts;
    using PubSub;

    /// <summary>
    /// Class for ICalculationEngineContract implementation
    /// </summary>
    public class AlarmsEvents : IAlarmsEventsContract
	{
        PublisherService publisher;

		/// <summary>
		/// Initializes a new instance of the <see cref="AlarmsEvents" /> class
		/// </summary>
		public AlarmsEvents()
		{
            publisher = new PublisherService();
		}

        public void PublishAlarmEvents(string alarm)
        {
            publisher.PublishAlarmsEvents(alarm);
        }

        /// <summary>
        /// Test method
        /// </summary>
        public void Test()
		{
			try
			{
				Console.WriteLine("AlarmsEvents: Test method");
			}
			catch (Exception ex)
			{
				string message = string.Format("Greska", ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}
		}
	}
}