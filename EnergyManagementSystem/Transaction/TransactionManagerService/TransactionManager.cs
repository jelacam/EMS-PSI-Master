using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;
using System.Threading;
using System.ServiceModel;

namespace EMS.Services.TransactionManagerService
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class TransactionManager : ITransactionCallback, IImporterContract
    {
        private static Delta deltaToApply;

        private static int noRespone = 0;
        private readonly int toRespond = 1;
        private object obj = new object();

        public UpdateResult ModelUpdate(Delta delta)
        {
            deltaToApply = delta;

            UpdateResult updateResult;
            updateResult = TransactionNMSProxy.Instance.Prepare(delta);

            //Thread.Sleep(10000);
            //if (noRespone == 1)
            //{
            //    bool commitResult = TransactionNMSProxy.Instance.Commit(delta);
            //    if (commitResult)
            //    {
            //        CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Commit phase for NMS finished!");
            //    }
            //    else
            //    {
            //        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Commit phase for NMS failed!");
            //        CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Start Rollback!");
            //        TransactionNMSProxy.Instance.Rollback();
            //    }
            //}

            // nakon sve tri prepare

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

                if (noRespone == toRespond)
                {
                    this.MessageReached += new MessageReachedEventHandler(Commit);
                    EventArgs e = new EventArgs();
                    OnMessageReached(e);
                }
                this.MessageReached -= new MessageReachedEventHandler(Commit);
            }
        }

        #region Event

        public delegate void MessageReachedEventHandler(object sender, EventArgs e);

        public event MessageReachedEventHandler MessageReached;

        protected virtual void OnMessageReached(EventArgs e)
        {
            MessageReached?.Invoke(this, e);

            //if (MessageReached != null)
            //{
            //    MessageReached(this, e);
            //}
        }

        private void Commit(object sender, EventArgs e)
        {
            bool commitResult = TransactionNMSProxy.Instance.Commit(deltaToApply);
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

            noRespone = 0;
        }

        #endregion Event
    }
}