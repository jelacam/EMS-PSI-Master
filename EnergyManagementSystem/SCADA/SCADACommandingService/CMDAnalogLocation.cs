//-----------------------------------------------------------------------
// <copyright file="CMDAnalogLocation.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.Services.SCADACommandingService
{
    using EMS.Services.NetworkModelService.DataModel.Meas;

    /// <summary>
    /// CMDAnalogLocation class
    /// </summary>
    public class CMDAnalogLocation
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="CMDAnalogLocation" /> class
		/// </summary>
        public CMDAnalogLocation()
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
    }
}
