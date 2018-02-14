using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    [ServiceContract]
    public interface IAesPublishContract
    {
        [OperationContract]
        void PublishStateChange(AlarmHelper alarm);

        [OperationContract]
        void PublishAlarmsEvents(AlarmHelper alarm, PublishingStatus status);
    }
}