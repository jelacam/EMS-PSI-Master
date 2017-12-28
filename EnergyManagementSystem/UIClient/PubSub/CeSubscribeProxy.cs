using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using EMS.ServiceContracts;
using EMS.Common;

namespace UIClient.PubSub
{
    public class CeSubscribeProxy : ICePubSubContract, IDisposable
    {

        private static ICePubSubContract proxy;
        private static DuplexChannelFactory<ICePubSubContract> factory;
        private static InstanceContext context;


        public static ICePubSubContract Instance
        {
            get
            {
                if(proxy == null)
                {
                    context = new InstanceContext(new CePubSubCallbackService());
                    factory = new DuplexChannelFactory<ICePubSubContract>(context, "PubSub");
                    proxy = factory.CreateChannel();
                }
                return proxy;
            }

            set
            {
                if(proxy == null)
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

        public void PublishOptimizationResults(float result)
        {
            CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Client does not have permissions to publis optimization results!");
            throw new Exception("Client does not have permissions to publis optimization results!");
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
