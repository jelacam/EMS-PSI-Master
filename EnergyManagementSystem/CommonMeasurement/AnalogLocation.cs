
// <copyright file="CMDAnalogLocation.cs" company="EMSTeam">
//     Company copyright tag.
// </copyright>

namespace EMS.CommonMeasurement
{
    using EMS.Services.NetworkModelService.DataModel.Meas;
    using System;

    /// <summary>
    /// CMDAnalogLocation class
    /// </summary>
    public class AnalogLocation : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogLocation" /> class
        /// </summary>
        public AnalogLocation()
        {
        }

        /// <summary>
        /// Gets or sets Analog of the entity
        /// </summary>
        public Analog Analog { get; set; }

        /// <summary>
        /// Gets or sets StartAddress of the entity
        /// </summary>
        public int StartAddress { get; set; }

        /// <summary>
        /// Gets or sets Length of the entity
        /// </summary>
        public int Length { get; set; }

        public object Clone()
        {
            AnalogLocation alocation = new AnalogLocation();
            alocation.Analog = Analog;
            alocation.StartAddress = StartAddress;
            alocation.Length = Length;

            return alocation;
        }
    }
}