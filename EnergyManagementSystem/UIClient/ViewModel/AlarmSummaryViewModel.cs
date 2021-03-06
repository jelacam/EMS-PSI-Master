﻿using EMS.Common;
using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UIClient.PubSub;

namespace UIClient.ViewModel
{
    public class AlarmSummaryViewModel : ViewModelBase
    {
        private AlarmsEventsSubscribeProxy aeSubscribeProxy;

        private ObservableCollection<AlarmHelper> alarmSummaryQueue = new ObservableCollection<AlarmHelper>();
        private ICommand acknowledgeCommand;

        public object alarmSummaryLock = new object();

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
            Title = "Alarm Summary";
            try
            {
                aeSubscribeProxy = new AlarmsEventsSubscribeProxy(CallbackAction);
                aeSubscribeProxy.Subscribe();
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Could not connect to Alarm Publisher Service! \n {0}", e.Message);
            }

            try
            {
                IntegirtyUpdate();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Successfully finished Integirty update operation for existing Alarms on AES! \n");
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Could not connect to Alarm Events Service for Integirty update operation! \n {0}", e.Message);
            }
        }

        public ICommand AcknowledgeCommand => acknowledgeCommand ?? (acknowledgeCommand = new RelayCommand<AlarmHelper>(AcknowledgeCommandExecute));

        private void AcknowledgeCommandExecute(AlarmHelper alarmHelper)
        {
            AlarmHelper alarmToRemove = new AlarmHelper();
            if (alarmHelper == null)
            {
                return;
            }

            if (alarmHelper.AckState == AckState.Unacknowledged)
            {
                lock (alarmSummaryLock)
                {
                    foreach (AlarmHelper alarm in AlarmSummaryQueue)
                    {
                        if (alarm.Gid.Equals(alarmHelper.Gid) && alarm.Persistent.Equals(PersistentState.Nonpersistent))
                        {
                            alarmToRemove = alarm;
                            break;
                        }
                        else if (alarm.Gid.Equals(alarmHelper.Gid) && alarm.Persistent.Equals(PersistentState.Persistent))
                        {
                            alarm.AckState = AckState.Acknowledged;
                            alarm.CurrentState = string.Format("{0} | {1}", alarm.CurrentState.Contains(State.Cleared.ToString()) ? State.Cleared.ToString() : State.Active.ToString(), alarm.AckState.ToString());
                            OnPropertyChanged(nameof(AlarmSummaryQueue));
                            CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Persistent alarm acknowledged");
                        }
                    }
                    if (alarmToRemove != null)
                    {
                        AlarmSummaryQueue.Remove(alarmToRemove);
                        OnPropertyChanged(nameof(AlarmSummaryQueue));
                        CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Non persistent alarm acknowledged and removed from alarm summary collection");
                    }
                }
            }
            else
            {
                alarmHelper.AckState = AckState.Unacknowledged;
                string state = alarmHelper.CurrentState.Contains(State.Cleared.ToString()) ? State.Cleared.ToString() : State.Active.ToString();
                alarmHelper.CurrentState = string.Format("{0} | {1}", state, alarmHelper.AckState.ToString());
                OnPropertyChanged(nameof(AlarmSummaryQueue));
            }

            //string str = alarmHelper.CurrentState;
            //str = str.Substring(0,str.IndexOf(",")+1);
            //alarmHelper.CurrentState = str + " " + alarmHelper.AckState;
        }

        private void CallbackAction(object obj)
        {
            AlarmHelper alarm = obj as AlarmHelper;

            if (obj == null)
            {
                throw new Exception("CallbackAction receive wrong param");
            }

            if (alarm.PubStatus.Equals(PublishingStatus.UPDATE))
            {
                UpdateAlarm(alarm);
            }
            else
            {
                AddAlarm(alarm);
            }
        }

        private void AddAlarm(AlarmHelper alarm)
        {
            lock (alarmSummaryLock)
            {
                List<AlarmHelper> alarmsToRemove = new List<AlarmHelper>(1);
                if (!alarm.Type.Equals(AlarmType.NORMAL))
                {
                    foreach (AlarmHelper aHelper in AlarmSummaryQueue)
                    {
                        if (aHelper.Gid.Equals(alarm.Gid) && aHelper.CurrentState.Contains(State.Active.ToString()))
                        {
                            //aHelper.CurrentState = string.Format("{0}, {1}", State.Active, aHelper.AckState);
                            //OnPropertyChanged(nameof(AlarmSummaryQueue));
                            UpdateAlarm(alarm);
                            return;
                        }
                    }
                }
                else //ako je tip NORMAL
                {
                    foreach (AlarmHelper aHelper in AlarmSummaryQueue)
                    {
                        if (aHelper.Gid.Equals(alarm.Gid) && aHelper.TimeStamp.Equals(alarm.TimeStamp))
                            return;
                    }
               
                    
                }

                foreach (AlarmHelper aHelper in AlarmSummaryQueue)
                {
                    if (aHelper.Gid == alarm.Gid)
                    {
                        alarmsToRemove.Add(aHelper);
                    }
                }
                foreach (AlarmHelper ah in alarmsToRemove)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AlarmSummaryQueue.Remove(ah);
                    });
                }

                try
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AlarmSummaryQueue.Add(alarm);
                    });
                }
                catch (Exception e)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceWarning, "AES can not update alarm values on UI becaus UI instance does not exist. Message: {0}", e.Message);
                }
                OnPropertyChanged(nameof(AlarmSummaryQueue));
            }
        }

        private void UpdateAlarm(AlarmHelper alarm)
        {
            lock (alarmSummaryLock)
            {
                foreach (AlarmHelper aHelper in AlarmSummaryQueue)
                {
                    if (aHelper.Gid.Equals(alarm.Gid) && aHelper.CurrentState.Contains(State.Active.ToString()))
                    {
                        if (!aHelper.CurrentState.Contains(AckState.Acknowledged.ToString()))
                        {
                            aHelper.CurrentState = alarm.CurrentState;
                        }
                        aHelper.Severity = alarm.Severity;
                        aHelper.Value = alarm.Value;
                        aHelper.Message = alarm.Message;
                        aHelper.TimeStamp = alarm.TimeStamp;
                    }
                }
                OnPropertyChanged(nameof(AlarmSummaryQueue));
            }
        }
        private void IntegirtyUpdate()
        {
            List<AlarmHelper> integirtyResult = AesIntegrityProxy.Instance.InitiateIntegrityUpdate();

            lock (alarmSummaryLock)
            {
                foreach (AlarmHelper alarm in integirtyResult)
                {
                    AlarmSummaryQueue.Add(alarm);
                    OnPropertyChanged(nameof(AlarmSummaryQueue));
                }
            }
        }
    }
}