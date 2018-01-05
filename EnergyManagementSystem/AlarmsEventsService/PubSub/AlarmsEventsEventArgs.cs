using EMS.CommonMeasurement;
using System;

namespace EMS.Services.AlarmsEventsService.PubSub
{
    public class AlarmsEventsEventArgs : EventArgs
    {
        private AlarmHelper alarm;

        public AlarmHelper Alarm
        {
            get { return alarm; }
            set { alarm = value; }
        }
    }
}