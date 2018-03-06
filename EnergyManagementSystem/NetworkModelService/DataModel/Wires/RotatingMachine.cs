//-----------------------------------------------------------------------
// <copyright file="RotatingMachine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using EMS.Common;

    /// <summary>
    /// RotatingMachine class
    /// </summary>
    public class RotatingMachine : RegulatingCondEq
    {
        /// <summary>
        /// ratedS of rotating machine
        /// </summary>
        private float ratedS;

        /// <summary>
        /// Initializes a new instance of the <see cref="RotatingMachine" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public RotatingMachine(long globalId) : base(globalId)
        {
        }

        /// <summary>
        /// Gets or sets RatedS of the entity
        /// </summary>
        public float RatedS
        {
            get { return this.ratedS; }
            set { this.ratedS = value; }
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
                RotatingMachine r = (RotatingMachine)obj;
                return r.ratedS == this.ratedS;
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
            RotatingMachine io = new RotatingMachine(base.GlobalId);
            io.Measurements = this.Measurements;
            io.Mrid = this.Mrid;
            io.Name = this.Name;
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
                case ModelCode.ROTATINGMACHINE_RATEDS:
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
                case ModelCode.ROTATINGMACHINE_RATEDS:
                    prop.SetValue(this.ratedS);
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
                case ModelCode.ROTATINGMACHINE_RATEDS:
                    this.ratedS = property.AsFloat();
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