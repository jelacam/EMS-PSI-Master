using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIClient.ViewModel;

namespace UIClient.View
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //DashboardViewModel viewModel = DataContext as DashboardViewModel;
            //if(viewModel == null)
            //{
            //    return;
            //}
            //if (e.WidthChanged)
            //{
            //    if (e.NewSize.Width < 20 && viewModel.IsOptionsExpanded)
            //    {
            //        viewModel.IsOptionsExpanded = false;
            //    }
            //    else if (e.NewSize.Width > 20 && !viewModel.IsOptionsExpanded)
            //    {
            //        viewModel.IsOptionsExpanded = true;
            //    }

            //}
        }
    }
}
