namespace UIClient.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
        private DashboardViewModel dashboardViewModel;

        public MainWindowViewModel()
        {
            DashboardViewModel = new DashboardViewModel();
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
    }
}
