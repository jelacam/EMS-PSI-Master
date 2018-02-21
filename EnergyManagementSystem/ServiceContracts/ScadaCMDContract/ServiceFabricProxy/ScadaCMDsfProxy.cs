using System;
using System.Collections.Generic;
using EMS.CommonMeasurement;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;

namespace EMS.ServiceContracts.ServiceFabricProxy
{
	public class ScadaCMDsfProxy : IScadaCMDContract
	{
		private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

		private WcfCommunicationClientFactory<IScadaCMDContract> factory;
		private ServicePartitionClient<WcfCommunicationClient<IScadaCMDContract>> proxy;

		public ScadaCMDsfProxy()
		{
			factory = new WcfCommunicationClientFactory<IScadaCMDContract>(
					clientBinding: CloudCommon.Binding.CreateCustomNetTcp(),
					servicePartitionResolver: resolver);
					//callback

			proxy = new ServicePartitionClient<WcfCommunicationClient<IScadaCMDContract>>(
					communicationClientFactory: factory,
					serviceUri: new Uri("fabric:/EMS/ScadaCommandingCloudService"),
					listenerName: "ScadaCMDEndpoint");
		}

		public bool SendDataToSimulator(List<MeasurementUnit> measurements)
		{
			return proxy.InvokeWithRetry(x => x.Channel.SendDataToSimulator(measurements));
		}
	}
}
