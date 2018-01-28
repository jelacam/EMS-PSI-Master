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
        private Action<object> callbackAction;

        public Action<object> CallbackAction
        {
            get
            {
                return callbackAction;
            }

            set
            {
                callbackAction = value;
            }
        }

        public void AlarmsEvents(AlarmHelper alarm)
        {
            Console.WriteLine("SessionID id: {0}", OperationContext.Current.SessionId);
            Console.WriteLine(string.Format("ALARM: {0} on Signal GID: {1} | SessionID id: {2}",
                                            alarm.Value.ToString(), alarm.Gid.ToString(), OperationContext.Current.SessionId));

            CommonTrace.WriteTrace(CommonTrace.TraceInfo, string.Format("ALARM: {0} on Signal GID: {1} | SessionID id: {2}",
                                                                        alarm.Value.ToString(), alarm.Gid.ToString(), OperationContext.Current.SessionId));
            CallbackAction(alarm);
        }

        public void ChangeAlarmStatus(long gid, string currentState)
        {
            Console.WriteLine("SessionID id: {0}", OperationContext.Current.SessionId);
            Console.WriteLine(string.Format("Alarm status: {0} on Signal GID: {1} | SessionID id: {2}",
                                            currentState, gid.ToString(), OperationContext.Current.SessionId));

            CommonTrace.WriteTrace(CommonTrace.TraceInfo, string.Format("Alarm status: {0} on Signal GID: {1} | SessionID id: {2}",
                                                                         currentState, gid.ToString(), OperationContext.Current.SessionId));

            AlarmHelper ah = new AlarmHelper()
            {
                Gid = gid,
                CurrentState = currentState,
                StatusChange = true
            };

            CallbackAction(ah);
        }
    }
}