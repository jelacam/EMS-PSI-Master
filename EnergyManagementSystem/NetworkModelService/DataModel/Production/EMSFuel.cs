using EMS.Common;
using EMS.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.NetworkModelService.DataModel.Production
{
    public class EMSFuel : IdentifiedObject
    {

        private EmsFuelType fuelType;

        private float unitPrice;

        private List<long> synchronousMachines = new List<long>();

        public EMSFuel(long globalId) : base(globalId)
        {
        }

        public EmsFuelType FuelType
        {
            get { return fuelType; }
            set { fuelType = value; }
        }

        public float UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        public List<long> SynchronousMachines
        {
            get { return synchronousMachines; }
            set { synchronousMachines = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                EMSFuel x = (EMSFuel)obj;
                return x.fuelType == this.fuelType && x.unitPrice == this.unitPrice && 
                       CompareHelper.CompareLists(x.SynchronousMachines, this.SynchronousMachines, true);
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

        public override object Clone()
        {
            EMSFuel io = new EMSFuel(base.GlobalId);
            io.FuelType = this.FuelType;
            io.Mrid = this.Mrid;
            io.Name = this.Name;
            io.SynchronousMachines = this.SynchronousMachines;
            io.UnitPrice = this.UnitPrice;

            return io;
        }

        #region IAccess Implementation

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.EMSFUEL_FUELTYPE:
                case ModelCode.EMSFUEL_UNITPRICE:
                case ModelCode.EMSFUEL_SYNCHRONOUSMACHINES:
                    return true;
                    
                default:
                    return base.HasProperty(t);
            }  
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.EMSFUEL_FUELTYPE:
                    prop.SetValue((short)this.FuelType);
                    break;

                case ModelCode.EMSFUEL_UNITPRICE:
                    prop.SetValue(this.UnitPrice);
                    break;

                case ModelCode.EMSFUEL_SYNCHRONOUSMACHINES:
                    prop.SetValue(this.SynchronousMachines);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
            
        }

        public override void SetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.EMSFUEL_FUELTYPE:
                    this.FuelType = (EmsFuelType)prop.AsEnum();
                    break;
                case ModelCode.EMSFUEL_UNITPRICE:
                    this.UnitPrice = prop.AsFloat();
                    break;

                default:
                    base.SetProperty(prop);
                    break;

            }
            
        }

        #endregion IAccess Implementation


        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return this.SynchronousMachines.Count > 0;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (this.SynchronousMachines != null && this.SynchronousMachines.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.EMSFUEL_SYNCHRONOUSMACHINES] = this.SynchronousMachines.GetRange(0, this.SynchronousMachines.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUEL:
                    this.SynchronousMachines.Add(globalId);
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
                case ModelCode.SYNCHRONOUSMACHINE_FUEL:

                    if (this.SynchronousMachines.Contains(globalId))
                    {
                        this.SynchronousMachines.Remove(globalId);
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
