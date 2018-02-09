using System.Collections.Generic;

namespace UIClient.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DashboardViewModel dashboardViewModel;
        private AlarmSummaryViewModel alarmSummaryViewModel;
        public DockManagerViewModel DockManagerViewModel { get; private set; }

        public MainWindowViewModel()
        {
            var documents = new List<ViewModelBase>();

            DashboardViewModel = new DashboardViewModel();
            DashboardViewModel.Title = "Dashboard";
            AlarmSummaryViewModel = new AlarmSummaryViewModel();
            AlarmSummaryViewModel.Title = "Alarm Summary";

            documents.Add(DashboardViewModel);
            documents.Add(new NMSViewModel(new View.NMSView()) { Title = "NMS"});
            documents.Add(new ImporterViewModel() { Title = "Importer" });
            documents.Add(new HistoryViewModel(new View.HistoryView()) { Title = "History" });
            documents.Add(new DMSOptionsViewModel( new View.DMSOptionsView()) { Title = "DMS" });
            documents.Add(AlarmSummaryViewModel);

            this.DockManagerViewModel = new DockManagerViewModel(documents);
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
