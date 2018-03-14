using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudCommon;
using EMS.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace EMSWeb
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class EMSWeb : StatelessService, IWebEMSContract
    {
        public EMSWeb(StatelessServiceContext context)
            : base(context)
        { }

        public void PublishOptimizationResults(List<MeasurementUI> result)
        {
            Console.WriteLine("kurac");
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .Build();
                    })),
                new ServiceInstanceListener(serviceContext => this.CreateWebServiceCommunicationListener(serviceContext), "PublishEndpoint")

            };
        }

        private ICommunicationListener CreateWebServiceCommunicationListener(StatelessServiceContext context)
        {
            //var bind = WcfUtility.CreateTcpListenerBinding();
            var listener = new WcfCommunicationListener<IWebEMSContract>(
                listenerBinding: Binding.CreateCustomNetTcp(),
                endpointResourceName: "PublishEndpoint",
                serviceContext: context,
                wcfServiceObject: this);

            return listener;
        }
    }
}
