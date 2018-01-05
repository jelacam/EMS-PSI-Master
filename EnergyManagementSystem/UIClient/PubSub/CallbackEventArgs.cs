using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIClient.PubSub
{
    public class CallbackEventArgs:EventArgs
    {
        public object EventData;
    }
}
