﻿using CloudCommon;
using EMS.Common;
using EMS.ServiceContracts;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.TransactionManagerService.ServiceFabricProxy
{
    public class TransactionCMDSfProxy : ITransactionContract
    {
        private ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

        private WcfCommunicationClientFactory<ITransactionContract> factory;
        private ServicePartitionClient<WcfCommunicationClient<ITransactionContract>> proxy;

        public TransactionCMDSfProxy()
        {
            factory = new WcfCommunicationClientFactory<ITransactionContract>(
                    clientBinding: Binding.CreateCustomNetTcp(),
                    servicePartitionResolver: resolver,
                    callback: new TransactionManager());

            proxy = new ServicePartitionClient<WcfCommunicationClient<ITransactionContract>>(
                    communicationClientFactory: factory,
                    serviceUri: new Uri("fabric:/EMS/ScadaCommandingCloudService"),
                    listenerName: "*");
        }

        public bool Commit(Delta delta)
        {
            return proxy.InvokeWithRetry(x => x.Channel.Commit(delta));
        }

        public UpdateResult Prepare(ref Delta delta)
        {
            Delta temp = delta;
            return proxy.InvokeWithRetry(x => x.Channel.Prepare(ref temp)); //PROVERITI ZA SVAKI SLUCAJ
        }

        public bool Rollback()
        {
            return proxy.InvokeWithRetry(x => x.Channel.Rollback());
        }
    }
}
