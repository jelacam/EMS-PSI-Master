using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using EMS.ServiceContracts;
using CloudCommon;
using EMS.Services.CalculationEngineService;

namespace CalculationEngineCloudService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class CalculationEngineCloudService : StatefulService
    {
        private CrToCe crToCe;
        private CeToUI ceToUI;
        private CalculationEngine ce;

        public CalculationEngineCloudService(StatefulServiceContext context)
            : base(context)
        {
            ce = new CalculationEngine(this.Context);
            crToCe = new CrToCe();
            ceToUI = new CeToUI();
            CrToCe.CalculationEngine = ce;
            CeToUI.CalculationEngine = ce;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new List<ServiceReplicaListener>
            {
                new ServiceReplicaListener(context => this.CreateCalculationEngineListener(context), "CalculationEngineEndpoint"),
                new ServiceReplicaListener(context => this.CreateCalculationEngineUIListener(context), "CalculationEngineUIEndpoint"),
                new ServiceReplicaListener(context => this.CreateCalculationEngineTransactionListener(context), "CalculationEngineTransactionEndpoint"),
                new ServiceReplicaListener(context => this.CreateChooseOptimizationListener(context), "CeChooseOptimizationEndpoint")
            };
        }

        private ICommunicationListener CreateCalculationEngineListener(StatefulServiceContext context)
        {
            var listener = new WcfCommunicationListener<ICalculationEngineContract>(
                           listenerBinding: Binding.CreateCustomNetTcp(),
                           endpointResourceName: "CalculationEngineEndpoint",
                           serviceContext: context,
                           wcfServiceObject: crToCe
            );
            ServiceEventSource.Current.ServiceMessage(context, "Created listener for CalculationEngineEndpoint");
            return listener;
        }

        private ICommunicationListener CreateCalculationEngineUIListener(StatefulServiceContext context)
        {
            var listener = new WcfCommunicationListener<ICalculationEngineUIContract>(
                           listenerBinding: Binding.CreateCustomNetTcp(),
                           endpointResourceName: "CalculationEngineUIEndpoint",
                           serviceContext: context,
                           wcfServiceObject: ceToUI
            );
            ServiceEventSource.Current.ServiceMessage(context, "Created listener for CalculationEngineHistoryDataEndpoint");
            return listener;
        }

        private ICommunicationListener CreateCalculationEngineTransactionListener(StatefulServiceContext context)
        {
            var listener = new WcfCommunicationListener<ITransactionContract>(
                           listenerBinding: Binding.CreateCustomNetTcp(),
                           endpointResourceName: "CalculationEngineTransactionEndpoint",
                           serviceContext: context,
                           wcfServiceObject: ce
            );
            ServiceEventSource.Current.ServiceMessage(context, "Created listener for CalculationEngineListenerEndpoint");
            return listener;
        }

        private ICommunicationListener CreateChooseOptimizationListener(StatefulServiceContext context)
        {
            var listener = new WcfCommunicationListener<IOptimizationAlgorithmContract>(
                           listenerBinding: Binding.CreateCustomNetTcp(),
                           endpointResourceName: "CeChooseOptimizationEndpoint",
                           serviceContext: context,
                           wcfServiceObject: ce
            );
            return listener;
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            #region CalculationEngine instantiation

            bool integrityState = ce.InitiateIntegrityUpdate();

            if (!integrityState)
            {
                ServiceEventSource.Current.ServiceMessage(this.Context, "CalculationEngine integrity update failed");
            }
            else
            {
                ServiceEventSource.Current.ServiceMessage(this.Context, "CalculationEngine integrity update succeeded.");
            }

            #endregion CalculationEngine instantiation

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}