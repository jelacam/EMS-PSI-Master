using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.TransactionManagerService
{
    public class TransactionManager : ITransactionCallback
    {
        public void Response(string message)
        {
            Console.WriteLine("Response: {0}", message);
        }
    }
}