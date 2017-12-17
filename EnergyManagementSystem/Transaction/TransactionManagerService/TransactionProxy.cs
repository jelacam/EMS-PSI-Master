using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using EMS.Common;

namespace EMS.Services.TransactionManagerService
{
    public class TransactionProxy : ITransactionContract
    {
        private static ITransactionContract proxy;
        private static DuplexChannelFactory<ITransactionContract> factory;

        public static ITransactionContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    InstanceContext context = new InstanceContext(new TransactionManager());
                    factory = new DuplexChannelFactory<ITransactionContract>(context, "ScadaTransactionEndpoint");
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

        public bool Commit()
        {
            return proxy.Commit();
        }

        public void Prepare(Delta delta)
        {
            proxy.Prepare(delta);
        }

        public bool Rollback()
        {
            return proxy.Rollback();
        }
    }
}