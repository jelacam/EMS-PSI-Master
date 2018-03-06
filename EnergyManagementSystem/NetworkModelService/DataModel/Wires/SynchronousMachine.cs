//-----------------------------------------------------------------------
// <copyright file="SynchronousMachine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using System.Collections.Generic;
    using Core;
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
        /// operatingMode of synchronous machine
        /// </summary>
        private SynchronousMachineOperatingMode operatingMode;

        /// <summary>
        /// fuel of synchronous machine
        /// </summary>
        private long fuel = 0;

        /// <summary>
        /// active of synchronous machine
        /// </summary>
        private bool active;

        /// <summary>
        /// loadPct of synchronous machine
        /// </summary>
        private float loadPct;

        /// <summary>
        /// maxCosPhi of synchronous machine
        /// </summary>
        private float maxCosPhi;

        /// <summary>
        /// minCosPhi of synchronous machine
        /// </summary>
        private float minCosPhi;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronousMachine" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public SynchronousMachine(long globalId) : base(globalId)
        {
        }

        /// <summary>
        /// Gets or sets Fuel of the entity
        /// </summary>
        public long Fuel
        {
            get
            {
                return this.fuel;
            }
            set
            {
                this.fuel = value;
            }
        }

        /// <summary>
        /// Gets or sets Active of the entity
        /// </summary>
        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
            }
        }

        /// <summary>
        /// Gets or sets LoadPct of the entity
        /// </summary>
        public float LoadPct
        {
            get
            {
                return this.loadPct;
            }
            set
            {
                this.loadPct = value;
            }
        }

        /// <summary>
        /// Gets or sets MaxCosPhi of the entity
        /// </summary>
        public float MaxCosPhi
        {
            get
            {
                return this.maxCosPhi;
            }
            set
            {
                this.maxCosPhi = value;
            }
        }

        /// <summary>
        /// Gets or sets MinCosPhi of the entity
        /// </summary>
        public float MinCosPhi
        {
            get
            {
                return this.minCosPhi;
            }
            set
            {
                this.minCosPhi = value;
            }
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
                return s.MaxQ == this.MaxQ && s.MinQ == this.MinQ && s.OperatingMode == this.OperatingMode &&
                s.Fuel == this.Fuel && s.Active == this.Active && s.LoadPct == this.LoadPct && s.MaxCosPhi == this.MaxCosPhi &&
                s.MinCosPhi == this.MinCosPhi;
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

        public override object Clone()
        {
            SynchronousMachine io = new SynchronousMachine(base.GlobalId);
            io.Active = this.Active;
            io.Fuel = this.Fuel;
            io.LoadPct = this.LoadPct;
            io.MaxCosPhi = this.MaxCosPhi;
            io.MaxQ = this.MaxQ;
            io.Measurements = this.Measurements;
            io.MinCosPhi = this.MinCosPhi;
            io.MinQ = this.MinQ;
            io.Mrid = this.Mrid;
            io.Name = this.Name;
            io.OperatingMode = this.OperatingMode;
            io.RatedS = this.RatedS;

            return io;

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
                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                case ModelCode.SYNCHRONOUSMACHINE_ACTIVE:
                case ModelCode.SYNCHRONOUSMACHINE_FUEL:
                case ModelCode.SYNCHRONOUSMACHINE_LOADPCT:
                case ModelCode.SYNCHRONOUSMACHINE_MAXCOSPHI:
                case ModelCode.SYNCHRONOUSMACHINE_MINCOSPHI:
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
                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    prop.SetValue(this.MaxQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    prop.SetValue(this.MinQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    prop.SetValue((short)this.OperatingMode);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_ACTIVE:
                    prop.SetValue(this.Active);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_FUEL:
                    prop.SetValue(this.Fuel);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_LOADPCT:
                    prop.SetValue(this.LoadPct);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXCOSPHI:
                    prop.SetValue(this.MaxCosPhi);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINCOSPHI:
                    prop.SetValue(this.MinCosPhi);
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
                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    this.MaxQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    this.MinQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    this.OperatingMode = (SynchronousMachineOperatingMode)property.AsEnum();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_ACTIVE:
                    this.Active = property.AsBool();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_FUEL:
                    this.Fuel = property.AsReference();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_LOADPCT:
                    this.LoadPct = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXCOSPHI:
                    this.MaxCosPhi = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINCOSPHI:
                    this.MinCosPhi = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (this.Fuel != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SYNCHRONOUSMACHINE_FUEL] = new List<long>();
                references[ModelCode.SYNCHRONOUSMACHINE_FUEL].Add(this.Fuel);
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation
    }
}
