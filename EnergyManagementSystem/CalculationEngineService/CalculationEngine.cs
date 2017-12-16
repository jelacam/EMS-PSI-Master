//-----------------------------------------------------------------------
// <copyright file="CalculationEngine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
	using System;
	using System.Collections.Generic;
	using CommonMeasurement;
	
	/// <summary>
	/// Class for CalculationEngine
	/// </summary>
	public class CalculationEngine
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CalculationEngine" /> class
		/// </summary>
		public CalculationEngine()
		{
		}

		/// <summary>
		/// Optimization algorithm
		/// </summary>
		/// <param name="measurements">list of measurements which should be optimized</param>
		/// <returns>returns true if optimization was successful</returns>
		public bool Optimize(List<MeasurementUnit> measurements)
		{
			bool result = false;
			if (measurements != null)
			{
				if (measurements.Count > 0)
				{
					Console.WriteLine("CE: Optimize");
					for (int i = 0; i < measurements.Count; i++)
					{
						measurements[i].CurrentValue = measurements[i].CurrentValue * 2;
						Console.WriteLine("gid: {0} value: {1}", measurements[i].Gid, measurements[i].CurrentValue);
					}

					result= true;
				}
			}

			return result;
        }
    }
}