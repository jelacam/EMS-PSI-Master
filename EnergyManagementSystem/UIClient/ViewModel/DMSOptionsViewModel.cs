using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Input;
using UIClient.Model;
using UIClient.View;

namespace UIClient.ViewModel
{
    public class DMSOptionsViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Tuple<double, double, DateTime>> cO2EmissionContainer = new ObservableCollection<Tuple<double, double, DateTime>>();
        private ObservableCollection<Tuple<double, double>> windContainer = new ObservableCollection<Tuple<double, double>>();
        private ObservableCollection<Tuple<double, double, double>> savingContainer = new ObservableCollection<Tuple<double, double, double>>();
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
        private ICommand viewWindProductionDataCommand;
        private ICommand viewSavingnDataCommand;
        private ICommand changePeriodCo2Command;
        private ICommand changePeriodWindProductionCommand;
        private ICommand changePeriodSavingCommand;
        private double totalCO2Reduction;
        private double totalCO2;
        private double totalWindProductionPercentage;
        private double totalWindProduction;
        private double totalWindSaving;
        private double totalCostWithoutRenewable;
        private double totalCostWithRenewable;

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
        }

        #region Properties

        public ObservableCollection<Tuple<double, double, double>> SavingContainer
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

        public ObservableCollection<Tuple<double, double>> WindContainer
        {
            get
            {
                return windContainer;
            }
            set
            {
                windContainer = value;
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

        public ICommand ViewWindProductionDataCommand => viewWindProductionDataCommand ?? (viewWindProductionDataCommand = new RelayCommand(ViewWindProductionDataCommandExecute));

        public ICommand ChangePeriodWindProductionCommand => changePeriodWindProductionCommand ?? (changePeriodWindProductionCommand = new RelayCommand(ChangePeriodWindProductionCommandExecute));

        public ICommand ChangePeriodSavingCommand => changePeriodSavingCommand ?? (changePeriodSavingCommand = new RelayCommand(ChangePeriodSavingCommandExecute));

        public ICommand ViewSavingnDataCommand => viewSavingnDataCommand ?? (viewSavingnDataCommand = new RelayCommand(ViewSavingnDataCommandExecute));

        #endregion Commands

        #region Command Executions

        private void ViewSavingnDataCommandExecute(object obj)
        {
            TotalWindSaving = 0;
            TotalCostWithoutRenewable = 0;
            TotalCostWithRenewable = 0;
            try
            {
                SavingContainer = new ObservableCollection<Tuple<double, double, double>>(CalculationEngineUIProxy.Instance.ReadWindFarmSavingDataFromDb(StartTimeSaving, EndTimeSaving));
                if (SavingContainer != null)
                {
                    foreach (Tuple<double, double, double> tuple in SavingContainer)
                    {
                        TotalCostWithoutRenewable += tuple.Item1;
                        TotalCostWithRenewable += tuple.Item2;
                        TotalWindSaving += tuple.Item3;
                    }
                }
                BarSavingData.Clear();
                BarSavingData.Add(new KeyValuePair<string, double>("Total Cost Without Renewable", TotalCostWithoutRenewable));
                BarSavingData.Add(new KeyValuePair<string, double>("Total Cost With Renewable", TotalCostWithRenewable));
                OnPropertyChanged(nameof(BarSavingData));
                OnPropertyChanged(nameof(TotalWindSaving));
            }
            catch (TimeoutException ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetTotalWindSaving from database. {0}; Exception type: {1}", ex.Message, ex.GetType());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Repeating request for Saving data");

                try
                {
                    SavingContainer = new ObservableCollection<Tuple<double, double, double>>(CalculationEngineUIProxy.Instance.ReadWindFarmSavingDataFromDb(StartTimeSaving, EndTimeSaving));
                    if (SavingContainer != null)
                    {
                        foreach (Tuple<double, double, double> tuple in SavingContainer)
                        {
                            TotalCostWithoutRenewable += tuple.Item1;
                            TotalCostWithRenewable += tuple.Item2;
                            TotalWindSaving += tuple.Item3;
                        }
                    }
                    BarSavingData.Clear();
                    BarSavingData.Add(new KeyValuePair<string, double>("Total Cost Without Renewable", TotalCostWithoutRenewable));
                    BarSavingData.Add(new KeyValuePair<string, double>("Total Cost With Renewable", TotalCostWithRenewable));
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
            try
            {
                List<Tuple<double, double, DateTime>> co2Emission = CalculationEngineUIProxy.Instance.GetCO2Emission(StartTimeCO2, EndTimeCO2);
                CO2EmissionContainer = new ObservableCollection<Tuple<double, double, DateTime>>(co2Emission);
                if (CO2EmissionContainer != null && CO2EmissionContainer.Count > 0)
                {
                    foreach (Tuple<double, double, DateTime> item in CO2EmissionContainer)
                    {
                        TotalCO2 += item.Item1;
                        TotalCO2Reduction += (item.Item1 - item.Item2);
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
                        foreach (Tuple<double, double, DateTime> item in CO2EmissionContainer)
                        {
                            TotalCO2 += item.Item1;
                            TotalCO2Reduction += (item.Item1 - item.Item2);
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

            TotalCO2Reduction = Math.Round(TotalCO2Reduction, 2);

            OnPropertyChanged(nameof(PieData));
            OnPropertyChanged(nameof(TotalCO2Reduction));
            OnPropertyChanged(nameof(CO2EmissionContainer));
        }

        private void ViewWindProductionDataCommandExecute(Object obj)
        {
            int i = 0;
            double rest = 100;
            TotalWindProduction = 0;
            try
            {
                WindContainer = new ObservableCollection<Tuple<double, double>>(CalculationEngineUIProxy.Instance.ReadWindFarmProductionDataFromDb(StartTimeWind, EndTimeWind));
                if (WindContainer != null && WindContainer.Count > 0)
                {
                    foreach (Tuple<double, double> item in WindContainer)
                    {
                        i++;
                        TotalWindProduction += item.Item1;
                        TotalWindProductionPercentage += item.Item2;
                    }
                    TotalWindProductionPercentage = TotalWindProductionPercentage / i;
                    rest = rest - TotalWindProductionPercentage;
                }
            }
            catch (TimeoutException ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error ViewWindProductionData from database. {0}; Exception type: {1}", ex.Message, ex.GetType());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Repeating request for Wind Production");

                try
                {
                    WindContainer = new ObservableCollection<Tuple<double, double>>(CalculationEngineUIProxy.Instance.ReadWindFarmProductionDataFromDb(StartTimeWind, EndTimeWind));
                    if (WindContainer != null && WindContainer.Count > 0)
                    {
                        foreach (Tuple<double, double> item in WindContainer)
                        {
                            i++;
                            TotalWindProduction += item.Item1;
                            TotalWindProductionPercentage += item.Item2;
                        }
                        TotalWindProductionPercentage = TotalWindProductionPercentage / i;
                        rest = rest - TotalWindProductionPercentage;
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
            PieDataWind.Add(new KeyValuePair<string, double>("Wind Production", TotalWindProductionPercentage));
            PieDataWind.Add(new KeyValuePair<string, double>("Other", rest));

            OnPropertyChanged(nameof(PieDataWind));
        }

        private void ChangePeriodCO2CommandExecute(object obj)
        {
            switch (SelectedPeriodCO2)
            {
                case PeriodValues.Last_Hour:
                    StartTimeCO2 = DateTime.Now.AddHours(-1);
                    EndTimeCO2 = DateTime.Now;
                    break;

                case PeriodValues.Today:
                    StartTimeCO2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    EndTimeCO2 = DateTime.Now;
                    break;

                case PeriodValues.Last_Month:
                    StartTimeCO2 = DateTime.Now.AddMonths(-1);
                    EndTimeCO2 = DateTime.Now;
                    break;

                case PeriodValues.Last_4_Month:
                    StartTimeCO2 = DateTime.Now.AddMonths(-4);
                    EndTimeCO2 = DateTime.Now;
                    break;

                case PeriodValues.Last_Year:
                    StartTimeCO2 = DateTime.Now.AddYears(-1);
                    EndTimeCO2 = DateTime.Now;
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

                case PeriodValues.Last_4_Month:
                    StartTimeSaving = DateTime.Now.AddMonths(-4);
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

                case PeriodValues.Last_4_Month:
                    StartTimeWind = DateTime.Now.AddMonths(-4);
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

        #endregion Command Executions
    }
}