﻿using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UIClient.View;

namespace UIClient.ViewModel
{
    public class HistoryViewModel : ViewModelBase
    {
        private ICommand showDataCommand;
        private string generatorGid;
        private DateTime startTime;
        private DateTime endTime;
        private List<Property> generatorsGids;
        private List<Tuple<double, DateTime>> measurements;
		private ObservableCollection<KeyValuePair<long, List<Tuple<double,DateTime>>>> generatorsContainer = new ObservableCollection<KeyValuePair<long, List<Tuple<double,DateTime>>>>();
		private ICommand visibilityCheckedCommand;
		private ICommand visibilityUncheckedCommand;
		private Dictionary<long, bool> gidToBoolMap = new Dictionary<long, bool>();

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
            generatorsGids = new List<Property>();

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
                        generatorsGids.Add(rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_GID));
                    }
                }

				foreach(ResourceDescription rd in internalSynchMachines)
				{
					if (rd.ContainsProperty(ModelCode.IDENTIFIEDOBJECT_GID))
					{
						long gid = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_GID).AsLong();
						var keyValuePair = GeneratorsContainer.FirstOrDefault(x => x.Key == gid);
						if (keyValuePair.Value == null)
						{
							GeneratorsContainer.Add(new KeyValuePair<long, List<Tuple<double, DateTime>>>(gid, new List<Tuple<double, DateTime>>()));
							GidToBoolMap.Add(gid, false);
						}
					}
				}
				GeneratorsContainer.ElementAt(0).Value.Add(new Tuple<double, DateTime>(2.5, DateTime.Now.AddSeconds(-3)));
				GeneratorsContainer.ElementAt(0).Value.Add(new Tuple<double, DateTime>(4.5, DateTime.Now.AddSeconds(-1)));

				OnPropertyChanged(nameof(GeneratorsGids));
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

		#endregion

		#region Properties

		public ObservableCollection<KeyValuePair<long, List<Tuple<double, DateTime>>>> GeneratorsContainer
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
            set { startTime = value; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public List<Property> GeneratorsGids
        {
            get { return generatorsGids; }
            set { generatorsGids = value; }
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
				List<Tuple<double, DateTime>> tempList;
				foreach(KeyValuePair<long,bool> keyPair in GidToBoolMap)
				{
					if (keyPair.Value == true)
					{
						try
						{
							tempList = CalculationEngineUIProxy.Instance.GetHistoryMeasurements(keyPair.Key, startTime, endTime);
							if (tempList == null)
							{
								continue;
							}

							var keyPairGenerator = GeneratorsContainer.FirstOrDefault(x => x.Key == keyPair.Key);
							if (keyPairGenerator.Value == null)
							{
								GeneratorsContainer.Add(new KeyValuePair<long, List<Tuple<double, DateTime>>>(keyPair.Key, tempList));
							}
							else
							{
								keyPairGenerator.Value.Clear();
								foreach(Tuple<double,DateTime> tuple in tempList)
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

		#endregion

	}
}
