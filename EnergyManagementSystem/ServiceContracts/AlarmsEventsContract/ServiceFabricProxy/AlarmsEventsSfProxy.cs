using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCommon;
using EMS.CommonMeasurement;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;

namespace EMS.ServiceContracts.ServiceFabricProxy
{
    public class AlarmsEventsSfProxy : IAlarmsEventsContract
    {
        private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

        private WcfCommunicationClientFactory<IAlarmsEventsContract> factory;
        private ServicePartitionClient<WcfCommunicationClient<IAlarmsEventsContract>> proxy;

        public AlarmsEventsSfProxy()
        {
            factory = new WcfCommunicationClientFactory<IAlarmsEventsContract>(
                    clientBinding: Binding.CreateCustomNetTcp(),
                    servicePartitionResolver: resolver);

            proxy = new ServicePartitionClient<WcfCommunicationClient<IAlarmsEventsContract>>(
                    communicationClientFactory: factory,
                    serviceUri: new Uri("fabric:/EMS/AlarmsEventsCloudService"),
                    listenerName: "AlarmsEventsEndpoint");
        }

        public void AddAlarm(AlarmHelper alarm)
        {
            proxy.InvokeWithRetry(x => x.Channel.AddAlarm(alarm));
        }

        public void UpdateStatus(AnalogLocation analogLoc, State state)
        {
            proxy.InvokeWithRetry(x => x.Channel.UpdateStatus(analogLoc, state));
        }
    }
}