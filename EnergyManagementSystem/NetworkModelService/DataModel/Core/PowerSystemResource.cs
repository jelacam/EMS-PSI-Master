//-----------------------------------------------------------------------
// <copyright file="PowerSystemResource.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Core
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// PowerSystemResource class
    /// </summary>
    public class PowerSystemResource : IdentifiedObject
    {
        /// <summary>
        /// List of Measurements
        /// </summary>
        private List<long> measurements = new List<long>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerSystemResource" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public PowerSystemResource(long globalId) : base(globalId)
        {
        }

        /// <summary>
        /// Gets or sets list of measurements of the entity
        /// </summary>
        public List<long> Measurements
        {
            get { return this.measurements; }
            set { this.measurements = value; }
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
                PowerSystemResource x = (PowerSystemResource)obj;
                return CompareHelper.CompareLists(x.measurements, this.measurements, true);
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
                case ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS:
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
                case ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS:
                    prop.SetValue(this.measurements);
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
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        /// <summary>
        /// Gets isReferenced of the entity
        /// </summary>
        public override bool IsReferenced
        {
            get
            {
                return this.measurements.Count > 0 || base.IsReferenced;
            }
        }

        /// <summary>
        /// Gets reference
        /// </summary>
        /// <param name="references">collection of references</param>
        /// <param name="refType">type of reference</param>
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (this.measurements != null && this.measurements.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS] = this.measurements.GetRange(0, this.measurements.Count);
            }

            base.GetReferences(references, refType);
        }

        /// <summary>
        /// Adds reference
        /// </summary>
        /// <param name="referenceId">referenceId of object</param>
        /// <param name="globalId">globalId of object</param>
        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:
                    this.measurements.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        /// <summary>
        /// Removes reference
        /// </summary>
        /// <param name="referenceId">referenceId of object</param>
        /// <param name="globalId">globalId of object</param>
        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE:

                    if (this.measurements.Contains(globalId))
                    {
                        this.measurements.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation
    }
}