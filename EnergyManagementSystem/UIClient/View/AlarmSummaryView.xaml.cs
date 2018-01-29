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

namespace UIClient.View
{
    /// <summary>
    /// Interaction logic for AlarmSummaryView.xaml
    /// </summary>
    public partial class AlarmSummaryView : UserControl
    {
        public AlarmSummaryView()
        {
            InitializeComponent();
            AlarmSummaryDataGrid.Items.SortDescriptions.Add(
                                new System.ComponentModel.SortDescription("TimeStamp", System.ComponentModel.ListSortDirection.Descending));
        }

        

        //private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if(e.Column.Header.ToString() == "Message")
        //    {
        //        e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        //    }

        //}
    }
}
