using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CommonMeasurement
{
    public class ScadaConfiguration
    {
        private List<AnalogLocation> analogsList = new List<AnalogLocation>();

        public List<AnalogLocation> AnalogsList
        {
            get { return analogsList; }
            set { analogsList = value; }
        }

        public ScadaConfiguration()
        {

        }

    }
}
