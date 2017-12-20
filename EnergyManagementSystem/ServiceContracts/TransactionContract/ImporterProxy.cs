using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;
using System.ServiceModel;

namespace EMS.ServiceContracts
{
    public class ImporterProxy : IImporterContract
    {
        private static IImporterContract proxy;
        private static ChannelFactory<IImporterContract> factory;

        public static IImporterContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    factory = new ChannelFactory<IImporterContract>("ImporterTransaction");
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

        public UpdateResult ModelUpdate(Delta delta)
        {
            return proxy.ModelUpdate(delta);
        }
    }
}