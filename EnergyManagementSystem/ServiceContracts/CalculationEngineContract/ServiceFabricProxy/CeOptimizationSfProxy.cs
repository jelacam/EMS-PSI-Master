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
    public class CeOptimizationSfProxy : IOptimizationAlgorithmContract
    {
        private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

        private WcfCommunicationClientFactory<IOptimizationAlgorithmContract> factory;
        private ServicePartitionClient<WcfCommunicationClient<IOptimizationAlgorithmContract>> proxy;

        public CeOptimizationSfProxy()
        {
            factory = new WcfCommunicationClientFactory<IOptimizationAlgorithmContract>(
                    clientBinding: Binding.CreateCustomNetTcp(),
                    servicePartitionResolver: resolver);

            proxy = new ServicePartitionClient<WcfCommunicationClient<IOptimizationAlgorithmContract>>(
                    communicationClientFactory: factory,
                    serviceUri: new Uri("fabric:/EMS/CalculationEngineCloudService"),
                    listenerName: "CeChooseOptimizationEndpoint",
                    partitionKey: new ServicePartitionKey("Measurements"));
        }

        public bool ChooseOptimization(OptimizationType optimizationType)
        {
            return proxy.InvokeWithRetry(x => x.Channel.ChooseOptimization(optimizationType));
        }
    }
}