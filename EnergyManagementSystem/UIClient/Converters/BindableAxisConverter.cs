using EMS.Services.NetworkModelService.DataModel.Core;
using EMS.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UIClient.Converters
{
    public class BindableAxisConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length < 2)
            {
                return null;
            }

            Dictionary<long, IdentifiedObject> nmsModelMap = values[1] as Dictionary<long, IdentifiedObject>;
            long gid = (long)values[0];

            IdentifiedObject idObj = null;
            if (nmsModelMap.TryGetValue(gid, out idObj))
            {

                //if(idObj is EnergyConsumer)
                //{
                //    EnergyConsumer eCons = (EnergyConsumer)idObj;
                //    return DoConversion(0f, int.MaxValue, parameter.ToString());
                //}

                if (idObj is SynchronousMachine)
                {
                    SynchronousMachine syncMach = (SynchronousMachine)idObj;
                    return DoConversion(syncMach.MinQ, syncMach.MaxQ, parameter.ToString());
                }

            }

            return DoConversion(0f, int.MaxValue, parameter.ToString());
        }

        private double DoConversion(float minValue, float maxValue, string param)
        {
            float offset = (maxValue - minValue) / 10;
            if(param == "Max")
            {
                return maxValue + offset;
            }

            if (param == "Min")
            {
                return minValue - offset;
            }

            if(param == "Interval")
            {
                return (int) (maxValue - minValue - offset) / 2;
            }

            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
