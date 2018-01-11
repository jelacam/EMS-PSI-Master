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
        /// <summary>
        /// entity for storing PublisherService instance
        /// </summary>
        private PublisherService publisher;

        /// <summary>
        /// list for storing AlarmHelper entities
        /// </summary>
        private List<AlarmHelper> alarms;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmsEvents" /> class
        /// </summary>
        public AlarmsEvents()
        {
            this.Publisher = new PublisherService();
            this.Alarms = new List<AlarmHelper>();
        }

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

        /// <summary>
        /// Gets or sets Publisher of the entity
        /// </summary>
        public PublisherService Publisher
        {
            get
            {
                return this.publisher;
            }

            set
            {
                this.publisher = value;
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
                this.Alarms.Add(alarm);
                this.Publisher.PublishAlarmsEvents(alarm);
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