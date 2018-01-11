using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.CommonMeasurement;
using System.ServiceModel;

namespace UIClient.PubSub
{
    public class AlarmsEventsSubscribeProxy : IAesPubSubContract, IDisposable
    {
        private static IAesPubSubContract proxy;
        private static DuplexChannelFactory<IAesPubSubContract> factory;
        private static InstanceContext context;

        public static IAesPubSubContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    context = new InstanceContext(new AePubSubCallbackService());
                    factory = new DuplexChannelFactory<IAesPubSubContract>(context, "AlarmsEventsPubSub");
                    proxy = factory.CreateChannel();
                }
                return proxy;
            }

            set
            {
                if (proxy == null)
                {
                    proxy = value;
                }
            }
        }
        

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }
        }

        public void Subscribe()
        {
            proxy.Subscribe();
        }

        public void Unsubscribe()
        {
            proxy.Unsubscribe();
        }
    }
}
