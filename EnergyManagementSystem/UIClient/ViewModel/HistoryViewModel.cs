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
        #region Fields

        private ICommand showDataCommand;
        private string generatorGid;
        private DateTime startTime;
        private DateTime endTime;
        private ICommand visibilityCheckedCommand;
        private ICommand visibilityUncheckedCommand;
        private ICommand allGeneratorsCheckedCommand;
        private ICommand allGeneratorsUnheckedCommand;
        private ICommand selectedPeriodCommand;
        private PeriodValues selectedPeriod;
        private GraphSample graphSampling;
        private List<long> generatorsFromNms = new List<long>();
        private List<Tuple<double, DateTime>> measurements;
        private Dictionary<long, bool> gidToBoolMap = new Dictionary<long, bool>();
        private ObservableCollection<Tuple<double, DateTime>> totalProduction = new ObservableCollection<Tuple<double, DateTime>>();
        private ObservableCollection<Tuple<double, DateTime>> graphTotalProduction = new ObservableCollection<Tuple<double, DateTime>>();
        private ObservableCollection<KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>> generatorsContainer = new ObservableCollection<KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>>();
        private ObservableCollection<Tuple<double, DateTime>> graphTotalProductionForSelected = new ObservableCollection<Tuple<double, DateTime>>();
        private ModelResourcesDesc modelResourcesDesc;
        private List<ModelCode> properties;
        private int iteratorId;
        private int resourcesLeft;
        private int numberOfResources = 2;
        private List<ResourceDescription> retList;
        private static List<ResourceDescription> internalSynchMachines;
        private bool isExpandedSeparated = false;
        #endregion

        public HistoryViewModel()
        {
            Title = "History";
            startTime = DateTime.Now.AddMinutes(-1);
            endTime = DateTime.Now;
            graphSampling = GraphSample.None;
            selectedPeriod = PeriodValues.None;

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
                        if (GeneratorsFromNms.Contains(gid))
                        {
                            continue;
                        }
                        GeneratorsFromNms.Add(gid);
                        GidToBoolMap.Add(gid, false);
                    }
                }
                OnPropertyChanged(nameof(GeneratorsFromNms));

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

        #endregion

        #region Properties

        public List<long> GeneratorsFromNms
        {
            get
            {
                return generatorsFromNms;
            }
            set
            {
                generatorsFromNms = value;
            }
        }

        public ObservableCollection<Tuple<double, DateTime>> TotalProduction
        {
            get
            {
                return totalProduction;
            }
            set
            {
                totalProduction = value;
            }
        }

        public ObservableCollection<Tuple<double, DateTime>> GraphTotalProduction
        {
            get
            {
                return graphTotalProduction;
            }
            set
            {
                graphTotalProduction = value;
            }
        }

        public ObservableCollection<Tuple<double, DateTime>> GraphTotalProductionForSelected
        {
            get
            {
                return graphTotalProductionForSelected;
            }
            set
            {
                graphTotalProductionForSelected = value;
            }
        }

        public HistoryView HistoryView { get; set; }

        public PeriodValues SelectedPeriod
        {
            get
            {
                return selectedPeriod;
            }
            set
            {
                selectedPeriod = value;
                UpdatePeriod();
                OnPropertyChanged(nameof(SelectedPeriod));
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
                graphSampling = GraphSample.None;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                graphSampling = GraphSample.None;
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

        public bool IsExpandedSeparated
        {
            get
            {
                return isExpandedSeparated;
            }

            set
            {
                isExpandedSeparated = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command Executions

        private void ShowDataCommandExecute(object obj)
        {
            ObservableCollection<Tuple<double, DateTime>> measurementsFromDb;
            ObservableCollection<Tuple<double, DateTime>> tempData;
            ObservableCollection<Tuple<double, DateTime>> tempContainer = new ObservableCollection<Tuple<double, DateTime>>();
            ObservableCollection<Tuple<double, DateTime>> tempContainterForProductionSelected = new ObservableCollection<Tuple<double, DateTime>>();

            GraphTotalProduction.Clear();
            GeneratorsContainer.Clear();
            GraphTotalProductionForSelected.Clear();

            foreach (KeyValuePair<long, bool> keyPair in GidToBoolMap)
            {
                if (keyPair.Value == true)
                {
                    try
                    {
                        measurementsFromDb = new ObservableCollection<Tuple<double, DateTime>>(CalculationEngineUIProxy.Instance.GetHistoryMeasurements(keyPair.Key, startTime, endTime));

                        if (measurementsFromDb == null)
                        {
                            continue;
                        }

                        if (graphSampling != GraphSample.None)
                        {
                            DateTime tempStartTime = startTime;
                            DateTime tempEndTime = IncrementTime(tempStartTime);

                            double averageProduction = 0;

                            while (tempEndTime <= endTime)
                            {
                                tempData = new ObservableCollection<Tuple<double, DateTime>>(measurementsFromDb.Where(x => x.Item2 > tempStartTime && x.Item2 < tempEndTime));
                                if (tempData != null && tempData.Count != 0)
                                {
                                    averageProduction = tempData.Average(x => x.Item1);
                                }
                                else
                                {
                                    averageProduction = 0;
                                }

                                tempStartTime = IncrementTime(tempStartTime);
                                tempEndTime = IncrementTime(tempEndTime);

                                tempContainer.Add(new Tuple<double, DateTime>(averageProduction, tempStartTime));
                            }
                            GeneratorsContainer.Add(new KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>(keyPair.Key, new ObservableCollection<Tuple<double, DateTime>>(tempContainer)));
                        }
                        else
                        {
                            GeneratorsContainer.Add(new KeyValuePair<long, ObservableCollection<Tuple<double, DateTime>>>(keyPair.Key, new ObservableCollection<Tuple<double, DateTime>>(measurementsFromDb)));
                        }

                        measurementsFromDb.Clear();
                        measurementsFromDb = null;
                        tempContainer.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceError, "[HistoryViewModel] Error ShowDataCommandExecute {0}", ex.Message);
                    }
                }
            }

            List<Tuple<double, DateTime>> allProductionValues = new List<Tuple<double, DateTime>>();
            List<DateTime> timestamps = new List<DateTime>();

            foreach (var keyPair in GeneratorsContainer)
            {
                allProductionValues.AddRange(keyPair.Value.ToList());
            }

            foreach(Tuple<double,DateTime> tuple in allProductionValues)
            {
                timestamps.Add(tuple.Item2);
            }
            timestamps = timestamps.Distinct().ToList();

            foreach (DateTime measTime in timestamps)
            {
                double production = 0;
                List<Tuple<double, DateTime>> tuples = allProductionValues.Where(x => x.Item2 == measTime).ToList();
                if (tuples != null)
                {
                    production = tuples.Sum(x => x.Item1);
                }
                tuples = null;
                GraphTotalProductionForSelected.Add(new Tuple<double, DateTime>(production, measTime));
            }

            TotalProduction = new ObservableCollection<Tuple<double, DateTime>>(CalculationEngineUIProxy.Instance.GetTotalProduction(StartTime, EndTime));
            GraphTotalProduction = new ObservableCollection<Tuple<double, DateTime>>();

            if (graphSampling != GraphSample.None)
            {
                DateTime tempStartTime = startTime;
                DateTime tempEndTime = IncrementTime(tempStartTime);

                double averageProduction;

                while (tempEndTime <= endTime)
                {
                    tempData = new ObservableCollection<Tuple<double, DateTime>>(TotalProduction.Where(x => x.Item2 > tempStartTime && x.Item2 < tempEndTime));
                    if (tempData != null && tempData.Count != 0)
                    {
                        averageProduction = tempData.Average(x => x.Item1);
                    }
                    else
                    {
                        averageProduction = 0;
                    }

                    tempStartTime = IncrementTime(tempStartTime);
                    tempEndTime = IncrementTime(tempEndTime);
                    GraphTotalProduction.Add(new Tuple<double, DateTime>(averageProduction, tempStartTime));
                }
            }
            else
            {
                GraphTotalProduction = TotalProduction;
            }
            IsExpandedSeparated = true;

            OnPropertyChanged(nameof(GraphTotalProductionForSelected));
            OnPropertyChanged(nameof(GraphTotalProduction));
            OnPropertyChanged(nameof(GeneratorsContainer));
        }

        private DateTime IncrementTime(DateTime pointTime)
        {
            switch (graphSampling)
            {
                case GraphSample.HourSample:
                    pointTime = pointTime.AddMinutes(5);
                    return pointTime;
                case GraphSample.TodaySample:
                    pointTime = pointTime.AddHours(1);
                    return pointTime;
                case GraphSample.YearSample:
                    pointTime = pointTime.AddMonths(1);
                    return pointTime;
                case GraphSample.LastMonthSample:
                    pointTime = pointTime.AddDays(1);
                    return pointTime;
                case GraphSample.Last4MonthSample:
                    pointTime = pointTime.AddDays(7);
                    return pointTime;
                default:
                    return pointTime;
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
            UpdatePeriod();
        }

        private void UpdatePeriod()
        {
            switch (SelectedPeriod)
            {
                case PeriodValues.Last_Hour:
                    StartTime = DateTime.Now.AddHours(-1);
                    EndTime = DateTime.Now;
                    graphSampling = GraphSample.HourSample;
                    break;
                case PeriodValues.Today:
                    StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    EndTime = DateTime.Now;
                    graphSampling = GraphSample.TodaySample;
                    break;
                case PeriodValues.Last_Month:
                    StartTime = DateTime.Now.AddMonths(-1);
                    EndTime = DateTime.Now;
                    graphSampling = GraphSample.LastMonthSample;
                    break;
                case PeriodValues.Last_4_Month:
                    StartTime = DateTime.Now.AddMonths(-4);
                    EndTime = DateTime.Now;
                    graphSampling = GraphSample.Last4MonthSample;
                    break;
                case PeriodValues.Last_Year:
                    StartTime = DateTime.Now.AddYears(-1);
                    EndTime = DateTime.Now;
                    graphSampling = GraphSample.YearSample;
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}
