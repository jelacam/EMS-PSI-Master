using System;
using System.Windows.Data;

namespace UIClient.Converters
{
    public class StringCompareConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return false;
            }

            string st1 = (string)values[0];
            string st2 = (string)values[1];

            if (st1.Length > 2)
            {
                st1 = st1.Substring(2);
            }

            return st1 == st2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
