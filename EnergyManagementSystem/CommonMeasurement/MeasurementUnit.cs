//-----------------------------------------------------------------------
// <copyright file="MeasurementUnit.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.CommonMeasurement
{
    using System;

    /// <summary>
    /// MeasurementUnit class
    /// </summary>
    public class MeasurementUnit
    {
        /// <summary>
        /// gid for MeasurementUnit
        /// </summary>
        private long gid;

        /// <summary>
        /// currentValue for MeasurementUnit
        /// </summary>
        private float currentValue;

        /// <summary>
        /// minValue for MeasurementUnit
        /// </summary>
        private float minValue;

        /// <summary>
        /// maxValue for MeasurementUnit
        /// </summary>
        private float maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasurementUnit" /> class
        /// </summary>
        public MeasurementUnit()
        {
        }

        /// <summary>
        /// Gets or sets Gid of the entity
        /// </summary>
        public long Gid
        {
            get
            {
                return this.gid;
            }

            set
            {
                this.gid = value;
            }
        }

        /// <summary>
        /// Gets or sets CurrentValue of the entity
        /// </summary>
        public float CurrentValue
        {
            get
            {
                return this.currentValue;
            }

            set
            {
                this.currentValue = value;
            }
        }

        /// <summary>
        /// Gets or sets MinValue of the entity
        /// </summary>
        public float MinValue
        {
            get
            {
                return this.minValue;
            }

            set
            {
                this.minValue = value;
            }
        }

        /// <summary>
        /// Gets or sets MaxValue of the entity
        /// </summary>
        public float MaxValue
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                this.maxValue = value;
            }
        }
    }
}