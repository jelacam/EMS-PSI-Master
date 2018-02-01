using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UIClient.Model;
using UIClient.View;

namespace UIClient.ViewModel
{
	public class HistoryViewModel : ViewModelBase
	{
		private ICommand showDataCommand;
		private string generatorGid;
		private DateTime startTime;
		private DateTime endTime;
		private List<Tuple<double, DateTime>> measurements;
		private ObservableCollection<KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>> generatorsContainer = new ObservableCollection<KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>>();
		private ICommand visibilityCheckedCommand;
		private ICommand visibilityUncheckedCommand;
		private ICommand allGeneratorsCheckedCommand;
		private ICommand allGeneratorsUnheckedCommand;
		private ICommand selectedPeriodCommand;
		private ICommand expandSeparateGensCommand;
		private Dictionary<long, bool> gidToBoolMap = new Dictionary<long, bool>();
		private PeriodValues selectedPeriod;
		private bool isSeparateGensExpanded = false;

		private ModelResourcesDesc modelResourcesDesc;
		private List<ModelCode> properties;
		private int iteratorId;
		private int resourcesLeft;
		private int numberOfResources = 2;
		private List<ResourceDescription> retList;
		private static List<ResourceDescription> internalSynchMachines;


		public HistoryViewModel(HistoryView mainWindow)
		{
			startTime = DateTime.Now;
			endTime = DateTime.Now;

			internalSynchMachines = new List<ResourceDescription>(5);
			modelResourcesDesc = new ModelResourcesDesc();
			retList = new List<ResourceDescription>(5);
			properties = new List<ModelCode>(10);
			ModelCode modelCodeSynchronousMachine = ModelCode.SYNCHRONOUSMACHINE;
			iteratorId = 0;
			resourcesLeft = 0;
			numberOfResources = 2;
			string message = string.Empty;


			// getting SynchronousMachine
			try
			{
				// first get all synchronous machines from NMS
				properties = modelResourcesDesc.GetAllPropertyIds(modelCodeSynchronousMachine);
				iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeSynchronousMachine, properties);
				resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

				while (resourcesLeft > 0)
				{
					List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
					retList.AddRange(rds);
					resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
				}
				NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);

				// add synchronous machines to internal collection
				internalSynchMachines.AddRange(retList);

				foreach (ResourceDescription rd in internalSynchMachines)
				{
					if (rd.ContainsProperty(ModelCode.IDENTIFIEDOBJECT_GID))
					{
						long gid = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_GID).AsLong();
						var keyValuePair = GeneratorsContainer.FirstOrDefault(x => x.Key == gid);
						if (keyValuePair.Value == null)
						{
							GeneratorsContainer.Add(new KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>(gid, new ObservableCollection<Tuple<double, DateTime>>()));
							GidToBoolMap.Add(gid, false);
						}
					}
				}
				OnPropertyChanged(nameof(GeneratorsContainer));

			}
			catch (Exception e)
			{
				message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCodeSynchronousMachine, e.Message);
				Console.WriteLine(message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				//return false;
			}

			// clear retList for getting new model from NMS
			retList.Clear();
		}

		#region Commands

		public ICommand ShowDataCommand => showDataCommand ?? (showDataCommand = new RelayCommand(ShowDataCommandExecute));

		public ICommand VisibilityCheckedCommand => visibilityCheckedCommand ?? (visibilityCheckedCommand = new RelayCommand<long>(VisibilityCheckedCommandExecute));

		public ICommand VisibilityUncheckedCommand => visibilityUncheckedCommand ?? (visibilityUncheckedCommand = new RelayCommand<long>(VisibilityUncheckedCommandExecute));

		public ICommand AllGeneratorsCheckedCommand => allGeneratorsCheckedCommand ?? (allGeneratorsCheckedCommand = new RelayCommand(AllGeneratorsCheckedCommandExecute));

		public ICommand AllGeneratorsUncheckedCommand => allGeneratorsUnheckedCommand ?? (allGeneratorsUnheckedCommand = new RelayCommand(AllGeneratorsUnheckedCommandExecute));

		public ICommand ChangePeriodCommand => selectedPeriodCommand ?? (selectedPeriodCommand = new RelayCommand(SelectedPeriodCommandExecute));

		public ICommand ExpandSeparateGensCommand => expandSeparateGensCommand ?? (expandSeparateGensCommand = new RelayCommand(ExpandSeparateGensCommandExecute));

		#endregion

		#region Properties

		public bool IsSeparateGensExpanded
		{
			get
			{
				return isSeparateGensExpanded;
			}
			set
			{
				isSeparateGensExpanded = value;
				OnPropertyChanged(nameof(IsSeparateGensExpanded));
			}
		}

		public PeriodValues SelectedPeriod
		{
			get
			{
				return selectedPeriod;
			}
			set
			{
				selectedPeriod = value;
			}
		}

		public ObservableCollection<KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>> GeneratorsContainer
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

		public string GeneratorGid
		{
			get { return generatorGid; }
			set { this.generatorGid = value; }
		}

		public List<Tuple<double, DateTime>> Measurements
		{
			get { return measurements; }
			set { measurements = value; }
		}

		public DateTime StartTime
		{
			get { return startTime; }
			set
			{
				startTime = value;
				OnPropertyChanged(nameof(StartTime));
			}
		}

		public DateTime EndTime
		{
			get { return endTime; }
			set
			{
				endTime = value;
				OnPropertyChanged(nameof(EndTime));
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

		#endregion

		#region Command Executions

		private void ShowDataCommandExecute(object obj)
		{
			if (GidToBoolMap.Count != 0)
			{
				ObservableCollection<Tuple<double, DateTime>> tempList;
				foreach (KeyValuePair<long, bool> keyPair in GidToBoolMap)
				{
					if (keyPair.Value == true)
					{
						try
						{
							tempList = new ObservableCollection<Tuple<double, DateTime>>(CalculationEngineUIProxy.Instance.GetHistoryMeasurements(keyPair.Key, startTime, endTime));
							if (tempList == null)
							{
								continue;
							}

							var keyPairGenerator = GeneratorsContainer.FirstOrDefault(x => x.Key == keyPair.Key);
							if (keyPairGenerator.Value == null)
							{
								GeneratorsContainer.Add(new KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>(keyPair.Key, tempList));
							}
							else
							{
								keyPairGenerator.Value.Clear();
								foreach (Tuple<double, DateTime> tuple in tempList)
								{
									keyPairGenerator.Value.Add(new Tuple<double, DateTime>(tuple.Item1, tuple.Item2));
								}

							}
							tempList.Clear();
							tempList = null;
						}
						catch (Exception ex)
						{
							CommonTrace.WriteTrace(CommonTrace.TraceError, "[HistoryViewModel] Error ShowDataCommandExecute {0}", ex.Message);
						}

					}
				}
				OnPropertyChanged(nameof(GeneratorsContainer));
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

		private void AllGeneratorsCheckedCommandExecute(object obj)
		{
			GidToBoolMap = GidToBoolMap.ToDictionary(p => p.Key, p => true);
			OnPropertyChanged(nameof(GidToBoolMap));
		}

		private void AllGeneratorsUnheckedCommandExecute(object obj)
		{
			GidToBoolMap = GidToBoolMap.ToDictionary(p => p.Key, p => false);
			OnPropertyChanged(nameof(GidToBoolMap));
		}

		private void SelectedPeriodCommandExecute(object obj)
		{
			if (SelectedPeriod == PeriodValues.Last_Hour)
			{
				StartTime = DateTime.Now.AddHours(-1);
				EndTime = DateTime.Now;
			}
			else if (SelectedPeriod == PeriodValues.Today)
			{
				StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
				EndTime = DateTime.Now;
			}
			else if (SelectedPeriod == PeriodValues.Last_Year)
			{
				StartTime = DateTime.Now.AddYears(-1);
				EndTime = DateTime.Now;
			}
			else if (SelectedPeriod == PeriodValues.Last_Month)
			{
				StartTime = DateTime.Now.AddMonths(-1);
				EndTime = DateTime.Now;
			}
			else if (SelectedPeriod == PeriodValues.Last_4_Month)
			{
				StartTime = DateTime.Now.AddMonths(-4);
				EndTime = DateTime.Now;
			}
		}

		private void ExpandSeparateGensCommandExecute(object obj)
		{
			if (IsSeparateGensExpanded)
			{
				IsSeparateGensExpanded = false;
			}
			else
			{
				IsSeparateGensExpanded = true;
			}
		}

		#endregion

	}
}
