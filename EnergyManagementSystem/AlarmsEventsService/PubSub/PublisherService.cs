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

        public static event AlarmEventHandler AlarmEvent;

        private IAesPubSubCallbackContract callback = null;
        private AlarmEventHandler alarmEventHandler = null;

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

        /// <summary>
        /// Clients call this service opeartion to subscribe.
        /// A alarms events event handler is registered for this client instance.
        /// </summary>
        public void Subscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<IAesPubSubCallbackContract>();
            alarmEventHandler = new AlarmEventHandler(AlarmsEventsHandler);
            AlarmEvent += alarmEventHandler;
        }

        /// <summary>
        /// Clients call this service opeartion to unsubscribe.
        /// </summary>
        public void Unsubscribe()
        {
            AlarmEvent -= alarmEventHandler;
        }

        /// <summary>
        /// Information source, in our case it is SCADA Krunching component, call this service operation to report a new Alarm.
        /// An alarm event is raised. The alarm event handlers for each subscriber will execute.
        /// </summary>
        /// <param name="alarm"></param>
        public void PublishAlarmsEvents(AlarmHelper alarm)
        {
            // TODO dodati novi alarm u kolekciju alarma
            // ako se novi klijent instancira on ce od alarm servisa raditi integrity update za alarme i dobice listu svih alarma

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
        }
    }
}