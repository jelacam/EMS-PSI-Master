using EMS.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMS.Common;

namespace EMS.Services.NetworkModelService.DataModel.MarketMangement
{
    public class MeasurementPoint : IdentifiedObject
    {
        private long timeSeries = 0;


        public MeasurementPoint(long globalId) : base(globalId)
        {

        }

        public long TimeSeries
        {
            get
            {
                return timeSeries;
            }

            set
            {
                timeSeries = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                MeasurementPoint x = (MeasurementPoint)obj;
                return (x.timeSeries == this.timeSeries);

            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.MEASUREMENTPOINT_TIMESERIES:
                    return true;


                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.MEASUREMENTPOINT_TIMESERIES:
                    prop.SetValue(timeSeries);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.MEASUREMENTPOINT_TIMESERIES:
                    timeSeries = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation
    }
}
