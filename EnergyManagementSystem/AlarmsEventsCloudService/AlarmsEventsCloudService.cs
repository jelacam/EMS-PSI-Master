using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudCommon;
using EMS.ServiceContracts;
using EMS.Services.AlarmsEventsService;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace AlarmsEventsCloudService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class AlarmsEventsCloudService : StatefulService
    {
        private AlarmsEvents alarmsEvents;

        public AlarmsEventsCloudService(StatefulServiceContext context)
            : base(context)
        {
            alarmsEvents = new AlarmsEvents();
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
            return new List<ServiceReplicaListener>() {
                new ServiceReplicaListener(context => this.CreateAlarmEventsListener(context), "AlarmsEventsEndpoint"),
                new ServiceReplicaListener(context => this.CreateAlarmsEventsIntegrityListener(context), "AlarmsEventsIntegrityEndpoint")
            };
        }

        #region Listeners

        /// <summary>
        /// Listener for adding new Alarms for SCADA KR
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ICommunicationListener CreateAlarmEventsListener(StatefulServiceContext context)
        {
            var listener = new WcfCommunicationListener<IAlarmsEventsContract>(
                listenerBinding: Binding.CreateCustomNetTcp(),
                endpointResourceName: "AlarmsEventsEndpoint",
                serviceContext: context,
                wcfServiceObject: alarmsEvents
            );

            return listener;
        }

        /// <summary>
        /// Listener for getting all alarms from AES (UI initiate this call)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ICommunicationListener CreateAlarmsEventsIntegrityListener(StatefulServiceContext context)
        {
            var listener = new WcfCommunicationListener<IAesIntegirtyContract>(
                listenerBinding: Binding.CreateCustomNetTcp(),
                endpointResourceName: "AlarmsEventsIntegrityEndpoint",
                serviceContext: context,
                wcfServiceObject: alarmsEvents
            );

            return listener;
        }

        #endregion Listeners

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            alarmsEvents.Instantiate(this.StateManager);

            ServiceEventSource.Current.ServiceMessage(this.Context, "AES instantiation finished.");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

               
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
