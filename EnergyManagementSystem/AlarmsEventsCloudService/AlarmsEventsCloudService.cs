using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudCommon;
using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using EMS.Services.AlarmsEventsService;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace AlarmsEventsCloudService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class AlarmsEventsCloudService : StatelessService, IAesIntegrityAsyncContract
    {
        private AlarmsEvents aEvents;

        public AlarmsEventsCloudService(StatelessServiceContext context)
            : base(context)
        {
            aEvents = new AlarmsEvents();
        }

        public Task<List<AlarmHelper>> InitiateIntegrityUpdate()
        {
            return Task.FromResult(aEvents.InitiateIntegrityUpdate());
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>
            {
                new ServiceInstanceListener(context => this.CreateAlarmEventsListener(context), "AlarmsEventsEndpoint"),
                new ServiceInstanceListener(context => this.CreateAlarmsEventsIntegrityListener(context), "AlarmsEventsIntegrityEndpoint"),
                new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context), "AlarmsEventsIntegrityAsyncEndpoint")

            };
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

        #region Listeners

        /// <summary>
        /// Listener for adding new Alarms for SCADA KR
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ICommunicationListener CreateAlarmEventsListener(StatelessServiceContext context)
        {
            var listener = new WcfCommunicationListener<IAlarmsEventsContract>(
                listenerBinding: Binding.CreateCustomNetTcp(),
                endpointResourceName: "AlarmsEventsEndpoint",
                serviceContext: context,
                wcfServiceObject: aEvents
            );

            return listener;
        }

        /// <summary>
        /// Listener for getting all alarms from AES (UI initiate this call)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ICommunicationListener CreateAlarmsEventsIntegrityListener(StatelessServiceContext context)
        {
            var listener = new WcfCommunicationListener<IAesIntegirtyContract>(
                listenerBinding: Binding.CreateCustomNetTcp(),
                endpointResourceName: "AlarmsEventsIntegrityEndpoint",
                serviceContext: context,
                wcfServiceObject: aEvents
            );

            return listener;
        }

        #endregion Listeners
    }
}