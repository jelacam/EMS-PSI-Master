using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EMS.CommonMeasurement;

namespace EMS.ServiceContracts
{
    public class CeOptimizationProxy : IOptimizationAlgorithmContract, IDisposable
    {
        private static IOptimizationAlgorithmContract proxy;
        private static ChannelFactory<IOptimizationAlgorithmContract> factory;

        public static IOptimizationAlgorithmContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    factory = new ChannelFactory<IOptimizationAlgorithmContract>("OptimizationAlgEndpoint");
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

        public bool ChooseOptimization(OptimizationType optimizationType)
        {
            return proxy.ChooseOptimization(optimizationType);
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
                Console.WriteLine("CE proxy exception: {0}", e.Message);
            }
        }
    }
}