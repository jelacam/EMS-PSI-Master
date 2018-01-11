using EMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UIClient.Converters
{
    public class EMSTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            var type = (EMSType)value;

            switch (type)
            {
                case EMSType.ANALOG:
                    return "Analog";
                case EMSType.ENERGYCONSUMER:
                    return "EnergyConsumer";
                case EMSType.SYNCHRONOUSMACHINE:
                    return "SynchronousMachine";
                case EMSType.EMSFUEL:
                    return "EMSFuel";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.ToObject(targetType, value);
        }
    }
}
