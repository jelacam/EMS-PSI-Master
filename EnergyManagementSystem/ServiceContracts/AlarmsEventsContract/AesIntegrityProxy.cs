using EMS.Common;
using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    public class AesIntegrityProxy : IAesIntegirtyContract, IDisposable
    {
        private static IAesIntegirtyContract proxy = null;
        private static ChannelFactory<IAesIntegirtyContract> factory = null;

        public static IAesIntegirtyContract Instance
        {
            get
            {
                if (proxy != null)
                {
                    if (!((ICommunicationObject)proxy).State.Equals(CommunicationState.Opened))
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Creating new channel for AesIntegrityProxy");
                        factory = new ChannelFactory<IAesIntegirtyContract>("AesIntegrityEndpoint");
                        proxy = factory.CreateChannel();
                    }
                }
                if (proxy == null)
                {
                    factory = new ChannelFactory<IAesIntegirtyContract>("AesIntegrityEndpoint");
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

        public List<AlarmHelper> InitiateIntegrityUpdate()
        {
            return proxy.InitiateIntegrityUpdate();
        }

        public void Dispose()
        {
            try
            {
                if (factory != null)
                {
                    factory = null;
                }
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("Communication exception: {0}", ce.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("AE integrity proxy exception: {0}", e.Message);
            }
        }
    }
}