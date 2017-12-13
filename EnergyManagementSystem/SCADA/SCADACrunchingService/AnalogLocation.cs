using EMS.Services.NetworkModelService.DataModel.Meas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.SCADACrunchingService
{
	public class AnalogLocation
	{
		public Analog Analog { get; set; }

		public int StartAddress { get; set; }

		public int Length { get; set; }

	}
}
