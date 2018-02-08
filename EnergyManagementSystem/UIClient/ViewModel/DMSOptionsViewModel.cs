
using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private DateTime startTime;
        private DateTime endTime;
        private DateTime startTimeWind;
        private DateTime endTimeWind;
        private DateTime startTimeSaving;
        private DateTime endTimeSaving;
        private PeriodValues selectedPeriod;
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

        #endregion

        public DMSOptionsViewModel(DMSOptionsView mainWindow)
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            StartTimeWind = DateTime.Now;
            EndTimeWind = DateTime.Now;
            StartTimeSaving = DateTime.Now;
            EndTimeSaving = DateTime.Now;
            TotalCO2Reduction = 0;
            TotalCO2 = 0;
            TotalWindSaving = 0;
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

        public PeriodValues SelectedPeriodWind
        {
            get
            {
                return selectedPeriodWind;
            }
            set
            {
                selectedPeriodWind = value;
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
            }
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

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged(nameof(EndTime));
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

        #endregion

        #region Commands

        public ICommand ViewCO2EmissionDataCommand => viewCO2EmissionDataCommand ?? (viewCO2EmissionDataCommand = new RelayCommand(ViewCO2EmissionDataCommandExecute));

        public ICommand ChangePeriodCO2Command => changePeriodCo2Command ?? (changePeriodCo2Command = new RelayCommand(ChangePeriodCO2CommandExecute));

        public ICommand ViewWindProductionDataCommand => viewWindProductionDataCommand ?? (viewWindProductionDataCommand = new RelayCommand(ViewWindProductionDataCommandExecute));

        public ICommand ChangePeriodWindProductionCommand => changePeriodWindProductionCommand ?? (changePeriodWindProductionCommand = new RelayCommand(ChangePeriodWindProductionCommandExecute));

        public ICommand ChangePeriodSavingCommand => changePeriodSavingCommand ?? (changePeriodSavingCommand = new RelayCommand(ChangePeriodSavingCommandExecute));

        public ICommand ViewSavingnDataCommand => viewSavingnDataCommand ?? (viewSavingnDataCommand = new RelayCommand(ViewSavingnDataCommandExecute));
        #endregion

        #region Command Executions


        private void ViewSavingnDataCommandExecute(object obj)
        {
            TotalWindSaving = 0;
            TotalCostWithoutRenewable = 0;
            TotalCostWithRenewable = 0;
            try
            {
                SavingContainer= new ObservableCollection<Tuple<double, double, double>>(CalculationEngineUIProxy.Instance.ReadWindFarmSavingDataFromDb(StartTimeSaving, EndTimeSaving));
                if (SavingContainer != null)
                {
                    foreach(Tuple<double, double, double> tuple in SavingContainer)
                    {
                        TotalCostWithoutRenewable += tuple.Item1;
                        TotalCostWithRenewable += tuple.Item2;
                        TotalWindSaving += tuple.Item3;
                    }
                }
                BarSavingData.Clear();
                BarSavingData.Add(new KeyValuePair<string, double>("Total_Cost_Without_Renewable", TotalCostWithoutRenewable));
                BarSavingData.Add(new KeyValuePair<string, double>("Total_Cost_With_Renewable", TotalCostWithRenewable));
                OnPropertyChanged(nameof(BarSavingData));
                OnPropertyChanged(nameof(TotalWindSaving));

            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetTotalWindSaving from database. {0}", ex.Message);
            }

        }

        private void ViewCO2EmissionDataCommandExecute(object obj)
        {
            TotalCO2Reduction = 0;
            TotalCO2 = 0;
            try
            {
                CO2EmissionContainer = new ObservableCollection<Tuple<double, double, DateTime>>(CalculationEngineUIProxy.Instance.GetCO2Emission(StartTime, EndTime));
                if(CO2EmissionContainer!=null && CO2EmissionContainer.Count > 0)
                {
                    foreach(Tuple<double,double,DateTime> item in CO2EmissionContainer)
                    {
                        TotalCO2 += item.Item1;
                        TotalCO2Reduction += (item.Item1 - item.Item2);
                    }
                }
            }
            catch(Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetCO2Emission from database. {0}", ex.Message);
            }
            PieData.Clear();
            PieData.Add(new KeyValuePair<string, double>("CO2_Saved", TotalCO2Reduction));
            PieData.Add(new KeyValuePair<string, double>("CO2_Remaining", TotalCO2));

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
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[DMSOptionsViewModel] Error GetCO2Emission from database. {0}", ex.Message);
            }
            PieDataWind.Clear();
            PieDataWind.Add(new KeyValuePair<string, double>("Wind Production", TotalWindProductionPercentage));
            PieDataWind.Add(new KeyValuePair<string, double>("Other", rest));

            OnPropertyChanged(nameof(PieDataWind));
        }

        private void ChangePeriodCO2CommandExecute(object obj)
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

        private void ChangePeriodSavingCommandExecute(object obj)
        {
            if (SelectedPeriodSaving == PeriodValues.Last_Hour)
            {
                StartTimeSaving = DateTime.Now.AddHours(-1);
                EndTimeSaving = DateTime.Now;
            }
            else if (SelectedPeriodSaving == PeriodValues.Today)
            {
                StartTimeSaving = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                EndTimeSaving = DateTime.Now;
            }
            else if (SelectedPeriodSaving == PeriodValues.Last_Year)
            {
                StartTimeSaving = DateTime.Now.AddYears(-1);
                EndTimeSaving = DateTime.Now;
            }
            else if (SelectedPeriodSaving == PeriodValues.Last_Month)
            {
                StartTimeSaving = DateTime.Now.AddMonths(-1);
                EndTimeSaving = DateTime.Now;
            }
            else if (SelectedPeriodSaving == PeriodValues.Last_4_Month)
            {
                StartTimeSaving = DateTime.Now.AddMonths(-4);
                EndTimeSaving = DateTime.Now;
            }
        }



        private void ChangePeriodWindProductionCommandExecute(object obj)
        {
            if (SelectedPeriodWind == PeriodValues.Last_Hour)
            {
                StartTimeWind = DateTime.Now.AddHours(-1);
                EndTimeWind = DateTime.Now;
            }
            else if (SelectedPeriodWind == PeriodValues.Today)
            {
                StartTimeWind = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                EndTimeWind = DateTime.Now;
            }
            else if (SelectedPeriodWind == PeriodValues.Last_Year)
            {
                StartTimeWind = DateTime.Now.AddYears(-1);
                EndTimeWind = DateTime.Now;
            }
            else if (SelectedPeriodWind == PeriodValues.Last_Month)
            {
                StartTimeWind = DateTime.Now.AddMonths(-1);
                EndTimeWind = DateTime.Now;
            }
            else if (SelectedPeriodWind == PeriodValues.Last_4_Month)
            {
                StartTimeWind = DateTime.Now.AddMonths(-4);
                EndTimeWind = DateTime.Now;
            }
        }

        #endregion
    }
}
