using EMS.Common;
using EMS.ServiceContracts;
using EMS.ServiceContracts.ServiceFabricProxy;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayService.Transaction
{
    public class TMModelUpdate : IImporterContract
    {
        public UpdateResult ModelUpdate(Delta delta)
        {
            Task<UpdateResult> updateResult = ModelUpdateAsync(delta);
            return updateResult.Result;

            //ImporterSfProxy imporeterSfProxy = new ImporterSfProxy();
            //return imporeterSfProxy.ModelUpdate(delta);
        }

        private async Task<UpdateResult> ModelUpdateAsync(Delta delta)
        {
            IImporterAsyncContract importerAsyncProxy = ServiceProxy.Create<IImporterAsyncContract>(
                    serviceUri: new Uri("fabric:/EMS/TransactionManagerCloudService"),
                    listenerName: "TransactionManagerAsyncEndpoint");

            var updateResult = await importerAsyncProxy.ModelUpdate(delta);
            return updateResult;
        }
    }
}