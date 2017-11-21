//-----------------------------------------------------------------------
// <copyright file="SynchronousMachine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// SynchronousMachine class
    /// </summary>
    public class SynchronousMachine : RotatingMachine
    {
        private float maxQ;
        private float minQ;
        private EmsFuelType fuelType;
        private SynchronousMachineOperatingMode operatingMode;
        public SynchronousMachine(long globalId) : base(globalId)
        {

        }

        public override bool Equals(object obj)
        {
            if( base.Equals(obj))
            {
                SynchronousMachine s = (SynchronousMachine)obj;
                return (s.maxQ == this.maxQ
                    && s.minQ == this.minQ
                    && s.fuelType == this.fuelType
                    && s.operatingMode == this.operatingMode);
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

        public float MaxQ { get => maxQ; set => maxQ = value; }
        public float MinQ { get => minQ; set => minQ = value; }
        public EmsFuelType FuelType { get => fuelType; set => fuelType = value; }
        public SynchronousMachineOperatingMode OperatingMode { get => operatingMode; set => operatingMode = value; }


        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch(t)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    return true;
                default:
                    return base.HasProperty(t);
            }
            
        }

        public override void GetProperty(Property prop)
        {
            switch(prop.Id)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                    prop.SetValue((short)fuelType);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    prop.SetValue(maxQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    prop.SetValue(minQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    prop.SetValue((short)operatingMode);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
            
        }

        public override void SetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                    fuelType = (EmsFuelType)property.AsEnum();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    maxQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    minQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    operatingMode = (SynchronousMachineOperatingMode)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
            
        }

        #endregion IAccess implementation

        #region IReference implementation

        //koliko znam ovde ne treba nista, ali neka cr proveri

        #endregion IReference implementation
    }
}
