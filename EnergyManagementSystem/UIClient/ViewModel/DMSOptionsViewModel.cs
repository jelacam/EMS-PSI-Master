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
    public class DMSOptionsViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Tuple<double, double, DateTime>> cO2EmissionContainer = new ObservableCollection<Tuple<double, double, DateTime>>();
        private ObservableCollection<Tuple<double, double, DateTime>> graphCO2EmissionContainer = new ObservableCollection<Tuple<double, double, DateTime>>();
        private ObservableCollection<Tuple<double, double, double, double, double, DateTime>> individualContainer = new ObservableCollection<Tuple<double, double, double, double, double, DateTime>>();
        private ObservableCollection<Tuple<double, double, double, DateTime>> savingContainer = new ObservableCollection<Tuple<double, double, double, DateTime>>();
        private ObservableCollection<KeyValuePair<string, double>> pieData = new ObservableCollection<KeyValuePair<string, double>>();
        private ObservableCollection<KeyValuePair<string, double>> pieDataWind = new ObservableCollection<KeyValuePair<string, double>>();
        private ObservableCollection<KeyValuePair<string, double>> barSavingData = new ObservableCollection<KeyValuePair<string, double>>();
        private DateTime startTimeCO2;
        private DateTime endTimeCO2;
        private DateTime startTimeWind;
        private DateTime endTimeWind;
        private DateTime startTimeSaving;
        private DateTime endTimeSaving;
        private PeriodValues selectedPeriodCO2;
        private PeriodValues selectedPeriodWind;
        private PeriodValues selectedPeriodSaving;
        private ICommand viewCO2EmissionDataCommand;
        private ICommand viewIndividualProductionDataCommand;
        private ICommand viewSavingnDataCommand;
        private ICommand changePeriodCo2Command;
        private ICommand changePeriodWindProductionCommand;
        private ICommand changePeriodSavingCommand;
        private ICommand cO2EmissionGraphCheckedCommand;
        private ICommand cO2EmissionGraphUnCheckedCommand;
        private ICommand cO2EmissionWithoutRenewablesGraphCheckedCommand;
        private ICommand cO2EmissionWithoutRenewablesGraphUnCheckedCommand;
        private double totalCO2Reduction;
        private double totalCO2;
        private double totalWindProductionPercentage;
        private double totalWindProduction;
        private double totalSolarProductionPercentage;
        private double totalSolarProduction;
        private double totalHydroProductionPercentage;
        private double totalHydroProduction;
        private double totalCoalProductionPercentage;
        private double totalCoalProduction;
        private double totalOilProductionPercentage;
        private double totalOilProduction;
        private double totalWindSaving;
        private double totalCostWithoutRenewable;
        private double totalCostWithRenewable;
        private double totalSum;
        private GraphSample graphSampling;
        private bool cO2EmissionWithoutRenewableGraphVisibility = false;
        private bool cO2EmissionGraphVisibility = true;

        #endregion Fields

        public DMSOptionsViewModel(DMSOptionsView mainWindow)
        {
            Title = "DMS";
            StartTimeCO2 = DateTime.Now.AddMinutes(-1);
            EndTimeCO2 = DateTime.Now;
            StartTimeWind = DateTime.Now.AddMinutes(-1);
            EndTimeWind = DateTime.Now;
            StartTimeSaving = DateTime.Now.AddMinutes(-1);
            EndTimeSaving = DateTime.Now;
            TotalCO2Reduction = 0;
            TotalCO2 = 0;
            TotalWindSaving = 0;
            SelectedPeriodCO2 = PeriodValues.None;
            SelectedPeriodSaving = PeriodValues.None;
            SelectedPeriodWind = PeriodValues.None;
            graphSampling = GraphSample.None;
        }

        #region Properties

        public bool CO2EmissionWithoutRenewableGraphVisibility
        {
            get
            {
                return cO2EmissionWithoutRenewableGraphVisibility;
            }
            set
            {
                cO2EmissionWithoutRenewableGraphVisibility = value;
                OnPropertyChanged(nameof(CO2EmissionWithoutRenewableGraphVisibility));
            }
        }

        public bool CO2EmissionGraphVisibility
        {
            get
            {
                return cO2EmissionGraphVisibility;
            }
            set
            {
                cO2EmissionGraphVisibility = value;
                OnPropertyChanged(nameof(CO2EmissionGraphVisibility));
            }
        }

        public ObservableCollection<Tuple<double, double, double, DateTime>> SavingContainer
        {
            get
            {
                return savingContainer;
            }
            set
            {
                savingContainer = value;
            }
        }

        public ObservableCollection<KeyValuePair<string, double>> BarSavingData
        {
            get
            {
                return barSavingData;
            }
            set
            {
                barSavingData = value;
            }
        }

        public ObservableCollection<KeyValuePair<string, double>> PieData
        {
            get
            {
                return pieData;
            }
            set
            {
                pieData = value;
            }
        }

        public ObservableCollection<KeyValuePair<string, double>> PieDataWind
        {
            get
            {
                return pieDataWind;
            }
            set
            {
                pieDataWind = value;
            }
        }

        public double TotalWindSaving
        {
            get
            {
                return totalWindSaving;
            }
            set
            {
                totalWindSaving = value;
            }
        }

        public double TotalCostWithoutRenewable
        {
            get
            {
                return totalCostWithoutRenewable;
            }
            set
            {
                totalCostWithoutRenewable = value;
            }
        }

        public double TotalCostWithRenewable
        {
            get
            {
                return totalCostWithRenewable;
            }
            set
            {
                totalCostWithRenewable = value;
            }
        }

        public double TotalCO2
        {
            get
            {
                return totalCO2;
            }
            set
            {
                totalCO2 = value;
            }
        }

        public double TotalCO2Reduction
        {
            get
            {
                return totalCO2Reduction;
            }
            set
            {
                totalCO2Reduction = value;
            }
        }

        public double TotalWindProduction
        {
            get
            {
                return totalWindProduction;
            }
            set
            {
                totalWindProduction = value;
                OnPropertyChanged(nameof(TotalWindProduction));
            }
        }

        public double TotalWindProductionPercentage
        {
            get
            {
                return totalWindProductionPercentage;
            }
            set
            {
                totalWindProductionPercentage = value;
            }
        }

        public double TotalSolarProduction
        {
            get
            {
                return totalSolarProduction;
            }
            set
            {
                totalSolarProduction = value;
                OnPropertyChanged(nameof(TotalSolarProduction));
            }
        }

        public double TotalSolarProductionPercentage
        {
            get
            {
                return totalSolarProductionPercentage;
            }
            set
            {
                totalSolarProductionPercentage = value;
            }
        }

        public double TotalHydroProduction
        {
            get
            {
                return totalHydroProduction;
            }
            set
            {
                totalHydroProduction = value;
                OnPropertyChanged(nameof(TotalHydroProduction));
            }
        }

        public double TotalHydroProductionPercentage
        {
            get
            {
                return totalHydroProductionPercentage;
            }
            set
            {
                totalHydroProductionPercentage = value;
            }
        }

        public double TotalCoalProduction
        {
            get
            {
                return totalCoalProduction;
            }
            set
            {
                totalCoalProduction = value;
                OnPropertyChanged(nameof(TotalCoalProduction));
            }
        }

        public double TotalCoalProductionPercentage
        {
            get
            {
                return totalCoalProductionPercentage;
            }
            set
            {
                totalCoalProductionPercentage = value;
            }
        }

        public double TotalOilProduction
        {
            get
            {
                return totalOilProduction;
            }
            set
            {
                totalOilProduction = value;
                OnPropertyChanged(nameof(TotalOilProduction));
            }
        }

        public double TotalOilProductionPercentage
        {
            get
            {
                return totalOilProductionPercentage;
            }
            set
            {
                totalOilProductionPercentage = value;
            }
        }

        public double TotalSum
        {
            get
            {
                return totalSum;
            }
            set
            {
                totalSum = value;
                OnPropertyChanged(nameof(TotalSum));
            }
        }

        public ObservableCollection<Tuple<double, double, DateTime>> CO2EmissionContainer
        {
            get
            {
                return cO2EmissionContainer;
            }
            set
            {
                cO2EmissionContainer = value;
            }
        }

        public ObservableCollection<Tuple<double, double, DateTime>> GraphCO2EmissionContainer
        {
            get
            {
                return graphCO2EmissionContainer;
            }
            set
            {
                graphCO2EmissionContainer = value;
            }
        }

        public ObservableCollection<Tuple<double, double, double, double, double, DateTime>> IndividualContainer
        {
            get
            {
                return individualContainer;
            }
            set
            {
                individualContainer = value;
            }
        }

        public PeriodValues SelectedPeriodCO2
        {
            get
            {
                return selectedPeriodCO2;
            }
            set
            {
                selectedPeriodCO2 = value;
                OnPropertyChanged(nameof(SelectedPeriodCO2));
            }
        }

        public PeriodValues SelectedPeriodWind
        {
            get
            {
                return selectedPeriodWind;
            }
            set
            {
                selectedPeriodWind = value;
                OnPropertyChanged(nameof(SelectedPeriodWind));
            }
        }

        public PeriodValues SelectedPeriodSaving
        {
            get
            {
                return selectedPeriodSaving;
            }
            set
            {
                selectedPeriodSaving = value;
                OnPropertyChanged(nameof(SelectedPeriodSaving));
            }
        }

        public DateTime StartTimeCO2
        {
            get { return startTimeCO2; }
            set
            {
                startTimeCO2 = value;
                graphSampling = GraphSample.None;
                OnPropertyChanged(nameof(StartTimeCO2));
            }
        }

        public DateTime StartTimeSaving
        {
            get
            {
                return startTimeSaving;
            }
            set
            {
                startTimeSaving = value;
                OnPropertyChanged(nameof(StartTimeSaving));
            }
        }

        public DateTime StartTimeWind
        {
            get { return startTimeWind; }
            set
            {
                startTimeWind = value;
                OnPropertyChanged(nameof(StartTimeWind));
            }
        }

        public DateTime EndTimeCO2
        {
            get { return endTimeCO2; }
            set
            {
                endTimeCO2 = value;
                graphSampling = GraphSample.None;
                OnPropertyChanged(nameof(EndTimeCO2));
            }
        }

        public DateTime EndTimeSaving
        {
            get
            {
                return endTimeSaving;
            }
            set
            {
                endTimeSaving = value;
                OnPropertyChanged(nameof(EndTimeSaving));
            }
        }

        public DateTime EndTimeWind
        {
            get { return endTimeWind; }
            set
            {
                endTimeWind = value;
                OnPropertyChanged(nameof(EndTimeWind));
            }
        }

        #endregion Properties

        #region Commands

        public ICommand ViewCO2EmissionDataCommand => viewCO2EmissionDataCommand ?? (viewCO2EmissionDataCommand = new RelayCommand(ViewCO2EmissionDataCommandExecute));

        public ICommand ChangePeriodCO2Command => changePeriodCo2Command ?? (changePeriodCo2Command = new RelayCommand(ChangePeriodCO2CommandExecute));

        public ICommand ViewIndividualProductionDataCommand => viewIndividualProductionDataCommand ?? (viewIndividualProductionDataCommand = new RelayCommand(ViewIndividualProductionDataCommandExecute));

        public ICommand ChangePeriodWindProductionCommand => changePeriodWindProductionCommand ?? (changePeriodWindProductionCommand = new RelayCommand(ChangePeriodWindProductionCommandExecute));

        public ICommand ChangePeriodSavingCommand => changePeriodSavingCommand ?? (changePeriodSavingCommand = new RelayCommand(ChangePeriodSavingCommandExecute));

        public ICommand ViewSavingnDataCommand => viewSavingnDataCommand ?? (viewSavingnDataCommand = new RelayCommand(ViewSavingnDataCommandExecute));

        public ICommand CO2EmissionGraphCheckedCommand => cO2EmissionGraphCheckedCommand ?? (cO2EmissionGraphCheckedCommand = new RelayCommand(CO2EmissionGraphCheckedCommandExecute));

        public ICommand CO2EmissionGraphUnCheckedCommand => cO2EmissionGraphUnCheckedCommand ?? (cO2EmissionGraphUnCheckedCommand = new RelayCommand(CO2EmissionGraphUnCheckedCommandExecute));

        public ICommand CO2EmissionWithoutRenewablesGraphCheckedCommand => cO2EmissionWithoutRenewablesGraphCheckedCommand ?? (cO2EmissionWithoutRenewablesGraphCheckedCommand = new RelayCommand(CO2EmissionWithoutRenewablesGraphCheckedCommandExecute));

        public ICommand CO2EmissionWithoutRenewablesGraphUnCheckedCommand => cO2EmissionWithoutRenewablesGraphUnCheckedCommand ?? (cO2EmissionWithoutRenewablesGraphUnCheckedCommand = new RelayCommand(CO2EmissionWithoutRenewablesGraphUnCheckedCommandExecute));

        #endregion Commands

        #region Command Executions

        private void ViewSavingnDataCommandExecute(object obj)
        {
            double timeFrame;
            TotalWindSaving = 0;
            TotalCostWithoutRenewable = 0;
            TotalCostWithRenewable = 0;
            try
            {
                SavingContainer = new ObservableCollection<Tuple<double, double, double, DateTime>>(CalculationEngineUIProxy.Instance.ReadWindFarmSavingDataFromDb(StartTimeSaving, EndTimeSaving));
                if (SavingContainer != null)
                {
                    //foreach (Tuple<double, double, double, DateTime> tuple in SavingContainer)
                    for (int i = 1; i < SavingContainer.Count; i++)
                    {
                        timeFrame = (SavingContainer[i].Item4 - SavingContainer[i - 1].Item4).TotalHours;
                        TotalCostWithoutRenewable += ((SavingContainer[i].Item1 + SavingContainer[i - 1].Item1) / 2) * timeFrame;
                        TotalCostWithRenewable += ((SavingContainer[i].Item2 + SavingContainer[i - 1].Item2) / 2) * timeFrame;
                        TotalWindSaving += ((SavingContainer[i].Item3 + SavingContainer[i - 1].Item3) / 2) * timeFrame;
                        //TotalCostWithoutRenewable += tuple.Item1;
                        //TotalCostWithRenewable += tuple.Item2;
                        //TotalWindSaving += tuple.Item3;
                    }

                    TotalCostWithoutRenewable = Math.Round(TotalCostWithoutRenewable, 2);
                    TotalCostWithRenewable = Math.Round(TotalCostWithRenewable, 2);
                    TotalWindSaving = Math.Round(TotalWindSaving, 2);
                }
                BarSavingData.Clear();
                BarSavingData.Add(new KeyValuePair<string, double>("Cost without wind and solar generators", TotalCostWithoutRenewable));
                BarSavingData.Add(new KeyValuePair<string, double>("Cost", TotalCostWithRenewable));
                OnPropertyChanged(nameof(BarSavingData));
                OnPropertyChanged(nameof(TotalWindSaving));
            }
            catch (TimeoutException ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetTotalWindSaving from database. {0}; Exception type: {1}", ex.Message, ex.GetType());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Repeating request for Saving data");
                try
                {
                    SavingContainer = new ObservableCollection<Tuple<double, double, double, DateTime>>(CalculationEngineUIProxy.Instance.ReadWindFarmSavingDataFromDb(StartTimeSaving, EndTimeSaving));
                    if (SavingContainer != null)
                    {
                        //foreach (Tuple<double, double, double, DateTime> tuple in SavingContainer)
                        for (int i = 1; i < SavingContainer.Count; i++)
                        {
                            timeFrame = (SavingContainer[i].Item4 - SavingContainer[i - 1].Item4).TotalHours;
                            TotalCostWithoutRenewable += ((SavingContainer[i].Item1 + SavingContainer[i - 1].Item1) / 2) * timeFrame;
                            TotalCostWithRenewable += ((SavingContainer[i].Item2 + SavingContainer[i - 1].Item2) / 2) * timeFrame;
                            TotalWindSaving += ((SavingContainer[i].Item3 + SavingContainer[i - 1].Item3) / 2) * timeFrame;
                            //TotalCostWithoutRenewable += tuple.Item1;
                            //TotalCostWithRenewable += tuple.Item2;
                            //TotalWindSaving += tuple.Item3;
                        }

                        TotalCostWithoutRenewable = Math.Round(TotalCostWithoutRenewable, 2);
                        TotalCostWithRenewable = Math.Round(TotalCostWithRenewable, 2);
                        TotalWindSaving = Math.Round(TotalWindSaving, 2);
                    }
                    BarSavingData.Clear();
                    BarSavingData.Add(new KeyValuePair<string, double>("Cost without wind and solar generators", TotalCostWithoutRenewable));
                    BarSavingData.Add(new KeyValuePair<string, double>("Cost", TotalCostWithRenewable));
                    OnPropertyChanged(nameof(BarSavingData));
                    OnPropertyChanged(nameof(TotalWindSaving));
                }
                catch (Exception e)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetTotalWindSaving from database. {0}; Exception type: {1}", e.Message, e.GetType());
                }
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetTotalWindSaving from database. {0}; Exception type: {1}", e.Message, e.GetType());
            }
        }

        private void ViewCO2EmissionDataCommandExecute(object obj)
        {
            TotalCO2Reduction = 0;
            TotalCO2 = 0;
            double averageEmissionWithoutRenewable;
            double averageEmiisionWithRenewable;
            ObservableCollection<Tuple<double, double, DateTime>> tempData;
            GraphCO2EmissionContainer.Clear();
            double timeFrame;
            try
            {
                List<Tuple<double, double, DateTime>> co2Emission = CalculationEngineUIProxy.Instance.GetCO2Emission(StartTimeCO2, EndTimeCO2);
                CO2EmissionContainer = new ObservableCollection<Tuple<double, double, DateTime>>(co2Emission);
                if (CO2EmissionContainer != null && CO2EmissionContainer.Count > 0)
                {
                    //foreach (Tuple<double, double, DateTime> item in CO2EmissionContainer)
                    //{
                    //    TotalCO2 += item.Item1;
                    //    TotalCO2Reduction += (item.Item1 - item.Item2);
                    //}
                    for (int i = 1; i < CO2EmissionContainer.Count; i++)
                    {
                        timeFrame = (CO2EmissionContainer[i].Item3 - CO2EmissionContainer[i - 1].Item3).TotalHours;

                        TotalCO2 += ((CO2EmissionContainer[i].Item1 + CO2EmissionContainer[i - 1].Item1) / 2) * timeFrame;
                        TotalCO2Reduction += ((CO2EmissionContainer[i].Item1 + CO2EmissionContainer[i - 1].Item1) / 2) * timeFrame -
                                             ((CO2EmissionContainer[i].Item2 + CO2EmissionContainer[i - 1].Item2) / 2) * timeFrame;
                    }

                    if (graphSampling != GraphSample.None)
                    {
                        DateTime tempStartTime = StartTimeCO2;
                        DateTime tempEndTime = IncrementTime(tempStartTime);

                        averageEmissionWithoutRenewable = 0;
                        averageEmiisionWithRenewable = 0;

                        while (tempEndTime <= EndTimeCO2)
                        {
                            tempData = new ObservableCollection<Tuple<double, double, DateTime>>(CO2EmissionContainer.Where(x => x.Item3 > tempStartTime && x.Item3 < tempEndTime));
                            if (tempData != null && tempData.Count != 0)
                            {
                                averageEmissionWithoutRenewable = tempData.Average(x => x.Item1);
                                averageEmiisionWithRenewable = tempData.Average(x => x.Item2);
                            }
                            else
                            {
                                averageEmissionWithoutRenewable = 0;
                                averageEmiisionWithRenewable = 0;
                            }
                            averageEmissionWithoutRenewable = averageEmissionWithoutRenewable * (tempEndTime - tempStartTime).TotalHours;
                            averageEmiisionWithRenewable = averageEmiisionWithRenewable * (tempEndTime - tempStartTime).TotalHours;
                            tempStartTime = IncrementTime(tempStartTime);
                            tempEndTime = IncrementTime(tempEndTime);
                            GraphCO2EmissionContainer.Add(new Tuple<double, double, DateTime>(averageEmissionWithoutRenewable, averageEmiisionWithRenewable, tempStartTime));
                        }
                    }
                    else
                    {
                        GraphCO2EmissionContainer = CO2EmissionContainer;
                    }
                }
            }
            catch (TimeoutException tex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetCO2Emission from database. {0}; Exception type: {1}", tex.Message, tex.GetType());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Repeating request for CO2 Emission");

                try
                {
                    List<Tuple<double, double, DateTime>> co2Emission = CalculationEngineUIProxy.Instance.GetCO2Emission(StartTimeCO2, EndTimeCO2);
                    CO2EmissionContainer = new ObservableCollection<Tuple<double, double, DateTime>>(co2Emission);
                    if (CO2EmissionContainer != null && CO2EmissionContainer.Count > 0)
                    {
                        //foreach (Tuple<double, double, DateTime> item in CO2EmissionContainer)
                        //{
                        //    TotalCO2 += item.Item1;
                        //    TotalCO2Reduction += (item.Item1 - item.Item2);
                        //}
                        for (int i = 1; i < CO2EmissionContainer.Count; i++)
                        {
                            timeFrame = (CO2EmissionContainer[i].Item3 - CO2EmissionContainer[i - 1].Item3).TotalHours;

                            TotalCO2 += ((CO2EmissionContainer[i].Item1 + CO2EmissionContainer[i - 1].Item1) / 2) * timeFrame;
                            TotalCO2Reduction += ((CO2EmissionContainer[i].Item1 + CO2EmissionContainer[i - 1].Item1) / 2) * timeFrame -
                                                 ((CO2EmissionContainer[i].Item2 + CO2EmissionContainer[i - 1].Item2) / 2) * timeFrame;
                        }

                        if (graphSampling != GraphSample.None)
                        {
                            DateTime tempStartTime = StartTimeCO2;
                            DateTime tempEndTime = IncrementTime(tempStartTime);

                            averageEmissionWithoutRenewable = 0;
                            averageEmiisionWithRenewable = 0;

                            while (tempEndTime <= EndTimeCO2)
                            {
                                tempData = new ObservableCollection<Tuple<double, double, DateTime>>(CO2EmissionContainer.Where(x => x.Item3 > tempStartTime && x.Item3 < tempEndTime));
                                if (tempData != null && tempData.Count != 0)
                                {
                                    averageEmissionWithoutRenewable = tempData.Average(x => x.Item1);
                                    averageEmiisionWithRenewable = tempData.Average(x => x.Item2);
                                }
                                else
                                {
                                    averageEmissionWithoutRenewable = 0;
                                    averageEmiisionWithRenewable = 0;
                                }
                                averageEmissionWithoutRenewable = averageEmissionWithoutRenewable * (tempEndTime - tempStartTime).TotalHours;
                                averageEmiisionWithRenewable = averageEmiisionWithRenewable * (tempEndTime - tempStartTime).TotalHours;
                                tempStartTime = IncrementTime(tempStartTime);
                                tempEndTime = IncrementTime(tempEndTime);
                                GraphCO2EmissionContainer.Add(new Tuple<double, double, DateTime>(averageEmissionWithoutRenewable, averageEmiisionWithRenewable, tempStartTime));
                            }
                        }
                        else
                        {
                            GraphCO2EmissionContainer = CO2EmissionContainer;
                        }
                    }
                }
                catch (Exception e)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetCO2Emission from database. {0}", e.Message);
                }
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetCO2Emission from database. {0}", e.Message);
            }

            PieData.Clear();
            PieData.Add(new KeyValuePair<string, double>("CO2 Saved", TotalCO2Reduction));
            PieData.Add(new KeyValuePair<string, double>("CO2 Remaining", TotalCO2));

            TotalCO2Reduction = Math.Round(TotalCO2Reduction, 4);

            OnPropertyChanged(nameof(PieData));
            OnPropertyChanged(nameof(TotalCO2Reduction));
            OnPropertyChanged(nameof(GraphCO2EmissionContainer));
        }

        private void ViewIndividualProductionDataCommandExecute(Object obj)
        {
            double ts;
            TotalWindProduction = 0;
            TotalSolarProduction = 0;
            TotalHydroProduction = 0;
            TotalCoalProduction = 0;
            TotalOilProduction = 0;
            TotalSum = 0;
            try
            {
                IndividualContainer = new ObservableCollection<Tuple<double, double, double, double, double, DateTime>>(CalculationEngineUIProxy.Instance.ReadIndividualFarmProductionDataFromDb(StartTimeWind, EndTimeWind));
                if (IndividualContainer != null && IndividualContainer.Count > 0)
                {
                    for (int i = 1; i < individualContainer.Count; i++)
                    {
                        ts = (individualContainer[i].Item6 - individualContainer[i - 1].Item6).TotalHours;
                        TotalWindProduction += ts * (individualContainer[i].Item1 + individualContainer[i - 1].Item1) / 2;
                        TotalSolarProduction += ts * (individualContainer[i].Item2 + individualContainer[i - 1].Item2) / 2;
                        TotalHydroProduction += ts * (individualContainer[i].Item3 + individualContainer[i - 1].Item3) / 2;
                        TotalCoalProduction += ts * (individualContainer[i].Item4 + individualContainer[i - 1].Item4) / 2;
                        TotalOilProduction += ts * (individualContainer[i].Item5 + individualContainer[i - 1].Item5) / 2;
                    }

                    TotalSum = TotalWindProduction + TotalSolarProduction + TotalHydroProduction + TotalCoalProduction + TotalOilProduction;
                    TotalWindProductionPercentage = 100 * TotalWindProduction / TotalSum;
                    TotalSolarProductionPercentage = 100 * TotalSolarProduction / TotalSum;
                    TotalHydroProductionPercentage = 100 * TotalHydroProduction / TotalSum;
                    TotalCoalProductionPercentage = 100 * TotalCoalProduction / TotalSum;
                    TotalOilProductionPercentage = 100 * TotalOilProduction / TotalSum;
                    TotalSum = Math.Round(TotalSum, 2);
                }
            }
            catch (TimeoutException ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error ViewWindProductionData from database. {0}; Exception type: {1}", ex.Message, ex.GetType());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Repeating request for Wind Production");
                try
                {
                    individualContainer = new ObservableCollection<Tuple<double, double, double, double, double, DateTime>>(CalculationEngineUIProxy.Instance.ReadIndividualFarmProductionDataFromDb(StartTimeWind, EndTimeWind));
                    if (IndividualContainer != null && IndividualContainer.Count > 0)
                    {
                        for (int i = 1; i < individualContainer.Count; i++)
                        {
                            ts = (individualContainer[i].Item6 - individualContainer[i - 1].Item6).TotalHours;
                            TotalWindProduction += ts * (individualContainer[i].Item1 + individualContainer[i - 1].Item1) / 2;
                            TotalSolarProduction += ts * (individualContainer[i].Item2 + individualContainer[i - 1].Item2) / 2;
                            TotalHydroProduction += ts * (individualContainer[i].Item3 + individualContainer[i - 1].Item3) / 2;
                            TotalCoalProduction += ts * (individualContainer[i].Item4 + individualContainer[i - 1].Item4) / 2;
                            TotalOilProduction += ts * (individualContainer[i].Item5 + individualContainer[i - 1].Item5) / 2;
                        }

                        TotalSum = TotalWindProduction + TotalSolarProduction + TotalHydroProduction + TotalCoalProduction + TotalOilProduction;
                        TotalWindProductionPercentage = 100 * TotalWindProduction / TotalSum;
                        TotalSolarProductionPercentage = 100 * TotalSolarProduction / TotalSum;
                        TotalHydroProductionPercentage = 100 * TotalHydroProduction / TotalSum;
                        TotalCoalProductionPercentage = 100 * TotalCoalProduction / TotalSum;
                        TotalOilProductionPercentage = 100 * TotalOilProduction / TotalSum;
                        TotalSum = Math.Round(TotalSum, 2);
                    }
                }
                catch (Exception e)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error ViewWindProductionData from database. {0}; Exception type: {1}", e.Message, e.GetType());
                }
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error ViewWindProductionData from database. {0}; Exception type: {1}", e.Message, e.GetType());
            }

            PieDataWind.Clear();
            PieDataWind.Add(new KeyValuePair<string, double>("Wind  ", TotalWindProduction));
            PieDataWind.Add(new KeyValuePair<string, double>("Solar  ", TotalSolarProduction));
            PieDataWind.Add(new KeyValuePair<string, double>("Hydro  ", TotalHydroProduction));
            PieDataWind.Add(new KeyValuePair<string, double>("Coal  ", TotalCoalProduction));
            PieDataWind.Add(new KeyValuePair<string, double>("Oil  ", TotalOilProduction));

            OnPropertyChanged(nameof(PieDataWind));
        }

        private void ChangePeriodCO2CommandExecute(object obj)
        {
            switch (SelectedPeriodCO2)
            {
                case PeriodValues.Last_Hour:
                    StartTimeCO2 = DateTime.Now.AddHours(-1);
                    EndTimeCO2 = DateTime.Now;
                    graphSampling = GraphSample.HourSample;
                    break;

                case PeriodValues.Today:
                    StartTimeCO2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    EndTimeCO2 = DateTime.Now;
                    graphSampling = GraphSample.TodaySample;
                    break;

                case PeriodValues.Last_Month:
                    StartTimeCO2 = DateTime.Now.AddMonths(-1);
                    EndTimeCO2 = DateTime.Now;
                    graphSampling = GraphSample.LastMonthSample;
                    break;

                case PeriodValues.Last_3_Month:
                    StartTimeCO2 = DateTime.Now.AddMonths(-3);
                    EndTimeCO2 = DateTime.Now;
                    graphSampling = GraphSample.Last4MonthSample;
                    break;

                case PeriodValues.Last_Year:
                    StartTimeCO2 = DateTime.Now.AddYears(-1);
                    EndTimeCO2 = DateTime.Now;
                    graphSampling = GraphSample.YearSample;
                    break;

                default:
                    break;
            }
        }

        private void ChangePeriodSavingCommandExecute(object obj)
        {
            switch (SelectedPeriodSaving)
            {
                case PeriodValues.Last_Hour:
                    StartTimeSaving = DateTime.Now.AddHours(-1);
                    EndTimeSaving = DateTime.Now;
                    break;

                case PeriodValues.Today:
                    StartTimeSaving = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    EndTimeSaving = DateTime.Now;
                    break;

                case PeriodValues.Last_Month:
                    StartTimeSaving = DateTime.Now.AddMonths(-1);
                    EndTimeSaving = DateTime.Now;
                    break;

                case PeriodValues.Last_3_Month:
                    StartTimeSaving = DateTime.Now.AddMonths(-3);
                    EndTimeSaving = DateTime.Now;
                    break;

                case PeriodValues.Last_Year:
                    StartTimeSaving = DateTime.Now.AddYears(-1);
                    EndTimeSaving = DateTime.Now;
                    break;

                default:
                    break;
            }
        }

        private void ChangePeriodWindProductionCommandExecute(object obj)
        {
            switch (SelectedPeriodWind)
            {
                case PeriodValues.Last_Hour:
                    StartTimeWind = DateTime.Now.AddHours(-1);
                    EndTimeWind = DateTime.Now;
                    break;

                case PeriodValues.Today:
                    StartTimeWind = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    EndTimeWind = DateTime.Now;
                    break;

                case PeriodValues.Last_Month:
                    StartTimeWind = DateTime.Now.AddMonths(-1);
                    EndTimeWind = DateTime.Now;
                    break;

                case PeriodValues.Last_3_Month:
                    StartTimeWind = DateTime.Now.AddMonths(-3);
                    EndTimeWind = DateTime.Now;
                    break;

                case PeriodValues.Last_Year:
                    StartTimeWind = DateTime.Now.AddYears(-1);
                    EndTimeWind = DateTime.Now;
                    break;

                default:
                    break;
            }
        }

        private void CO2EmissionWithoutRenewablesGraphCheckedCommandExecute(object obj)
        {
            CO2EmissionWithoutRenewableGraphVisibility = true;
        }

        private void CO2EmissionWithoutRenewablesGraphUnCheckedCommandExecute(object obj)
        {
            CO2EmissionWithoutRenewableGraphVisibility = false;
        }

        private void CO2EmissionGraphCheckedCommandExecute(object obj)
        {
            CO2EmissionGraphVisibility = true;
        }

        private void CO2EmissionGraphUnCheckedCommandExecute(object obj)
        {
            CO2EmissionGraphVisibility = false;
        }

        #endregion Command Executions

        #region Private Methods

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

        #endregion Private Methods
    }
}