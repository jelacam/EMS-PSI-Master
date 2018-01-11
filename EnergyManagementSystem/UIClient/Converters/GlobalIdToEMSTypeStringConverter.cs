using EMS.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UIClient.Converters
{
    class GlobalIdToEMSTypeStringConverter : IValueConverter
    {
        EMSTypeToStringConverter converter;
        public GlobalIdToEMSTypeStringConverter()
        {
            converter = new EMSTypeToStringConverter();
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";

            EMSType type = (EMSType)ModelCodeHelper.ExtractTypeFromGlobalId((long)value);

            return converter.Convert(type, null, null, null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
