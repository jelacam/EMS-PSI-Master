using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIClient.PubSub;

namespace UIClient.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private CeSubscribeProxy ceSubscribeProxy;

        private const int MAX_DISPLAY_NUMBER = 10;
        private const int NUMBER_OF_ALLOWED_ATTEMPTS = 5; // number of allowed attepts to subscribe to the CE
        private int attemptsCount = 0;

        private Dictionary<long, ObservableCollection<MeasurementUI>> generatorsContainer = new Dictionary<long, ObservableCollection<MeasurementUI>>();
        private Dictionary<long, ObservableCollection<MeasurementUI>> energyConsumersContainer = new Dictionary<long, ObservableCollection<MeasurementUI>>();
        private Dictionary<long, long> generatorsCount = new Dictionary<long, long>(); // privremeno dok se ne napravi pravi timestamp
        public Dictionary<long, ObservableCollection<MeasurementUI>> GeneratorsContainer
        {
            get
            {
                return generatorsContainer;
            }

            set
            {
                generatorsContainer = value;

            }
        }

        public Dictionary<long, ObservableCollection<MeasurementUI>> EnergyConsumersContainer
        {
            get
            {
                return energyConsumersContainer;
            }

            set
            {
                energyConsumersContainer = value;
            }
        }

        public DashboardViewModel()
        {
            SubsrcibeToCE();
        }

        private void SubsrcibeToCE()
        {
            try
            {
                ceSubscribeProxy = new CeSubscribeProxy(CallbackAction);
                ceSubscribeProxy.Subscribe();
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Could not connect to Publisher Service! \n ");
                Thread.Sleep(1000);
                if(attemptsCount++ < NUMBER_OF_ALLOWED_ATTEMPTS)
                {
                    SubsrcibeToCE();
                }
                else
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "Could not connect to Publisher Service!  \n {0}", e.Message);
                }
            }
        }

        private void CallbackAction(object obj)
        {
            MeasurementUI measUI = obj as MeasurementUI;

            if (obj == null)
            {
                throw new Exception("CallbackAction receive wrong param");
            }

            if((EMSType)ModelCodeHelper.ExtractTypeFromGlobalId(measUI.Gid) == EMSType.ENERGYCONSUMER)
            {
                AddMeasurmentToEnergyConsumers(measUI);

            }else
            {
                AddMeasurmentToGenerators(measUI);
            }
        }

        private void AddMeasurmentToGenerators(MeasurementUI measUI)
        {
            ObservableCollection<MeasurementUI> tempQueue;

            if (GeneratorsContainer.TryGetValue(measUI.Gid, out tempQueue))
            {
               // measUI.TimeStamp = ++generatorsCount[measUI.Gid]; //temp 
                tempQueue.Add(measUI);
                if (tempQueue.Count > MAX_DISPLAY_NUMBER)
                {
                    tempQueue.RemoveAt(0);
                }
            }
            else
            {
                tempQueue = new ObservableCollection<MeasurementUI>();
                //measUI.TimeStamp = 0;
                tempQueue.Add(measUI);
                GeneratorsContainer.Add(measUI.Gid, tempQueue);
                generatorsCount.Add(measUI.Gid, 0);//temp
            }
            OnPropertyChanged(nameof(GeneratorsContainer));
        }

        /// <summary>
        /// Add measurent in propertly queue
        /// </summary>
        /// <param name="measUI"></param>
        private void AddMeasurmentToEnergyConsumers(MeasurementUI measUI)
        {
            ObservableCollection<MeasurementUI> tempQueue;

            if (EnergyConsumersContainer.TryGetValue(measUI.Gid, out tempQueue))
            {
                tempQueue.Add(measUI);
                if (tempQueue.Count > MAX_DISPLAY_NUMBER)
                {
                    tempQueue.RemoveAt(0);
                }
            }
            else
            {
                tempQueue = new ObservableCollection<MeasurementUI>();
                tempQueue.Add(measUI);
                EnergyConsumersContainer.Add(measUI.Gid, tempQueue);
                generatorsCount.Add(measUI.Gid, 0);//temp
            }
            OnPropertyChanged(nameof(EnergyConsumersContainer));
        }

        protected override void OnDispose()
        {
            ceSubscribeProxy.Unsubscribe();
            base.OnDispose();
        }
    }
}
