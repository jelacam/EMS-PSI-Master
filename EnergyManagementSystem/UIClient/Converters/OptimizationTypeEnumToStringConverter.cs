using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UIClient.Converters
{
    public class OptimizationTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is OptimizationType)
            {
                switch ((OptimizationType)value)
                {
                    case OptimizationType.Linear:
                        return "Linear Programming";
                    case OptimizationType.Genetic:
                        return "Genetic Algorithm";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
