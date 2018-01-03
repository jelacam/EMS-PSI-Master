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
	using System.Collections.Generic;
	using CommonMeasurement;

    /// <summary>
    /// Class for ICalculationEngineContract implementation
    /// </summary>
    public class AlarmsEvents : IAlarmsEventsContract
	{
        PublisherService publisher;
		private List<AlarmHelper> alarms;

		/// <summary>
		/// Initializes a new instance of the <see cref="AlarmsEvents" /> class
		/// </summary>
		public AlarmsEvents()
		{
            publisher = new PublisherService();
			this.Alarms = new List<AlarmHelper>();
		}

        public void PublishAlarmEvents(string alarm)
        {
            publisher.PublishAlarmsEvents(alarm);
		/// <summary>
		/// Gets or sets Alarms of the entity
		/// </summary>
		public List<AlarmHelper> Alarms
		{
			get
			{
				return this.alarms;
			}

			set
			{
				this.alarms = value;
			}
		}		
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

		/// <summary>
		/// Adds new alarm
		/// </summary>
		/// <param name="alarm">alarm to add</param>
		public void AddAlarm(AlarmHelper alarm)
		{	
			try
			{
				this.alarms.Add(alarm);
				Console.WriteLine("AlarmsEvents: AddAlarm method");
			}
			catch (Exception ex)
			{
				string message = string.Format("Greska ", ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}
		}
	}
}