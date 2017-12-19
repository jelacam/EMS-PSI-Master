using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    public class ScadaCMDProxy : IScadaCMDContract, IDisposable
    {
        private static IScadaCMDContract proxy;
        private static ChannelFactory<IScadaCMDContract> factory;

        public static IScadaCMDContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    factory = new ChannelFactory<IScadaCMDContract>("*");
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
            try
            {
                if (factory != null)
                {
                    factory = null;
                }
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("ScadaCMDProxy Communication exception: {0}", ce.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("ScadaCMDProxy exception: {0}", e.Message);
            }
        }

        public bool SendDataToSimulator(List<MeasurementUnit> measurements)
        {
			return proxy.SendDataToSimulator(measurements);
        }

        public void Test()
        {
            proxy.Test();
        }
    }
}