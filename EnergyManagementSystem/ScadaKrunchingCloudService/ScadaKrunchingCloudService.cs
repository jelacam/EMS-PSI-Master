using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using EMS.Services.SCADACrunchingService;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using EMS.ServiceContracts;

namespace ScadaKrunchingCloudService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class ScadaKrunchingCloudService : StatelessService
    {
		private SCADACrunching scadaCR;

		public ScadaKrunchingCloudService(StatelessServiceContext context)
            : base(context)
        {
			scadaCR = new SCADACrunching();
		}

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
			return new List<ServiceInstanceListener>
			{
				new ServiceInstanceListener(context => this.CreateScadaCRListener(context), "ScadaCREndpoint"),
				new ServiceInstanceListener(context => this.CreateTransactionCRListener(context), "TransactionCREndpoint")
			};
		}

		private ICommunicationListener CreateScadaCRListener(StatelessServiceContext context)
		{
			var listener = new WcfCommunicationListener<IScadaCRContract>(
				listenerBinding: CloudCommon.Binding.CreateCustomNetTcp(),
				endpointResourceName: "ScadaCREndpoint",
				serviceContext: context,
				wcfServiceObject: scadaCR
			);

			return listener;
		}

		private ICommunicationListener CreateTransactionCRListener(StatelessServiceContext context)
		{
			var listener = new WcfCommunicationListener<ITransactionContract>(
				listenerBinding: CloudCommon.Binding.CreateCustomNetTcp(),
				endpointResourceName: "TransactionCREndpoint",
				serviceContext: context,
				wcfServiceObject: scadaCR
			);

			return listener;
		}

		/// <summary>
		/// This is the main entry point for your service instance.
		/// </summary>
		/// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
		protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                //ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
