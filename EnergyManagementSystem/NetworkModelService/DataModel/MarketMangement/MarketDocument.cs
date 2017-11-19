using EMS.Services.NetworkModelService.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMS.Common;
using EMS.Services.NetworkModelService.DataModel.Core;

namespace EMS.Services.NetworkModelService.DataModel.MarketMangement
{
    public class MarketDocument : Document
    {

        private long reason = 0;

        private long process = 0;

        private List<long> timeSeries = new List<long>();

        public MarketDocument(long globalId) : base(globalId)
        {

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

        public long Process
        {
            get
            {
                return process;
            }

            set
            {
                process = value;
            }
        }

        public List<long> TimeSeries
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
                MarketDocument x = (MarketDocument)obj;
                return (x.reason == this.reason && x.Process == this.Process &&
                        CompareHelper.CompareLists(x.TimeSeries, this.TimeSeries, false));

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
                case ModelCode.MARKETDOC_REASON:
                case ModelCode.MARKETDOC_PROCESS:
                case ModelCode.MARKETDOC_TIMESERIES:
                    return true;
                  

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.MARKETDOC_REASON:
                    prop.SetValue(reason);
                    break;

                case ModelCode.MARKETDOC_PROCESS:
                    prop.SetValue(Process);
                    break;

                case ModelCode.MARKETDOC_TIMESERIES:
                    prop.SetValue(TimeSeries);
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
                case ModelCode.MARKETDOC_REASON:
                    reason = property.AsReference();
                    break;

                case ModelCode.MARKETDOC_PROCESS:
                    process = property.AsReference();
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
                return timeSeries.Count != 0 || base.IsReferenced;
            }
        }


        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            /// reference
            if (reason != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.MARKETDOC_REASON] = new List<long>();
                references[ModelCode.MARKETDOC_REASON].Add(reason);
            }

            if (process != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.MARKETDOC_PROCESS] = new List<long>();
                references[ModelCode.MARKETDOC_PROCESS].Add(process);
            }

            /// target
            if (timeSeries != null && timeSeries.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.MARKETDOC_TIMESERIES] = timeSeries.GetRange(0, timeSeries.Count);
            }
            

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                /// model code atributa klase koja referencira target
                case ModelCode.TIMESERIES_MARKETDOC:
                    timeSeries.Add(globalId);
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
                case ModelCode.TIMESERIES_MARKETDOC:

                    if (timeSeries.Contains(globalId))
                    {
                        timeSeries.Remove(globalId);
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
