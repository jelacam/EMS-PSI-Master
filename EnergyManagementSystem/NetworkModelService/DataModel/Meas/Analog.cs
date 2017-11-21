//-----------------------------------------------------------------------
// <copyright file="Analog.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Meas
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// Analog class
    /// </summary>
    public class Analog : Measurement
    {
        private float maxValue;
        private float minValue;
        private float normalValue;
        private SignalDirection signalDirection;

        public Analog(long globalId) : base(globalId)
        {

        }

        public override bool Equals(object obj)
        {
            if(base.Equals(obj))
            {
                Analog a = (Analog)obj;
                return (a.maxValue == this.maxValue
                    && a.minValue == this.minValue
                    && a.normalValue == this.normalValue
                    && a.signalDirection == this.signalDirection);
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

        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public float MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public float NormalValue
        {
            get { return normalValue; }
            set { normalValue = value; }
        }

        public SignalDirection SignalDirection
        {
            get { return signalDirection; }
            set { signalDirection = value; }
        }


        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch(t)
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

        public override void GetProperty(Property prop)
        {
            switch(prop.Id)
            {
                case ModelCode.ANALOG_MAXVALUE:
                    prop.SetValue(maxValue);
                    break;

                case ModelCode.ANALOG_MINVALUE:
                    prop.SetValue(minValue);
                    break;

                case ModelCode.ANALOG_NORMALVALUE:
                    prop.SetValue(normalValue);
                    break;

                case ModelCode.ANALOG_SIGNALDIRECTION:
                    prop.SetValue((short)signalDirection);
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
                case ModelCode.ANALOG_MAXVALUE:
                    maxValue = property.AsFloat();
                    break;

                case ModelCode.ANALOG_MINVALUE:
                    minValue = property.AsFloat();
                    break;

                case ModelCode.ANALOG_NORMALVALUE:
                    normalValue = property.AsFloat();
                    break;

                case ModelCode.ANALOG_SIGNALDIRECTION:
                    signalDirection = (SignalDirection)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    return;
            }
            
        }

        #endregion IAccess implementation

        #region IReference implementation

       //koliko znam ovde ne treba nista, ali neka cr proveri

        #endregion IReference implementation

    }
}
