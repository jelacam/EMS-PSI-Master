using System.Windows.Controls;
using UIClient.ViewModel;

namespace UIClient.View
{
	/// <summary>
	/// Interaction logic for ImporterView.xaml
	/// </summary>
	public partial class ImporterView : UserControl
	{
		public ImporterView()
		{
			DataContext = new ImporterViewModel();
			InitializeComponent();
		}
	}
}
