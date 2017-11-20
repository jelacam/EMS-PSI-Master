//-----------------------------------------------------------------------
// <copyright file="EnergyConsumer.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;

    /// <summary>
    /// EnergyConsumer class
    /// </summary>
    public class EnergyConsumer : ConductingEquipment
    {
        private float pFixed;
        private float pFixedPct;
        private float qFixed;
        private float qFixedPct;

        public EnergyConsumer(long globalId) : base(globalId)
        {

        }

        public float PFixed
        {
            get { return pFixed; }
            set { pFixed = value; }
        }

        public float PFixedPct
        {
            get { return pFixedPct; }
            set { pFixedPct = value; }
        }

        public float QFixed
        {
            get { return qFixed; }
            set { qFixed = value; }
        }

        public float QFixedPct
        {
            get { return qFixedPct; }
            set { qFixedPct = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                EnergyConsumer x = (EnergyConsumer)obj;
                return (x.pFixed == this.pFixed && x.pFixedPct == this.pFixedPct && x.qFixed == this.qFixed && x.qFixedPct == this.qFixedPct);
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

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {

                case ModelCode.ENERGYCONSUMER_PFIXED:
                    prop.SetValue(pFixed);
                    break;

                case ModelCode.ENERGYCONSUMER_PFIXEDPCT:
                    prop.SetValue(pFixedPct);
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXED:
                    prop.SetValue(qFixed);
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXEDPCT:
                    prop.SetValue(qFixedPct);
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
                case ModelCode.ENERGYCONSUMER_PFIXED:
                    pFixed = property.AsFloat();
                    break;

                case ModelCode.ENERGYCONSUMER_PFIXEDPCT:
                    pFixedPct = property.AsFloat();
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXED:
                    qFixed = property.AsFloat();
                    break;

                case ModelCode.ENERGYCONSUMER_QFIXEDPCT:
                    qFixedPct = property.AsFloat();
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
