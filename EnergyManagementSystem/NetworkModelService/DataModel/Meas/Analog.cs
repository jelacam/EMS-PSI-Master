//-----------------------------------------------------------------------
// <copyright file="Analog.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Meas
{
    using EMS.Common;

    /// <summary>
    /// Analog class
    /// </summary>
    public class Analog : Measurement
    {
        /// <summary>
        /// maxValue of analog
        /// </summary>
        private float maxValue;

        /// <summary>
        /// minValue of analog
        /// </summary>
        private float minValue;

        /// <summary>
        /// normalValue of analog
        /// </summary>
        private float normalValue;

        /// <summary>
        /// signalDirection of analog
        /// </summary>
        private SignalDirection signalDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Analog" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public Analog(long globalId) : base(globalId)
        {
        }

        public Analog():base()
        {

        }

        /// <summary>
        /// Gets or sets MaxValue of the entity
        /// </summary>
        public float MaxValue
        {
            get { return this.maxValue; }
            set { this.maxValue = value; }
        }

        /// <summary>
        /// Gets or sets MinValue of the entity
        /// </summary>
        public float MinValue
        {
            get { return this.minValue; }
            set { this.minValue = value; }
        }

        /// <summary>
        /// Gets or sets NormalValue of the entity
        /// </summary>
        public float NormalValue
        {
            get { return this.normalValue; }
            set { this.normalValue = value; }
        }

        /// <summary>
        /// Gets or sets SignalDirection of the entity
        /// </summary>
        public SignalDirection SignalDirection
        {
            get { return this.signalDirection; }
            set { this.signalDirection = value; }
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
                Analog a = (Analog)obj;
                return a.maxValue == this.maxValue && a.minValue == this.minValue && a.normalValue == this.normalValue && a.signalDirection == this.signalDirection;
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
            Analog io = new Analog();
            io.MaxValue = this.MaxValue;
            io.MeasurementType = this.MeasurementType;
            io.MinValue = this.MinValue;
            io.Mrid = this.Mrid;
            io.Name = this.Name;
            io.NormalValue = this.NormalValue;
            io.PowerSystemResource = this.PowerSystemResource;
            io.SignalDirection = this.SignalDirection;
            io.UnitSymbol = this.UnitSymbol;

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
                case ModelCode.ANALOG_MAXVALUE:
                case ModelCode.ANALOG_MINVALUE:
                case ModelCode.ANALOG_NORMALVALUE:
                case ModelCode.ANALOG_SIGNALDIRECTION:
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
                case ModelCode.ANALOG_MAXVALUE:
                    prop.SetValue(this.maxValue);
                    break;

                case ModelCode.ANALOG_MINVALUE:
                    prop.SetValue(this.minValue);
                    break;

                case ModelCode.ANALOG_NORMALVALUE:
                    prop.SetValue(this.normalValue);
                    break;

                case ModelCode.ANALOG_SIGNALDIRECTION:
                    prop.SetValue((short)this.signalDirection);
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
                case ModelCode.ANALOG_MAXVALUE:
                    this.maxValue = property.AsFloat();
                    break;

                case ModelCode.ANALOG_MINVALUE:
                    this.minValue = property.AsFloat();
                    break;

                case ModelCode.ANALOG_NORMALVALUE:
                    this.normalValue = property.AsFloat();
                    break;

                case ModelCode.ANALOG_SIGNALDIRECTION:
                    this.signalDirection = (SignalDirection)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    return;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        #endregion IReference implementation
    }
}