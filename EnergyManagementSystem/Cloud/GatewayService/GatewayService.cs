using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using CloudCommon;
using EMS.Common;
using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using GatewayService.PubSub;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace GatewayService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class GatewayService : StatefulService, IGatewayServiceContracts
    {
        public GatewayService(StatefulServiceContext context)
            : base(context)
        { }

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
                new ServiceReplicaListener(context => this.CreateAESSubscribeListener(context), "AESSubscribeEndpoint"),
                new ServiceReplicaListener(context => this.CreateAESPublishListener(context), "AESPublishEndpoint"),
                new ServiceReplicaListener(context => this.CreateAESIntegirityUpdateListener(context), "AESIntegrityUpdate")
            };
        }

        #region Listeners

        #region AlarmsEventsService listeners

        /// <summary>
        ///  Gateway listener for Alarms Events Service
        ///  Address: "net.tcp://host:20030/AlarmsEvents/SubscribeService"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ICommunicationListener CreateAESSubscribeListener(StatefulServiceContext context)
        {
            string host = context.NodeContext.IPAddressOrFQDN;

            var endpointConfig = context.CodePackageActivationContext.GetEndpoint("AESSubscribeEndpoint");
            int port = endpointConfig.Port;
            var scheme = endpointConfig.UriScheme.ToString();
            var pathSufix = endpointConfig.PathSuffix.ToString();

            string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/AlarmsEvents/{3}", scheme, host, port, pathSufix);

            var listener = new WcfCommunicationListener<IAesSubscribeContract>(
                           listenerBinding: Binding.CreateCustomNetTcp(),
                           address: new EndpointAddress(uri),
                           serviceContext: context,
                           wcfServiceObject: this
            );

            return listener;
        }

        /// <summary>
        /// Gateway listener for Alarms Events Service
        ///  Address: "net.tcp://host:20030/AlarmsEvents/PublishService"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ICommunicationListener CreateAESPublishListener(StatefulServiceContext context)
        {
            string host = context.NodeContext.IPAddressOrFQDN;

            var endpointConfig = context.CodePackageActivationContext.GetEndpoint("AESPublishEndpoint");
            int port = endpointConfig.Port;
            var scheme = endpointConfig.UriScheme.ToString();
            var pathSufix = endpointConfig.PathSuffix.ToString();

            string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/AlarmsEvents/{3}", scheme, host, port, pathSufix);

            var listener = new WcfCommunicationListener<IAesPublishContract>(
                            listenerBinding: Binding.CreateCustomNetTcp(),
                            address: new EndpointAddress(uri),
                            serviceContext: context,
                            wcfServiceObject: this
            );

            return listener;
        }

        /// <summary>
        /// Gateway listener for Alarm Events Service
        /// Address: "net.tcp://localhost:20023/AlarmsEvents/IntegrityUpdate"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ICommunicationListener CreateAESIntegirityUpdateListener(StatefulServiceContext context)
        {
            string host = context.NodeContext.IPAddressOrFQDN;

            var endpointConfig = context.CodePackageActivationContext.GetEndpoint("AESIntegrityUpdate");
            int port = endpointConfig.Port;
            var scheme = endpointConfig.UriScheme.ToString();
            var pathSufix = endpointConfig.PathSuffix.ToString();

            string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/AlarmsEvents/{3}", scheme, host, port, pathSufix);

            var listener = new WcfCommunicationListener<IAesIntegirtyContract>(
                            listenerBinding: Binding.CreateCustomNetTcp(),
                            address: new EndpointAddress(uri),
                            serviceContext: context,
                            wcfServiceObject: this
            );

            return listener;
        }

        #endregion AlarmsEventsService listeners

        #endregion Listeners

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            #region AlarmsEventsService PubSub initialization

            alarmEventHandler = new AlarmEventHandler(AlarmsEventsHandler);
            AlarmEvent += alarmEventHandler;

            alarmUpdateHandler = new AlarmUpdateHandler(AlarmUpdateEventsHandler);
            AlarmUpdate += alarmUpdateHandler;

            #endregion AlarmsEventsService PubSub initialization

            // TODO: Replace the following sample code with your own logic
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        #region IAesSubscribeContract implementation

        public delegate void AlarmEventHandler(object sender, AlarmsEventsEventArgs e);

        public delegate void AlarmUpdateHandler(object sender, AlarmUpdateEventArgs e);

        public static event AlarmEventHandler AlarmEvent;

        public static event AlarmUpdateHandler AlarmUpdate;

        private IAesSubscribeCallbackContract callback = null;
        private AlarmEventHandler alarmEventHandler = null;
        private AlarmUpdateHandler alarmUpdateHandler = null;

        private static List<IAesSubscribeCallbackContract> clientsToPublish = new List<IAesSubscribeCallbackContract>(4);

        private object clientsLocker = new object();

        /// <summary>
        /// This event handler runs when a AlarmsEvents event is raised.
        /// The client's AlarmsEvents operation is invoked to provide notification about the new alarm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AlarmsEventsHandler(object sender, AlarmsEventsEventArgs e)
        {
            List<IAesSubscribeCallbackContract> faultetClients = new List<IAesSubscribeCallbackContract>(4);

            foreach (IAesSubscribeCallbackContract client in clientsToPublish)
            {
                if ((client as ICommunicationObject).State.Equals(CommunicationState.Opened))
                {
                    client.AlarmsEvents(e.Alarm);
                }
                else
                {
                    faultetClients.Add(client);
                }
            }

            lock (clientsLocker)
            {
                foreach (IAesSubscribeCallbackContract client in faultetClients)
                {
                    clientsToPublish.Remove(client);
                }
            }
        }

        public void AlarmUpdateEventsHandler(object sender, AlarmUpdateEventArgs e)
        {
            List<IAesSubscribeCallbackContract> faultetClients = new List<IAesSubscribeCallbackContract>(4);

            foreach (IAesSubscribeCallbackContract client in clientsToPublish)
            {
                if ((client as ICommunicationObject).State.Equals(CommunicationState.Opened))
                {
                    client.UpdateAlarmsEvents(e.Alarm);
                }
                else
                {
                    faultetClients.Add(client);
                }
            }
            lock (clientsLocker)
            {
                foreach (IAesSubscribeCallbackContract client in faultetClients)
                {
                    clientsToPublish.Remove(client);
                }
            }
        }

        /// <summary>
        /// Clients call this service opeartion to subscribe.
        /// A alarms events event handler is registered for this client instance.
        /// </summary>
        public void Subscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<IAesSubscribeCallbackContract>();

            clientsToPublish.Add(callback);
        }

        /// <summary>
        /// Clients call this service opeartion to unsubscribe.
        /// </summary>
        public void Unsubscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<IAesSubscribeCallbackContract>();

            clientsToPublish.Remove(callback);
        }

        #endregion IAesSubscribeContract implementation

        #region IAesPublishContract implementation

        /// <summary>
        /// Information source, in our case it is SCADA Krunching component, call this service operation to report a new Alarm.
        /// An alarm event is raised. The alarm event handlers for each subscriber will execute.
        /// </summary>
        /// <param name="alarm"></param>
        public void PublishAlarmsEvents(AlarmHelper alarm, PublishingStatus status)
        {
            switch (status)
            {
                case PublishingStatus.INSERT:
                    {
                        AlarmsEventsEventArgs e = new AlarmsEventsEventArgs()
                        {
                            Alarm = alarm
                        };

                        try
                        {
                            AlarmEvent(this, e);
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("AES does not have any subscribed client for publishing new alarms. {0}", ex.Message);
                            CommonTrace.WriteTrace(CommonTrace.TraceVerbose, message);
                            Console.WriteLine(message);
                        }

                        break;
                    }

                case PublishingStatus.UPDATE:
                    {
                        AlarmUpdateEventArgs e = new AlarmUpdateEventArgs()
                        {
                            Alarm = alarm
                        };

                        try
                        {
                            AlarmUpdate(this, e);
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("AES does not have any subscribed client for publishing alarm status change. {0}", ex.Message);
                            CommonTrace.WriteTrace(CommonTrace.TraceVerbose, message);
                            Console.WriteLine(message);
                        }
                        break;
                    }
            }
        }

        public void PublishStateChange(AlarmHelper alarm)
        {
            AlarmUpdateEventArgs e = new AlarmUpdateEventArgs()
            {
                Alarm = alarm
            };

            try
            {
                AlarmUpdate(this, e);
            }
            catch (Exception ex)
            {
                string message = string.Format("AES does not have any subscribed client for publishing alarm status change. {0}", ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, message);
                Console.WriteLine(message);
            }
        }

        #endregion IAesPublishContract implementation

        #region IAesIntegirtyContract implementation

        public List<AlarmHelper> InitiateIntegrityUpdate()
        {
            //AesIntegritySfProxy aesIntegritySfProxy = new AesIntegritySfProxy();

            //List<AlarmHelper> integrityResult = aesIntegritySfProxy.InitiateIntegrityUpdate();

            //return integrityResult;
            throw new NotImplementedException();
        }

        #endregion IAesIntegirtyContract implementation
    }
}