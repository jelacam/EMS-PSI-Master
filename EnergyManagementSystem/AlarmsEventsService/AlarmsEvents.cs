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
    using System.ServiceModel;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// Class for ICalculationEngineContract implementation
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AlarmsEvents : IAlarmsEventsContract, IAesIntegirtyContract
    {
        /// <summary>
        /// entity for storing PublisherService instance
        /// </summary>
        private PublisherService publisher;

        /// <summary>
        /// list for storing AlarmHelper entities
        /// </summary>
        private List<AlarmHelper> alarms;

        private readonly int DEADBAND_VALUE = 20;

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
            PublishingStatus publishingStatus = PublishingStatus.INSERT;
            try
            {
                alarm.AckState = AckState.Unacknowledged;
                alarm.CurrentState = string.Format("{0}, {1}", State.Active, alarm.AckState);
                
                // deadband check
                foreach (AlarmHelper item in Alarms)
                {
                    if (item.Gid.Equals(alarm.Gid))
                    {
                        if (Math.Abs(alarm.Value - item.Value) < DEADBAND_VALUE)
                        {
                            // vrednost na signalu je u granicama deadbanda
                            publishingStatus = PublishingStatus.UPDATE;
                            alarm.PubStatus = publishingStatus;
                            item.Value = alarm.Value;
                            item.CurrentState = alarm.CurrentState;
                            item.LastChange = alarm.TimeStamp;
                        }
                    }
                }
                // ako je insert dodaj u listu - inace je updateovan
                if (publishingStatus.Equals(PublishingStatus.INSERT))
                {
                    alarm.InitiatingValue = alarm.Value;
                    this.Alarms.Add(alarm);
                    if (InsertAlarmIntoDb(alarm))
                    {
                        Console.WriteLine("Alarm with GID:{0} recorded into alarms database.",alarm.Gid);
                    }
                }

                this.Publisher.PublishAlarmsEvents(alarm, publishingStatus);
                //Console.WriteLine("AlarmsEvents: AddAlarm method");
                string message = string.Format("Alarm on Analog Gid: {0} - Value: {1}", alarm.Gid, alarm.Value);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            }
            catch (Exception ex)
            {
                string message = string.Format("Greska ", ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                //throw new Exception(message);
            }
        }

        public void UpdateStatus(AnalogLocation analogLoc, State state)
        {
            long powerSystemResGid = analogLoc.Analog.PowerSystemResource;
            foreach (AlarmHelper alarm in this.Alarms)
            {
                if (alarm.Gid.Equals(powerSystemResGid))
                {
                    alarm.CurrentState = string.Format("{0}, {1}", state, alarm.AckState);
                    alarm.PubStatus = PublishingStatus.UPDATE;
                    try
                    {
                        this.Publisher.PublishStateChange(alarm);
                        string message = string.Format("Alarm on Gid: {0} - Changed status: {1}", alarm.Gid, alarm.CurrentState);
                        CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Greska ", ex.Message);
                        CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                        // throw new Exception(message);
                    }
                }
            }
        }

        public List<AlarmHelper> InitiateIntegrityUpdate()
        {
            string message = string.Format("UI client requested integirty update for existing alarms.");
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            return Alarms;
        }

        private bool InsertAlarmIntoDb(AlarmHelper alarm)
        {
            bool success = true;

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertAlarm", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@gid", SqlDbType.BigInt).Value = alarm.Gid;
                        cmd.Parameters.Add("@alarmValue", SqlDbType.Float).Value = alarm.Value;
                        cmd.Parameters.Add("@minValue", SqlDbType.Float).Value = alarm.MinValue;
                        cmd.Parameters.Add("@maxValue", SqlDbType.Float).Value = alarm.MaxValue;
                        cmd.Parameters.Add("@timeStamp", SqlDbType.DateTime).Value = alarm.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@severity", SqlDbType.Int).Value = alarm.Severity;
                        cmd.Parameters.Add("@initiatingValue", SqlDbType.Float).Value = alarm.InitiatingValue;
                        cmd.Parameters.Add("@lastChange", SqlDbType.DateTime).Value = alarm.LastChange.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@currentState", SqlDbType.VarChar).Value = alarm.CurrentState;
                        cmd.Parameters.Add("@ackState", SqlDbType.Int).Value = alarm.AckState;
                        cmd.Parameters.Add("@pubStatus", SqlDbType.Int).Value = alarm.PubStatus;
                        cmd.Parameters.Add("@alarmType", SqlDbType.Int).Value = alarm.Type;
                        cmd.Parameters.Add("@persistent", SqlDbType.Int).Value = alarm.Persistent;
                        cmd.Parameters.Add("@inhibit", SqlDbType.Int).Value = alarm.Inhibit;
                        cmd.Parameters.Add("@message", SqlDbType.NText).Value = alarm.Message;

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    success = false;
                    string message = string.Format("Failed to insert alarm into database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return success;
        }
    }
}