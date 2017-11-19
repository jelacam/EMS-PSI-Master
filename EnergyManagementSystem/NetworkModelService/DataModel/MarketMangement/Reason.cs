using EMS.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMS.Common;

namespace EMS.Services.NetworkModelService.DataModel.MarketMangement
{
    
    public class Reason : IdentifiedObject
    {

        private string code = string.Empty;

        private string text = string.Empty;

        private List<long> marketDocuments = new List<long>();

        private List<long> timeSeries = new List<long>();

        public Reason(long globalId) : base(globalId)
        {

        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public List<long> MarketDocuments
        {
            get
            {
                return marketDocuments;
            }

            set
            {
                marketDocuments = value;
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
                Reason x = (Reason)obj;
                return (x.code == this.code && x.text == this.text && 
                        CompareHelper.CompareLists(x.marketDocuments, this.marketDocuments, false) &&
                        CompareHelper.CompareLists(x.timeSeries, this.timeSeries, false));

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
                case ModelCode.REASON_CODE:
                case ModelCode.REASON_TEXT:
                case ModelCode.REASON_MARKETDOCUMENT:
                case ModelCode.REASON_TIMESERIES:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.REASON_CODE:
                    prop.SetValue(code);
                    break;

                case ModelCode.REASON_TEXT:
                    prop.SetValue(code);
                    break;

                case ModelCode.REASON_MARKETDOCUMENT:
                    prop.SetValue(marketDocuments);
                    break;

                case ModelCode.REASON_TIMESERIES:
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
                case ModelCode.REASON_CODE:
                    code = property.AsString();
                    break;

                case ModelCode.REASON_TEXT:
                    text = property.AsString();
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
                return marketDocuments.Count != 0 || timeSeries.Count != 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            /// target
            if (marketDocuments != null && marketDocuments.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REASON_MARKETDOCUMENT] = marketDocuments.GetRange(0, marketDocuments.Count);
            }

            /// target
            if(timeSeries != null && timeSeries.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REASON_TIMESERIES] = timeSeries.GetRange(0, timeSeries.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                /// model code atributa klase koja referencira target
                case ModelCode.MARKETDOC_REASON:
                    marketDocuments.Add(globalId);
                    break;

                case ModelCode.TIMESERIES_REASON:
                    timeSeries.Add(GlobalId);
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
                case ModelCode.MARKETDOC_REASON:

                    if (marketDocuments.Contains(globalId))
                    {
                        marketDocuments.Remove(globalId);
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
