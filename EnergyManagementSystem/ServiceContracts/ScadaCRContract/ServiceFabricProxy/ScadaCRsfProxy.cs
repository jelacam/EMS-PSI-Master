using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;

namespace EMS.ServiceContracts.ServiceFabricProxy
{
	public class ScadaCRsfProxy:IScadaCRContract
	{
		private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

		private WcfCommunicationClientFactory<IScadaCRContract> factory;
		private ServicePartitionClient<WcfCommunicationClient<IScadaCRContract>> proxy;

		public ScadaCRsfProxy()
		{
			factory = new WcfCommunicationClientFactory<IScadaCRContract>(
					clientBinding: CloudCommon.Binding.CreateCustomNetTcp(),
					servicePartitionResolver: resolver);
			//callback

			proxy = new ServicePartitionClient<WcfCommunicationClient<IScadaCRContract>>(
					communicationClientFactory: factory,
					serviceUri: new Uri("fabric:/EMS/ScadaKrunchingCloudService"),
					listenerName: "ScadaCREndpoint");
		}

		public bool SendValues(byte[] value)
		{
			return proxy.InvokeWithRetry(x => x.Channel.SendValues(value));
		}
	}
}
