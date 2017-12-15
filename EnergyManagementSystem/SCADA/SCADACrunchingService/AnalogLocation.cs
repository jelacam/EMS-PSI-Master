//-----------------------------------------------------------------------
// <copyright file="AnalogLocation.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACrunchingService
{
	using EMS.Services.NetworkModelService.DataModel.Meas;

	/// <summary>
	/// AnalogLocation class
	/// </summary>
	public class AnalogLocation
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
	}
}
