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
        private static int toRespond = 1;
        private object obj = new object();

        public UpdateResult ModelUpdate(Delta delta)
        {
            deltaToApply = delta;
            UpdateResult updateResult = new UpdateResult();

            List<long> idToRemove = new List<long>(10);
            int analogProperty = 0;
            foreach (ResourceDescription rd_item in delta.InsertOperations)
            {
                foreach (Property pr_item in rd_item.Properties)
                {
                    if (ModelCodeHelper.GetTypeFromModelCode(pr_item.Id).Equals(EMSType.ANALOG))
                    {
                        analogProperty++;
                    }
                }

                if (analogProperty == 0)
                {
                    idToRemove.Add(rd_item.Id);
                }

                analogProperty = 0;
            }

            if (idToRemove.Count != 0 && (delta.InsertOperations.Count - idToRemove.Count > 0))
            {
                toRespond = 3;
                updateResult = TransactionNMSProxy.Instance.Prepare(delta);

                foreach (long id in idToRemove)
                {
                    delta.RemoveResourceDescription(id, DeltaOpType.Insert);
                }

                TransactionCRProxy.Instance.Prepare(delta);
                TransactionCMDProxy.Instance.Prepare(delta);
            }
            else
            {
                toRespond = 1;
                updateResult = TransactionNMSProxy.Instance.Prepare(delta);
            }

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
            //MessageReached?.Invoke(this, e);

            if (MessageReached != null)
            {
                MessageReached(this, e);
            }
        }

        private void Commit(object sender, EventArgs e)
        {
            bool commitResultScadaCR;
            bool commitResultScadaCMD;

            bool commitResultSCADA = true;

            bool commitResultNMS = TransactionNMSProxy.Instance.Commit(deltaToApply);
            if (toRespond == 3)
            {
                commitResultScadaCR = TransactionCRProxy.Instance.Commit(deltaToApply);
                commitResultScadaCMD = TransactionCMDProxy.Instance.Commit(deltaToApply);

                commitResultSCADA = commitResultScadaCMD && commitResultScadaCR;
            }

            if (commitResultNMS && commitResultSCADA)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Commit phase for NMS finished!");
            }
            else
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Commit phase for NMS failed!");
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Start Rollback!");
                TransactionNMSProxy.Instance.Rollback();
                TransactionCRProxy.Instance.Rollback();
                TransactionCMDProxy.Instance.Rollback();
            }

            noRespone = 0;
        }

        #endregion Event
    }
}