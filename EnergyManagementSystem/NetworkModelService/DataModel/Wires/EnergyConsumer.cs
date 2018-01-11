//-----------------------------------------------------------------------
// <copyright file="EnergyConsumer.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;

    /// <summary>
    /// EnergyConsumer class
    /// </summary>
    public class EnergyConsumer : ConductingEquipment
    {
        /// <summary>
        /// pFixed of energy consumer
        /// </summary>
        private float pFixed;

        /// <summary>
        /// pFixedPct of energy consumer
        /// </summary>
        private float pFixedPct;

        /// <summary>
        /// qFixed of energy consumer
        /// </summary>
        private float qFixed;

        /// <summary>
        /// qFixedPct of energy consumer
        /// </summary>
        private float qFixedPct;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnergyConsumer" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public EnergyConsumer(long globalId) : base(globalId)
        {
        }

        /// <summary>
        /// Gets or sets PFixed of the entity
        /// </summary>
        public float PFixed
        {
            get { return this.pFixed; }
            set { this.pFixed = value; }
        }

        /// <summary>
        /// Gets or sets PFixedPct of the entity
        /// </summary>
        public float PFixedPct
        {
            get { return this.pFixedPct; }
            set { this.pFixedPct = value; }
        }

        /// <summary>
        /// Gets or sets QFixed of the entity
        /// </summary>
        public float QFixed
        {
            get { return this.qFixed; }
            set { this.qFixed = value; }
        }

        /// <summary>
        /// Gets or sets QFixedPct of the entity
        /// </summary>
        public float QFixedPct
        {
            get { return this.qFixedPct; }
            set { this.qFixedPct = value; }
        }

        /// <summary>
        /// Chechs are the entities equals
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <returns>indicator of equality</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                EnergyConsumer x = (EnergyConsumer)obj;
                return x.pFixed == this.pFixed && x.pFixedPct == this.pFixedPct && x.qFixed == this.qFixed && x.qFixedPct == this.qFixedPct;
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
                case ModelCode.ENERGYCONSUMER_PFIXED:
                case ModelCode.ENERGYCONSUMER_PFIXEDPCT:
                case ModelCode.ENERGYCONSUMER_QFIXED:
                case ModelCode.ENERGYCONSUMER_QFIXEDPCT:
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
                case ModelCode.ENERGYCONSUMER_PFIXED:
                    prop.SetValue(this.pFixed);
                    break;

                case ModelCode.ENERGYCONSUMER_PFIXEDPCT:
                    prop.SetValue(this.pFixedPct);
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXED:
                    prop.SetValue(this.qFixed);
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXEDPCT:
                    prop.SetValue(this.qFixedPct);
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
                case ModelCode.ENERGYCONSUMER_PFIXED:
                    this.pFixed = property.AsFloat();
                    break;

                case ModelCode.ENERGYCONSUMER_PFIXEDPCT:
                    this.pFixedPct = property.AsFloat();
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXED:
                    this.qFixed = property.AsFloat();
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXEDPCT:
                    this.qFixedPct = property.AsFloat();
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
