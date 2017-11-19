using EMS.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMS.Common;

namespace EMS.Services.NetworkModelService.DataModel.MarketMangement
{
    public class TimeSeries : IdentifiedObject
    {
        private string objectAgregation = string.Empty;

        private string product = string.Empty;

        private string version = string.Empty;

        private long marketDocument = 0;

        private long reason = 0;

        private List<long> measurementPoints = new List<long>();


        public TimeSeries(long globalId) : base (globalId)
        {

        }

        public string ObjectAgregation
        {
            get
            {
                return objectAgregation;
            }

            set
            {
                objectAgregation = value;
            }
        }

        public string Product
        {
            get
            {
                return product;
            }

            set
            {
                product = value;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }

        public long MarketDocument
        {
            get
            {
                return marketDocument;
            }

            set
            {
                marketDocument = value;
            }
        }

        public long Reason
        {
            get
            {
                return reason;
            }

            set
            {
                reason = value;
            }
        }

        public List<long> MeasurementPoints
        {
            get
            {
                return measurementPoints;
            }

            set
            {
                measurementPoints = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                TimeSeries x = (TimeSeries)obj;
                return (x.objectAgregation == this.objectAgregation && x.product == this.product  && x.version == this.version &&
                        x.reason == this.reason && x.product == this.product && 
                        CompareHelper.CompareLists(x.measurementPoints, this.measurementPoints, false));

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
                case ModelCode.TIMESERIES_OBJAGR:
                case ModelCode.TIMESERIES_PRODUCT:
                case ModelCode.TIMESERIES_VERSION:
                case ModelCode.TIMESERIES_MARKETDOC:
                case ModelCode.TIMESERIES_REASON:
                case ModelCode.TIMESERIES_MEASUREMENTPOINT:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.TIMESERIES_OBJAGR:
                    prop.SetValue(objectAgregation);
                    break;

                case ModelCode.TIMESERIES_PRODUCT:
                    prop.SetValue(product);
                    break;

                case ModelCode.TIMESERIES_VERSION:
                    prop.SetValue(version);
                    break;

                case ModelCode.TIMESERIES_MARKETDOC:
                    prop.SetValue(marketDocument);
                    break;

                case ModelCode.TIMESERIES_REASON:
                    prop.SetValue(reason);
                    break;

                case ModelCode.TIMESERIES_MEASUREMENTPOINT:
                    prop.SetValue(measurementPoints);
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
                case ModelCode.TIMESERIES_OBJAGR:
                    objectAgregation = property.AsString();
                    break;

                case ModelCode.TIMESERIES_PRODUCT:
                    product = property.AsString();
                    break;

                case ModelCode.TIMESERIES_VERSION:
                    version = property.AsString();
                    break;

                case ModelCode.TIMESERIES_MARKETDOC:
                    marketDocument = property.AsReference();
                    break;

                case ModelCode.TIMESERIES_REASON:
                    reason = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return measurementPoints.Count != 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            /// target
            if (measurementPoints != null && measurementPoints.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TIMESERIES_MEASUREMENTPOINT] = measurementPoints.GetRange(0, measurementPoints.Count);
            }

            /// reference
            if (reason != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TIMESERIES_REASON] = new List<long>();
                references[ModelCode.TIMESERIES_REASON].Add(reason);
            }

            if (marketDocument != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TIMESERIES_MARKETDOC] = new List<long>();
                references[ModelCode.TIMESERIES_MARKETDOC].Add(marketDocument);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                /// model code atributa klase koja referencira target
                case ModelCode.MEASUREMENTPOINT_TIMESERIES:
                    measurementPoints.Add(globalId);
                    break;


                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                /// model code atributa klase koja referencira target
                case ModelCode.MEASUREMENTPOINT_TIMESERIES:

                    if (measurementPoints.Contains(globalId))
                    {
                        measurementPoints.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation
    }
}
