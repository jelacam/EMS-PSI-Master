namespace UIClient.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DashboardViewModel dashboardViewModel;
        private AlarmSummaryViewMode alarmSummaryViewModel;

        public MainWindowViewModel()
        {
            DashboardViewModel = new DashboardViewModel();
            AlarmSummaryViewModel = new AlarmSummaryViewMode();
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

        public AlarmSummaryViewMode AlarmSummaryViewModel
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
