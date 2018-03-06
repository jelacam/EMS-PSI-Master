//-----------------------------------------------------------------------
// <copyright file="Measurement.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Meas
{
    using System.Collections.Generic;
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;

    /// <summary>
    /// Measurement class
    /// </summary>
    public class Measurement : IdentifiedObject
    {
        /// <summary>
        /// measurementType of measurement
        /// </summary>
        private string measurementType = string.Empty;

        /// <summary>
        /// unitSymbol of measurement
        /// </summary>
        private UnitSymbol unitSymbol;

        /// <summary>
        /// powerSystemResource of measurement
        /// </summary>
        private long powerSystemResource = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Measurement" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public Measurement(long globalId) : base(globalId)
        {
        }

        public Measurement():base()
        {

        }

        /// <summary>
        /// Gets or sets MeasurementType of the entity
        /// </summary>
        public string MeasurementType
        {
            get { return this.measurementType; }
            set { this.measurementType = value; }
        }

        /// <summary>
        /// Gets or sets UnitSymbol of the entity
        /// </summary>
        public UnitSymbol UnitSymbol
        {
            get { return this.unitSymbol; }
            set { this.unitSymbol = value; }
        }

        /// <summary>
        /// Gets or sets PowerSystemResource of the entity
        /// </summary>
        public long PowerSystemResource
        {
            get { return this.powerSystemResource; }
            set { this.powerSystemResource = value; }
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
                Measurement x = (Measurement)obj;
                return x.powerSystemResource == this.powerSystemResource && x.measurementType == this.measurementType && x.unitSymbol == this.unitSymbol;
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
            Measurement io = new Measurement();
            io.MeasurementType = this.MeasurementType;
            io.Mrid = this.Mrid;
            io.Name = this.Name;
            io.PowerSystemResource = this.PowerSystemResource;
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
                case ModelCode.MEASUREMENT_MEASUREMENTTYPE:
                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:
                case ModelCode.MEASUREMENT_UNITSYMBOL:
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
                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:
                    prop.SetValue(this.powerSystemResource);
                    break;

                case ModelCode.MEASUREMENT_MEASUREMENTTYPE:
                    prop.SetValue(this.measurementType);
                    break;

                case ModelCode.MEASUREMENT_UNITSYMBOL:
                    prop.SetValue((short)this.unitSymbol);
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
                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:
                    this.powerSystemResource = property.AsReference();
                    break;

                case ModelCode.MEASUREMENT_MEASUREMENTTYPE:
                    this.measurementType = property.AsString();
                    break;

                case ModelCode.MEASUREMENT_UNITSYMBOL:
                    this.unitSymbol = (UnitSymbol)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        /// <summary>
        /// Get references
        /// </summary>
        /// <param name="references">collection of references</param>
        /// <param name="refType">type of reference</param>
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (this.powerSystemResource != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE] = new List<long>();
                references[ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE].Add(this.powerSystemResource);
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation
    }
}
