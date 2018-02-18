using EMS.Common;
using EMS.ServiceContracts;
using EMS.ServiceContracts.ServiceFabricProxy;
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
            ImporterSfProxy imporeterSfProxy = new ImporterSfProxy();
            return imporeterSfProxy.ModelUpdate(delta);
        }
    }
}
