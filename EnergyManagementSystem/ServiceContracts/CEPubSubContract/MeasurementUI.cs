using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    public class MeasurementUI
    {
        public long Gid { get; set; }

        public float MeasurementValue { get; set; }

        public string AlarmType { get; set; }

        public float TimeStamp { get; set; }
    }
}
