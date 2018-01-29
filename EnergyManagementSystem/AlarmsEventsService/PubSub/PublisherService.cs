namespace EMS.Services.AlarmsEventsService.PubSub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ServiceModel;
    using ServiceContracts;
    using Common;
    using CommonMeasurement;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PublisherService : IAesPubSubContract
    {
        public delegate void AlarmEventHandler(object sender, AlarmsEventsEventArgs e);

        public delegate void AlarmUpdateHandler(object sender, AlarmUpdateEventArgs e);

        public static event AlarmEventHandler AlarmEvent;

        public static event AlarmUpdateHandler AlarmUpdate;

        private IAesPubSubCallbackContract callback = null;
        private AlarmEventHandler alarmEventHandler = null;
        private AlarmUpdateHandler alarmUpdateHandler = null;

        /// <summary>
        /// This event handler runs when a AlarmsEvents event is raised.
        /// The client's AlarmsEvents operation is invoked to provide notification about the new alarm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AlarmsEventsHandler(object sender, AlarmsEventsEventArgs e)
        {
            callback.AlarmsEvents(e.Alarm);
        }

        public void AlarmUpdateEventsHandler(object sender, AlarmUpdateEventArgs e)
        {
            callback.UpdateAlarmsEvents(e.Alarm);
        }

        /// <summary>
        /// Clients call this service opeartion to subscribe.
        /// A alarms events event handler is registered for this client instance.
        /// </summary>
        public void Subscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<IAesPubSubCallbackContract>();

            alarmEventHandler = new AlarmEventHandler(AlarmsEventsHandler);
            AlarmEvent += alarmEventHandler;

            alarmUpdateHandler = new AlarmUpdateHandler(AlarmUpdateEventsHandler);
            AlarmUpdate += alarmUpdateHandler;
        }

        /// <summary>
        /// Clients call this service opeartion to unsubscribe.
        /// </summary>
        public void Unsubscribe()
        {
            AlarmEvent -= alarmEventHandler;
            AlarmUpdate -= alarmUpdateHandler;
        }

        /// <summary>
        /// Information source, in our case it is SCADA Krunching component, call this service operation to report a new Alarm.
        /// An alarm event is raised. The alarm event handlers for each subscriber will execute.
        /// </summary>
        /// <param name="alarm"></param>
        public void PublishAlarmsEvents(AlarmHelper alarm, PublishingStatus status)
        {
            switch(status)
            {
                case PublishingStatus.INSERT:
                {
                    AlarmsEventsEventArgs e = new AlarmsEventsEventArgs()
                    {
                        Alarm = alarm
                    };

                    try
                    {
                        AlarmEvent(this, e);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("AES does not have any subscribed client for publishing new alarms. {0}", ex.Message);
                        CommonTrace.WriteTrace(CommonTrace.TraceVerbose, message);
                        Console.WriteLine(message);
                    }

                    break;
                }

                case PublishingStatus.UPDATE:
                {
                    AlarmUpdateEventArgs e = new AlarmUpdateEventArgs()
                    {
                        Alarm = alarm
                    };

                    try
                    {
                        AlarmUpdate(this, e);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("AES does not have any subscribed client for publishing alarm status change. {0}", ex.Message);
                        CommonTrace.WriteTrace(CommonTrace.TraceVerbose, message);
                        Console.WriteLine(message);
                    }
                    break;
                }
            }

           
        }

        public void PublishStateChange(AlarmHelper alarm)
        {
            AlarmUpdateEventArgs e = new AlarmUpdateEventArgs()
            {
                Alarm = alarm
            
            };

            try
            {
                AlarmUpdate(this, e);
            }
            catch (Exception ex)
            {
                string message = string.Format("AES does not have any subscribed client for publishing alarm status change. {0}", ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, message);
                Console.WriteLine(message);
            }
        }
    }
}