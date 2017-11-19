using EMS.Common;

namespace EMS.Services.NetworkModelService.DataModel.MarketMangement
{
    public class BidTimeSeries : TimeSeries
    {
        private bool blockBid;

        private string direction = string.Empty;

        private bool divisible;

        private string linkedBidsIdentification = string.Empty;

        private float minimumActivationQuantity = 0;

        private float stepIncrementQuantity = 0;

        public bool BlockBid
        {
            get
            {
                return blockBid;
            }

            set
            {
                blockBid = value;
            }
        }

        public string Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }

        public bool Divisible
        {
            get
            {
                return divisible;
            }

            set
            {
                divisible = value;
            }
        }

        public string LinkedBidsIdentification
        {
            get
            {
                return linkedBidsIdentification;
            }

            set
            {
                linkedBidsIdentification = value;
            }
        }

        public float MinimumActivationQuantity
        {
            get
            {
                return minimumActivationQuantity;
            }

            set
            {
                minimumActivationQuantity = value;
            }
        }

        public float StepIncrementQuantity
        {
            get
            {
                return stepIncrementQuantity;
            }
           
            set
            {
                stepIncrementQuantity = value;
            }
        }

        public BidTimeSeries(long globalId) : base(globalId)
        {
        }

        

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                BidTimeSeries x = (BidTimeSeries)obj;
                return (x.blockBid == this.blockBid && x.direction == this.direction && x.divisible == this.divisible &&
                        x.linkedBidsIdentification == this.linkedBidsIdentification && x.minimumActivationQuantity == this.minimumActivationQuantity &&
                        x.stepIncrementQuantity == this.stepIncrementQuantity);
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
                case ModelCode.BIDTIMESERIES_BLOCKBID:
                case ModelCode.BIDTIMESERIES_DIRECTION:
                case ModelCode.BIDTIMESERIES_DIVISIBLE:
                case ModelCode.BIDTIMESERIES_LINKEDBIDSID:
                case ModelCode.BIDTIMESERIES_MINACTQUANTITY:
                case ModelCode.BIDTIMESERIES_STEPINCQUANTITY:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.BIDTIMESERIES_BLOCKBID:
                    prop.SetValue(blockBid);
                    break;

                case ModelCode.BIDTIMESERIES_DIRECTION:
                    prop.SetValue(direction);
                    break;

                case ModelCode.BIDTIMESERIES_DIVISIBLE:
                    prop.SetValue(divisible);
                    break;

                case ModelCode.BIDTIMESERIES_LINKEDBIDSID:
                    prop.SetValue(linkedBidsIdentification);
                    break;

                case ModelCode.BIDTIMESERIES_MINACTQUANTITY:
                    prop.SetValue(minimumActivationQuantity);
                    break;

                case ModelCode.BIDTIMESERIES_STEPINCQUANTITY:
                    prop.SetValue(stepIncrementQuantity);
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
                case ModelCode.BIDTIMESERIES_BLOCKBID:
                    blockBid = property.AsBool();
                    break;

                case ModelCode.BIDTIMESERIES_DIRECTION:
                    direction = property.AsString();
                    break;

                case ModelCode.BIDTIMESERIES_DIVISIBLE:
                    divisible = property.AsBool();
                    break;

                case ModelCode.BIDTIMESERIES_LINKEDBIDSID:
                    linkedBidsIdentification = property.AsString();
                    break;

                case ModelCode.BIDTIMESERIES_MINACTQUANTITY:
                    minimumActivationQuantity = property.AsFloat();
                    break;

                case ModelCode.BIDTIMESERIES_STEPINCQUANTITY:
                    stepIncrementQuantity = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation
    }
}