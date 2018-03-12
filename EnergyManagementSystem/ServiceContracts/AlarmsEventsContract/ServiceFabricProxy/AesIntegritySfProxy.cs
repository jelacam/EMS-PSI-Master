using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.CommonMeasurement;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using CloudCommon;

namespace EMS.ServiceContracts.ServiceFabricProxy
{
    public class AesIntegritySfProxy : IAesIntegirtyContract
    {
        private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

        private WcfCommunicationClientFactory<IAesIntegirtyContract> factory;
        private ServicePartitionClient<WcfCommunicationClient<IAesIntegirtyContract>> proxy;

        public AesIntegritySfProxy()
        {
            factory = new WcfCommunicationClientFactory<IAesIntegirtyContract>(
                    clientBinding: Binding.CreateCustomNetTcp(),
                    servicePartitionResolver: resolver);

            proxy = new ServicePartitionClient<WcfCommunicationClient<IAesIntegirtyContract>>(
                    communicationClientFactory: factory,
                    serviceUri: new Uri("fabric:/EMS/AlarmsEventsCloudService"),
                    listenerName: "AlarmsEventsIntegrityEndpoint",
                    partitionKey: ServicePartitionKey.Singleton);
        }

        public List<AlarmHelper> InitiateIntegrityUpdate()
        {
            return proxy.InvokeWithRetry(x => x.Channel.InitiateIntegrityUpdate());
        }
    }
}
