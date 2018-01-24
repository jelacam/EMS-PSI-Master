namespace UIClient.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DashboardViewModel dashboardViewModel;
        private AlarmSummaryViewModel alarmSummaryViewModel;

        public MainWindowViewModel()
        {
            DashboardViewModel = new DashboardViewModel();
            AlarmSummaryViewModel = new AlarmSummaryViewModel();
        }

        public DashboardViewModel DashboardViewModel
        {
            get
            {
                return dashboardViewModel;
            }

            set
            {
                dashboardViewModel = value;
                OnPropertyChanged();
            }
        }

        public AlarmSummaryViewModel AlarmSummaryViewModel
        {
            get
            {
                return alarmSummaryViewModel;
            }

            set
            {
                alarmSummaryViewModel = value;
                OnPropertyChanged();
            }
        }

    }
}
