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

        public UpdateResult ModelUpdate(Delta delta)
        {
            UpdateResult updateResult;
            updateResult = TransactionNMSProxy.Instance.Prepare(delta);

            Thread.Sleep(10000);
            if(noRespone == 1)
            {
                bool commitResult = TransactionNMSProxy.Instance.Commit(delta);
                if (commitResult)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Commit phase for NMS finished!");
                }
                else
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Commit phase for NMS failed!");
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Start Rollback!");
                    TransactionNMSProxy.Instance.Rollback();
                }
            }

            // nakon sve tri prepare 

            noRespone = 0;

            return updateResult;
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