//-----------------------------------------------------------------------
// <copyright file="SynchronousMachine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using EMS.Common;

    /// <summary>
    /// SynchronousMachine class
    /// </summary>
    public class SynchronousMachine : RotatingMachine
    {
        /// <summary>
        /// maxQ of synchronous machine
        /// </summary>
        private float maxQ;

        /// <summary>
        /// minQ of synchronous machine
        /// </summary>
        private float minQ;

        /// <summary>
        /// fuelType of synchronous machine
        /// </summary>
        private EmsFuelType fuelType;

        /// <summary>
        /// operatingMode of synchronous machine
        /// </summary>
        private SynchronousMachineOperatingMode operatingMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronousMachine" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public SynchronousMachine(long globalId) : base(globalId)
        {
        }

        /// <summary>
        /// Gets or sets MaxQ of the entity
        /// </summary>
        public float MaxQ
        {
            get
            {
                return this.maxQ;
            }

            set
            {
                this.maxQ = value;
            }
        }

        /// <summary>
        /// Gets or sets MinQ of the entity
        /// </summary>
        public float MinQ
        {
            get
            {
                return this.minQ;
            }

            set
            {
                this.minQ = value;
            }
        }

        /// <summary>
        /// Gets or sets FuelType of the entity
        /// </summary>
        public EmsFuelType FuelType
        {
            get
            {
                return this.fuelType;
            }

            set
            {
                this.fuelType = value;
            }
        }

        /// <summary>
        /// Gets or sets OperatingMode of the entity
        /// </summary>
        public SynchronousMachineOperatingMode OperatingMode
        {
            get
            {
                return this.operatingMode;
            }

            set
            {
                this.operatingMode = value;
            }
        }

        /// <summary>
        /// Checks are the entities equals
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <returns>indicator of equality</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SynchronousMachine s = (SynchronousMachine)obj;
                return s.MaxQ == this.MaxQ && s.MinQ == this.MinQ && s.FuelType == this.FuelType && s.OperatingMode == this.OperatingMode;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns hash code of the entity
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation
        /// <summary>
        /// Checks if the entity has a property
        /// </summary>
        /// <param name="t">model code of property</param>
        /// <returns>indicator of has property</returns>
        public override bool HasProperty(ModelCode t)
        {
            switch (t)
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

        /// <summary>
        /// Gets the property
        /// </summary>
        /// <param name="prop">property to get</param>
        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                    prop.SetValue((short)this.FuelType);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    prop.SetValue(this.MaxQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    prop.SetValue(this.MinQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    prop.SetValue((short)this.OperatingMode);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        /// <summary>
        /// Sets the property
        /// </summary>
        /// <param name="property">property to set</param>
        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                    this.FuelType = (EmsFuelType)property.AsEnum();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    this.MaxQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    this.MinQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    this.OperatingMode = (SynchronousMachineOperatingMode)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        #endregion IReference implementation
    }
}
