using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayService.PubSub
{
    public class AlarmUpdateEventArgs : EventArgs
    {
        private AlarmHelper alarm;

        public AlarmHelper Alarm
        {
            get
            {
                return alarm;
            }
            set
            {
                alarm = value;
            }
        }
    }
}