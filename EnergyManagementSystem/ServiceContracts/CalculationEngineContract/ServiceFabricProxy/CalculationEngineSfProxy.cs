using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Threading.Tasks;
using EMS.CommonMeasurement;
using CloudCommon;

namespace EMS.ServiceContracts.ServiceFabricProxy
{
    public class CalculationEngineSfProxy : ICalculationEngineContract
    {

        private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

        private WcfCommunicationClientFactory<ICalculationEngineContract> factory;
        private ServicePartitionClient<WcfCommunicationClient<ICalculationEngineContract>> proxy;

        public CalculationEngineSfProxy()
        {
            factory = new WcfCommunicationClientFactory<ICalculationEngineContract>(
                    clientBinding: Binding.CreateCustomNetTcp(),
                    servicePartitionResolver: resolver);

            proxy = new ServicePartitionClient<WcfCommunicationClient<ICalculationEngineContract>>(
                    communicationClientFactory: factory,
                    serviceUri: new Uri("fabric:/EMS/CalculationEngineCloudService"),
                    listenerName: "CalculationEngineEndpoint",
                    partitionKey: new ServicePartitionKey(0));
        }

        public bool OptimisationAlgorithm(List<MeasurementUnit> measEnergyConsumers, List<MeasurementUnit> measGenerators, float windSpeed, float sunlight)
        {
            return proxy.InvokeWithRetry(x => x.Channel.OptimisationAlgorithm(measEnergyConsumers, measGenerators, windSpeed, sunlight));
        }
    }
}
