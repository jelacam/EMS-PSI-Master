using CloudCommon;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    public class WebEmsSfProxy : IWebEMSContract
    {
        private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

        private WcfCommunicationClientFactory<IWebEMSContract> factory;
        private ServicePartitionClient<WcfCommunicationClient<IWebEMSContract>> proxy;

        public WebEmsSfProxy()
        {
            factory = new WcfCommunicationClientFactory<IWebEMSContract>(
                    clientBinding: Binding.CreateCustomNetTcp(),
                    servicePartitionResolver: resolver);

            proxy = new ServicePartitionClient<WcfCommunicationClient<IWebEMSContract>>(
                    communicationClientFactory: factory,
                    serviceUri: new Uri("fabric:/EMS/WebEMS"),
                    listenerName: "PublishEndpoint");
        }

        public void PublishOptimizationResults(List<MeasurementUI> result)
        {
            proxy.InvokeWithRetry(x => x.Channel.PublishOptimizationResults(result));
        }
    }
}
