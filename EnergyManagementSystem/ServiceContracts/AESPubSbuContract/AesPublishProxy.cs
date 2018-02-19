using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EMS.CommonMeasurement;

namespace EMS.ServiceContracts
{
    /// <summary>
    /// net.tcp://host:20030/AlarmsEvents/PublishService
    /// </summary>
    public class AesPublishProxy : IAesPublishContract
    {
        public static IAesPublishContract proxy;
        public static ChannelFactory<IAesPublishContract> factory;

        public static IAesPublishContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    factory = new ChannelFactory<IAesPublishContract>("AesPublishEndpoint");
                    proxy = factory.CreateChannel();
                }
                return proxy;
            }
            set
            {
                if (proxy == null)
                {
                    proxy = value;
                }
            }
        }

        public void PublishAlarmsEvents(AlarmHelper alarm, PublishingStatus status)
        {
            proxy.PublishAlarmsEvents(alarm, status);
        }

        public void PublishStateChange(AlarmHelper alarm)
        {
            proxy.PublishStateChange(alarm);
        }
    }
}