using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.AlarmsEventsService.PubSub
{
    public class AlarmStateEventArgs : EventArgs
    {
        private long gid;
        private string currentState;

        public long Gid
        {
            get { return gid; }
            set { gid = value; }
        }

        public string CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }
    }
}