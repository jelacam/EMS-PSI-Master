using CloudCommon;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts.ServiceFabricProxy
{
    public class CalculationEngineHistorySfProxy : ICalculationEngineUIContract
    {
        private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

        private WcfCommunicationClientFactory<ICalculationEngineUIContract> factory;
        private ServicePartitionClient<WcfCommunicationClient<ICalculationEngineUIContract>> proxy;

        public CalculationEngineHistorySfProxy()
        {
            factory = new WcfCommunicationClientFactory<ICalculationEngineUIContract>(
                    clientBinding: Binding.CreateCustomNetTcp(),
                    servicePartitionResolver: resolver);

            proxy = new ServicePartitionClient<WcfCommunicationClient<ICalculationEngineUIContract>>(
                    communicationClientFactory: factory,
                    serviceUri: new Uri("fabric:/EMS/CalculationEngineCloudService"),
                    listenerName: "CalculationEngineUIEndpoint",
                    partitionKey: new ServicePartitionKey("HistoryData"));
        }

        public List<Tuple<double, double, DateTime>> GetCO2Emission(DateTime startTime, DateTime endTime)
        {
            return proxy.InvokeWithRetry(x => x.Channel.GetCO2Emission(startTime, endTime));
        }

        public List<Tuple<double, DateTime>> GetHistoryMeasurements(long gid, DateTime startTime, DateTime endTime)
        {
            return proxy.InvokeWithRetry(x => x.Channel.GetHistoryMeasurements(gid, startTime, endTime));
        }

        public List<Tuple<double, DateTime>> GetTotalProduction(DateTime startTime, DateTime endTime)
        {
            return proxy.InvokeWithRetry(x => x.Channel.GetTotalProduction(startTime, endTime));
        }

        public List<Tuple<double, double, double, double, double>> ReadIndividualFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            return proxy.InvokeWithRetry(x => x.Channel.ReadIndividualFarmProductionDataFromDb(startTime, endTime));
        }

        public List<Tuple<double, double, double>> ReadWindFarmSavingDataFromDb(DateTime startTime, DateTime endTime)
        {
            return proxy.InvokeWithRetry(x => x.Channel.ReadWindFarmSavingDataFromDb(startTime, endTime));
        }
    }
}