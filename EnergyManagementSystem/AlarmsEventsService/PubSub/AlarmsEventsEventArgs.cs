using System;

namespace EMS.Services.AlarmsEventsService.PubSub
{
    public class AlarmsEventsEventArgs : EventArgs
    {
        private string alarm;

        public string Alarm
        {
            get { return alarm; }
            set { alarm = value; }
        }
    }
}