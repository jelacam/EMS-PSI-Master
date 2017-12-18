using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;
using System.Threading;

namespace EMS.Services.TransactionManagerService
{
    public class TransactionManager : ITransactionCallback, IImporterContract
    {
        private static int noRespone = 0;
        private object obj = new object();

        public bool ModelUpdate(Delta delta)
        {
            // logika za distribuiranu transakciju
            TransactionNMSProxy.Instance.Prepare(delta);

            Thread.Sleep(10000);
            if(noRespone == 1)
            {
                TransactionNMSProxy.Instance.Commit();
            }

            // nakon sve tri prepare 
            noRespone = 0;
            return true;
        }

        public void Response(string message)
        {
            lock (obj)
            {
                Console.WriteLine("Response: {0}", message);
                if (message.Equals("OK"))
                {
                    noRespone++;
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Model update prepare was successful on service!");
                }
                else
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "An error ocured during model update prepare!");
                }
            }
        }
    }
}