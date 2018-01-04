using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.CommonMeasurement;
using System.ServiceModel;
using EMS.Common;

namespace UIClient.PubSub
{
    public class AePubSubCallbackService : IAesPubSubCallbackContract
    {
        public void AlarmsEvents(AlarmHelper alarm)
        {
            Console.WriteLine("SessionID id: {0}", OperationContext.Current.SessionId);
            Console.WriteLine(string.Format("ALARM: {0} on Signal GID: {1} | SessionID id: {2}", 
                                            alarm.Value.ToString(), alarm.Gid.ToString(), OperationContext.Current.SessionId));

            CommonTrace.WriteTrace(CommonTrace.TraceInfo, string.Format("ALARM: {0} on Signal GID: {1} | SessionID id: {2}",
                                                                        alarm.Value.ToString(), alarm.Gid.ToString(), OperationContext.Current.SessionId));
        }
    }
}
