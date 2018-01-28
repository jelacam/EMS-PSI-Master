using EMS.Common;
using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIClient.PubSub;

namespace UIClient.ViewModel
{
    public class AlarmSummaryViewModel : ViewModelBase
    {
        private AlarmsEventsSubscribeProxy aeSubscribeProxy;

        private ObservableCollection<AlarmHelper> alarmSummaryQueue = new ObservableCollection<AlarmHelper>();

        public ObservableCollection<AlarmHelper> AlarmSummaryQueue
        {
            get
            {
                return alarmSummaryQueue;
            }
            set
            {
                alarmSummaryQueue = value;
                OnPropertyChanged(nameof(AlarmSummaryQueue));
            }
        }

        public AlarmSummaryViewModel()
        {
            try
            {
                aeSubscribeProxy = new AlarmsEventsSubscribeProxy(CallbackAction);
                aeSubscribeProxy.Subscribe();
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Could not connect to Alarm Publisher Service! \n {0}", e.Message);
            }
        }

        private void CallbackAction(object obj)
        {
            AlarmHelper alarm = obj as AlarmHelper;

            if (obj == null)
            {
                throw new Exception("CallbackAction receive wrong param");
            }

            if (alarm.StatusChange)
            {
                ChangeAlarmSate(alarm);
            }
            else
            {
                AddAlarm(alarm);
            }
        }

        private void AddAlarm(AlarmHelper alarm)
        {
            foreach (AlarmHelper aHelper in AlarmSummaryQueue)
            {
                if (aHelper.Gid.Equals(alarm.Gid))
                {
                    aHelper.CurrentState = string.Format("{0}, {1}", State.Active, aHelper.AckState);
                    OnPropertyChanged(nameof(AlarmSummaryQueue));
                }
            }

            AlarmSummaryQueue.Add(alarm);

            OnPropertyChanged(nameof(AlarmSummaryQueue));
        }

        private void ChangeAlarmSate(AlarmHelper alarm)
        {
            foreach (AlarmHelper aHelper in AlarmSummaryQueue)
            {
                if (aHelper.Gid.Equals(alarm.Gid))
                {
                    aHelper.CurrentState = alarm.CurrentState;
                    OnPropertyChanged(nameof(AlarmSummaryQueue));
                }
            }
        }
    }
}