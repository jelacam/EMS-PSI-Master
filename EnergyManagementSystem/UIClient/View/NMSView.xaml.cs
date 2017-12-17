using System.Windows.Controls;
using UIClient.ViewModel;

namespace UIClient.View
{
	/// <summary>
	/// Interaction logic for NMSView.xaml
	/// </summary>
	public partial class NMSView : UserControl
	{
		public NMSView()
		{
			DataContext = new NMSViewModel(this);
			InitializeComponent();
		}
	}
}
