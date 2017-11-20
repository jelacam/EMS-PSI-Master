//-----------------------------------------------------------------------
// <copyright file="Measurement.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Meas
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;

    /// <summary>
    /// Measurement class
    /// </summary>
    public class Measurement : IdentifiedObject
    {
        private string measurementType = string.Empty;
        private UnitSymbol unitSymbol;
        private long powerSystemResource = 0;

        public Measurement(long globalId) : base(globalId)
        {

        }

        public string MeasurementType
        {
            get { return measurementType; }
            set { measurementType = value; }
        }

        public UnitSymbol UnitSymbol
        {
            get { return unitSymbol; }
            set { unitSymbol = value; }
        }

        public long PowerSystemResource
        {
            get { return powerSystemResource; }
            set { powerSystemResource = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Measurement x = (Measurement)obj;
                return (x.powerSystemResource == this.powerSystemResource && x.measurementType == this.measurementType && x.unitSymbol == this.unitSymbol);
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
                case ModelCode.MEASUREMENT_MEASUREMENTTYPE:
                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:
                case ModelCode.MEASUREMENT_UNITSYMBOL:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {

                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:
                    prop.SetValue(powerSystemResource);
                    break;

                case ModelCode.MEASUREMENT_MEASUREMENTTYPE:
                    prop.SetValue(measurementType);
                    break;

                case ModelCode.MEASUREMENT_UNITSYMBOL:
                    prop.SetValue((short)unitSymbol);
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
                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:
                    powerSystemResource = property.AsReference();
                    break;

                case ModelCode.MEASUREMENT_MEASUREMENTTYPE:
                    measurementType = property.AsString();
                    break;

                case ModelCode.MEASUREMENT_UNITSYMBOL:
                    unitSymbol = (UnitSymbol)property.AsEnum();
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
            if (powerSystemResource != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE] = new List<long>();
                references[ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE].Add(powerSystemResource);
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation

    }
}
