
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
        private ObservableCollection<KeyValuePair<string, double>> pieData = new ObservableCollection<KeyValuePair<string, double>>();
        private DateTime startTime;
        private DateTime endTime;
        private PeriodValues selectedPeriod;
        private ICommand viewCO2EmissionDataCommand;
        private ICommand changePeriodCo2Command;
        private double totalCO2Reduction;
        private double totalCO2;

        #endregion

        public DMSOptionsViewModel(DMSOptionsView mainWindow)
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            TotalCO2Reduction = 0;
            totalCO2 = 0;
        }

        #region Properties

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

        #endregion

        #region Commands

        public ICommand ViewCO2EmissionDataCommand => viewCO2EmissionDataCommand ?? (viewCO2EmissionDataCommand = new RelayCommand(ViewCO2EmissionDataCommandExecute));

        public ICommand ChangePeriodCO2Command => changePeriodCo2Command ?? (changePeriodCo2Command = new RelayCommand(ChangePeriodCO2CommandExecute));

        #endregion

        #region Command Executions

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

        #endregion
    }
}
