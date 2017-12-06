using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    public class ScadaCRProxy : IScadaCRContract, IDisposable
    {
        private static IScadaCRContract proxy;
        private static ChannelFactory<IScadaCRContract> factory;

        public static IScadaCRContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    factory = new ChannelFactory<IScadaCRContract>("*");
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

        public void Test()
        {
            proxy.Test();
        }

        public bool SendValues(byte[] value)
        {
            return proxy.SendValues(value);
        }
    }
}