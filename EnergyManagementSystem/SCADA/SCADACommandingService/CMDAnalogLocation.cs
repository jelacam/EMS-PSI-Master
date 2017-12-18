using EMS.Services.NetworkModelService.DataModel.Meas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.SCADACommandingService
{
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
