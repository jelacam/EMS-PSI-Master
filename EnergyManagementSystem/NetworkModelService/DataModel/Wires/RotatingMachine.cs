//-----------------------------------------------------------------------
// <copyright file="RotatingMachine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// RotatingMachine class
    /// </summary>
    public class RotatingMachine : RegulatingCondEq
    {
        private float ratedS;
        public RotatingMachine(long globalId) : base(globalId)
        {

        }

        public override bool Equals(object obj)
        {
            if(base.Equals(obj))
            {
                RotatingMachine r = (RotatingMachine)obj;
                return (r.ratedS == this.ratedS);
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

        public float RatedS { get => ratedS; set => ratedS = value; }

        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ROTATINGMACHINE_RATEDS:
                    return true;

                default:
                    return base.HasProperty(t);
            }
            
        }

        public override void GetProperty(Property prop)
        {
            switch(prop.Id)
            {
                case ModelCode.ROTATINGMACHINE_RATEDS:
                    prop.SetValue(ratedS);
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
                case ModelCode.ROTATINGMACHINE_RATEDS:
                    ratedS = property.AsFloat();
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
