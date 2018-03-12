using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.AlarmsEventsService
{
    [DataContract]
    public sealed class AlarmsData
    {
        private static readonly IEnumerable<AlarmHelper> NoAlarms = ImmutableList<AlarmHelper>.Empty;

        [DataMember]
        public IEnumerable<AlarmHelper> Alarms { get; private set; }

        [OnDeserialized]
        private void OnDeserialize(StreamingContext context)
        {
            Alarms = Alarms.ToImmutableList();
        }

        public void AddAlarm(AlarmHelper alarm)
        {
            ((ImmutableList<AlarmHelper>)((Alarms == null) ? NoAlarms : new List<AlarmHelper>())).Add(alarm);
        }

        public void AddAlarms(List<AlarmHelper> alarms)
        {
            
            ((ImmutableList<AlarmHelper>)((Alarms == null) ? NoAlarms : new List<AlarmHelper>())).AddRange(alarms);
        }
    }
}
