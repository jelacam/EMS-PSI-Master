using EMS.Common;
using EMS.Services.NetworkModelService.DataModel.Meas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CommonMeasurement
{
    public static class ResourcesDescriptionConverter
    {
        public static Analog ConvertToAnalog(ResourceDescription rd)
        {
            Analog analog = null;

            analog = new Analog(rd.Id);
            Property property = null;
            property = rd.GetProperty(ModelCode.ANALOG_MAXVALUE);
            if (property != null)
            {
                analog.SetProperty(property);
                property = null;
            }
            property = rd.GetProperty(ModelCode.ANALOG_MINVALUE);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.ANALOG_NORMALVALUE);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.ANALOG_SIGNALDIRECTION);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.MEASUREMENT_UNITSYMBOL);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_GID);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            if (property != null)
            {
                analog.SetProperty(property);
            }
            property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
            if (property != null)
            {
                analog.SetProperty(property);
            }

            return analog;
        }
    }
}