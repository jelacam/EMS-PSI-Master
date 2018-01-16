using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UIClient.Converters
{
    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
            //Dictionary<long, ObservableCollection<MeasurementUI>> dictionary = value as Dictionary<long, ObservableCollection<MeasurementUI>>;
            //if(dictionary == null || dictionary.Count == 0)
            //{
            //    return null;
            //}
            //return dictionary.ToList()[0].Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
