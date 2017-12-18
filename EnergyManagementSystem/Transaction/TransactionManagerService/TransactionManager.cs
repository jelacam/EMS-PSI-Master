using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;

namespace EMS.Services.TransactionManagerService
{
    public class TransactionManager : ITransactionCallback, IImporterContract
    {
        public bool ModelUpdate(Delta delta)
        {
            // logika za distribuiranu transakciju

            return true;
        }

        public void Response(string message)
        {
            Console.WriteLine("Response: {0}", message);
        }
    }
}