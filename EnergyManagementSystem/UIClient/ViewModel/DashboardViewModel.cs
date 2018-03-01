using EMS.Common;
using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using UIClient.PubSub;

namespace UIClient.ViewModel
{
	public class DashboardViewModel : ViewModelBase
    {
        #region Fields

        private CeSubscribeProxy ceSubscribeProxy;

        private int MAX_DISPLAY_NUMBER = 10;
        private int MAX_DISPLAY_TOTAL_NUMBER = 20;
		private const int NUMBER_OF_ALLOWED_ATTEMPTS = 5; // number of allowed attepts to subscribe to the CE
        private int attemptsCount = 0;

        private readonly double graphSizeOffset = 18;

        private float currentConsumption;
        private float currentProduction;
        private bool isOptionsExpanded = false;

        private ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> generatorsContainer = new ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>>();
        private ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> energyConsumersContainer = new ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>>();

		private ObservableCollection<KeyValuePair<DateTime, float>> generationList= new ObservableCollection<KeyValuePair<DateTime, float>>();
		private ObservableCollection<KeyValuePair<DateTime, float>> demandList= new ObservableCollection<KeyValuePair<DateTime, float>>();

		private Dictionary<long, bool> gidToBoolMap = new Dictionary<long, bool>();
        private ICommand expandCommand;
        private ICommand visibilityCheckedCommand;
        private ICommand visibilityUncheckedCommand;
        private ICommand changeAlgorithmCommand;
        private ICommand goToCommand;
        private OptimizationType selectedOptimizationType = OptimizationType.Linear;

        private double sizeValue;
        private double graphWidth;
        private double graphHeight;

        #endregion Fields

        #region Properties

        public ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> GeneratorsContainer
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

        public ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> EnergyConsumersContainer
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

		public ObservableCollection<KeyValuePair<DateTime, float>> DemandList
		{
			get
			{
				return demandList;
			}

			set
			{
				demandList = value;
			}
		}

		public ObservableCollection<KeyValuePair<DateTime, float>> GenerationList
		{
			get
			{
				return generationList;
			}

			set
			{
				generationList = value;
			}
		}


		public float CurrentConsumption
        {
            get
            {
                return currentConsumption;
            }

            set
            {
                currentConsumption = value;
                OnPropertyChanged();
            }
        }

        public float CurrentProduction
        {
            get
            {
                return currentProduction;
            }

            set
            {
                currentProduction = value;
                OnPropertyChanged();
            }
        }

        public bool IsOptionsExpanded
        {
            get
            {
                return isOptionsExpanded;
            }

            set
            {
                isOptionsExpanded = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<long, bool> GidToBoolMap
        {
            get
            {
                return gidToBoolMap;
            }

            set
            {
                gidToBoolMap = value;
            }
        }

        public OptimizationType SelectedOptimizationType
        {
            get
            {
                return selectedOptimizationType;
            }

            set
            {
                selectedOptimizationType = value;
                OnPropertyChanged();
            }
        }

        public double SizeValue
        {
            get
            {
                return sizeValue;
            }

            set
            {
                sizeValue = value;
                OnPropertyChanged();
                UpdateSizeWidget(value);
            }
        }

        public double GraphWidth
        {
            get
            {
                return graphWidth;
            }

            set
            {
                graphWidth = value;
                OnPropertyChanged();
            }
        }

        public double GraphHeight
        {
            get
            {
                return graphHeight;
            }

            set
            {
                graphHeight = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        public DashboardViewModel()
        {
            SubsrcibeToCE();
            //ceSubscribeProxy.ChooseOptimization(selectedOptimizationType);
            CeOptimizationProxy.Instance.ChooseOptimization(selectedOptimizationType);

            SizeValue = 0;

            GraphWidth = 16 * graphSizeOffset;
            GraphHeight = 9 * graphSizeOffset;
            Title = "Dashboard";
        }

        #region Commands

        public ICommand ExpandCommand => expandCommand ?? (expandCommand = new RelayCommand(ExpandCommandExecute));

        public ICommand VisibilityCheckedCommand => visibilityCheckedCommand ?? (visibilityCheckedCommand = new RelayCommand<long>(VisibilityCheckedCommandExecute));

        public ICommand VisibilityUncheckedCommand => visibilityUncheckedCommand ?? (visibilityUncheckedCommand = new RelayCommand<long>(VisibilityUncheckedCommandExecute));

        public ICommand ChangeAlgorithmCommand => changeAlgorithmCommand ?? (changeAlgorithmCommand = new RelayCommand(ChangeAlgorithmCommandExecute));

        public ICommand GoToCommand => goToCommand ?? (goToCommand = new RelayCommand<long>(GoToCommandCommandExecute));

		

		#endregion Commands

		#region CommandsExecutions

		private void ExpandCommandExecute(object obj)
        {
            if (IsOptionsExpanded)
            {
                IsOptionsExpanded = false;
            }
            else
            {
                IsOptionsExpanded = true;
            }
        }

        private void VisibilityCheckedCommandExecute(long gid)
        {
            GidToBoolMap[gid] = true;
            OnPropertyChanged(nameof(GidToBoolMap));
        }

        private void VisibilityUncheckedCommandExecute(long gid)
        {
            GidToBoolMap[gid] = false;
            OnPropertyChanged(nameof(GidToBoolMap));
        }

        private void ChangeAlgorithmCommandExecute(object obj)
        {
            //ceSubscribeProxy.ChooseOptimization(SelectedOptimizationType);
            CeOptimizationProxy.Instance.ChooseOptimization(SelectedOptimizationType);
        }

        private void UpdateSizeWidget(double sliderValue)
        {
            GraphWidth = (sliderValue + 1) * 16 * graphSizeOffset;
            GraphHeight = (sliderValue + 1) * 9 * graphSizeOffset;
            MAX_DISPLAY_NUMBER = 10 * ((int)sliderValue + 1);

            foreach (var keyPair in GeneratorsContainer)
            {
                while (keyPair.Value.Count > MAX_DISPLAY_NUMBER)
                {
                    keyPair.Value.RemoveAt(0);
                }
            }

            foreach (var keyPair in EnergyConsumersContainer)
            {
                while (keyPair.Value.Count > MAX_DISPLAY_NUMBER)
                {
                    keyPair.Value.RemoveAt(0);
                }
            }
        }

        private void GoToCommandCommandExecute(long gid)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            var mainWindVM = mainWindow.DataContext as MainWindowViewModel;

            var historyVm = mainWindVM?.DockManagerViewModel.Documents.FirstOrDefault(x => x is HistoryViewModel) as HistoryViewModel;

            if (historyVm != null && historyVm.GidToBoolMap.ContainsKey(gid))
            {
                mainWindow.SetActiveDocument(historyVm);
                historyVm.GidToBoolMap[gid] = true;
                historyVm.SelectedPeriod = Model.PeriodValues.Last_Hour;
                historyVm.ShowDataCommand.Execute(null);
                historyVm.OnPropertyChanged("GidToBoolMap");
            }
        }

        #endregion CommandsExecutions

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
                if (attemptsCount++ < NUMBER_OF_ALLOWED_ATTEMPTS)
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
            List<MeasurementUI> measUIs = obj as List<MeasurementUI>;

            if (obj == null)
            {
                throw new Exception("CallbackAction receive wrong param");
            }
            if (measUIs.Count == 0)
            {
                return;
            }

            try
            {
                if ((EMSType)ModelCodeHelper.ExtractTypeFromGlobalId(measUIs[0].Gid) == EMSType.ENERGYCONSUMER)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AddMeasurmentTo(EnergyConsumersContainer, measUIs);
                        CurrentConsumption = measUIs.Sum(x => x.CurrentValue);
						DemandList.Add(new KeyValuePair<DateTime, float>(measUIs.Last().TimeStamp,CurrentConsumption));
						if(DemandList.Count > MAX_DISPLAY_TOTAL_NUMBER)
						{
							DemandList.RemoveAt(0);
						}
                    });
                }
                else
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AddMeasurmentTo(GeneratorsContainer, measUIs);
						CurrentProduction = measUIs.Sum(x => x.CurrentValue);
						GenerationList.Add(new KeyValuePair<DateTime, float>(measUIs.Last().TimeStamp, CurrentProduction));
						if (GenerationList.Count > MAX_DISPLAY_TOTAL_NUMBER)
						{
							GenerationList.RemoveAt(0);
						}
					});
                }
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "CES can not update measurement values on UI becaus UI instance does not exist. Message: {0}", e.Message);
            }
        }

        private void AddMeasurmentTo(ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> container, List<MeasurementUI> measUIs)
        {
            foreach (var measUI in measUIs)
            {
                var keyPair = container.FirstOrDefault(x => x.Key == measUI.Gid);

                if (keyPair.Value == null)
                {
                    var tempQueue = new ObservableCollection<MeasurementUI>();
                    tempQueue.Add(measUI);
                    container.Add(new KeyValuePair<long, ObservableCollection<MeasurementUI>>(measUI.Gid, tempQueue));
                    if (!GidToBoolMap.ContainsKey(measUI.Gid))
                    {
                        GidToBoolMap.Add(measUI.Gid, true);
                    }
                }
                else
                {
                    keyPair.Value.Add(measUI);
                    if (keyPair.Value.Count > MAX_DISPLAY_NUMBER)
                    {
                        keyPair.Value.RemoveAt(0);
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            ceSubscribeProxy.Unsubscribe();
            base.OnDispose();
        }
    }
}