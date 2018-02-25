using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;
using System.Threading;
using System.ServiceModel;
using EMS.Services.TransactionManagerService.ServiceFabricProxy;

namespace EMS.Services.TransactionManagerService
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class TransactionManager : ITransactionCallback, IImporterContract
    {
        private static Delta deltaToApply;
        private static Delta ceDeltaToApply;
        private static int noRespone = 0;
        private static int toRespond = 1;
        private object obj = new object();

        public UpdateResult ModelUpdate(Delta delta)
        {
            deltaToApply = delta;

            // delta object for caclculation engine - contains EMSFuels and SynchronousMachines
            Delta ceDelta = new Delta();

            UpdateResult updateResult = new UpdateResult();

            List<long> idToRemove = new List<long>(10);

            #region oldDeclarations

            int analogProperty = 0;
            int ceProperty = 0;

            #endregion oldDeclarations

            Delta analogsDelta = delta.SeparateDeltaForEMSType(EMSType.ANALOG);
            Delta emsFuelsDelta = delta.SeparateDeltaForEMSType(EMSType.EMSFUEL);
            Delta synchMachsDelta = delta.SeparateDeltaForEMSType(EMSType.SYNCHRONOUSMACHINE);
            Delta energyConsDelta = delta.SeparateDeltaForEMSType(EMSType.ENERGYCONSUMER);

            ceDelta = emsFuelsDelta + synchMachsDelta + energyConsDelta;

            #region oldcode

            //foreach (ResourceDescription rd_item in delta.InsertOperations)
            //{
            //    foreach (Property pr_item in rd_item.Properties)
            //    {
            //        if (ModelCodeHelper.GetTypeFromModelCode(pr_item.Id).Equals(EMSType.ANALOG))
            //        {
            //            analogProperty++;
            //        }
            //        else if (ModelCodeHelper.GetTypeFromModelCode(pr_item.Id).Equals(EMSType.EMSFUEL) || ModelCodeHelper.GetTypeFromModelCode(pr_item.Id).Equals(EMSType.SYNCHRONOUSMACHINE))
            //        {
            //            ceProperty++;
            //        }
            //    }

            //    if (analogProperty == 0)
            //    {
            //        idToRemove.Add(rd_item.Id);
            //    }
            //    if (ceProperty != 0)
            //    {
            //        ceDelta.InsertOperations.Add(rd_item);
            //    }

            //    analogProperty = 0;
            //    ceProperty = 0;
            //}

            //if (idToRemove.Count != 0 && (delta.InsertOperations.Count - idToRemove.Count > 0))
            //{
            //    if (ceDelta.InsertOperations.Count != 0)
            //    {
            //        toRespond = 4;
            //        TransactionCEProxy.Instance.Prepare(ceDelta);
            //    }
            //    else
            //    {
            //        toRespond = 3;
            //    }

            //    updateResult = TransactionNMSProxy.Instance.Prepare(delta);

            //    foreach (long id in idToRemove)
            //    {
            //        delta.RemoveResourceDescription(id, DeltaOpType.Insert);
            //    }

            //    TransactionCRProxy.Instance.Prepare(delta);
            //    TransactionCMDProxy.Instance.Prepare(delta);
            //}
            //else
            //{
            //    if (ceDelta.InsertOperations.Count != 0)
            //    {
            //        toRespond = 2;
            //        TransactionCEProxy.Instance.Prepare(ceDelta);
            //    }
            //    else
            //    {
            //        toRespond = 1;
            //    }

            //    updateResult = TransactionNMSProxy.Instance.Prepare(delta);
            //}

            #endregion oldcode

            if (analogsDelta.InsertOperations.Count != 0 || analogsDelta.UpdateOperations.Count != 0)
            {
                toRespond++;
            }
            if (ceDelta.InsertOperations.Count != 0 || ceDelta.UpdateOperations.Count != 0)
            {
                toRespond++;
            }

            // first transaction - send delta to NMS
            //updateResult = TransactionNMSProxy.Instance.Prepare(ref delta);

            TransactionNMSSfProxy transactionNMSSfProxy = new TransactionNMSSfProxy();
            TransactionCESfProxy transactionCESfProxy = new TransactionCESfProxy();
            TransactionCMDSfProxy transactionCMDSfProxy = new TransactionCMDSfProxy();
            TransactionCRSfProxy transactionCRSfProxy = new TransactionCRSfProxy();

            updateResult = transactionNMSSfProxy.Prepare(ref delta);

            // create new delta object from delta with gids
            analogsDelta = delta.SeparateDeltaForEMSType(EMSType.ANALOG);
            emsFuelsDelta = delta.SeparateDeltaForEMSType(EMSType.EMSFUEL);
            synchMachsDelta = delta.SeparateDeltaForEMSType(EMSType.SYNCHRONOUSMACHINE);
            energyConsDelta = delta.SeparateDeltaForEMSType(EMSType.ENERGYCONSUMER);

            ceDelta = emsFuelsDelta + synchMachsDelta + energyConsDelta;
            ceDeltaToApply = ceDelta;

            // second transaction - send ceDelta to CE
            if (toRespond == 2)
            {
                if (ceDelta.InsertOperations.Count != 0 || ceDelta.UpdateOperations.Count != 0)
                {
                    //TransactionCEProxy.Instance.Prepare(ref ceDelta);
                    transactionCESfProxy.Prepare(ref ceDelta);
                }
                else
                {
                    //TransactionCRProxy.Instance.Prepare(ref analogsDelta);
                    //TransactionCMDProxy.Instance.Prepare(ref analogsDelta);

                    transactionCRSfProxy.Prepare(ref analogsDelta);
                    transactionCMDSfProxy.Prepare(ref analogsDelta);
                }
            }
            else if (toRespond == 3)
            {
                // second transaction - send ceDelta to CE, analogDelta to SCADA
                //TransactionCEProxy.Instance.Prepare(ref ceDelta);
                //TransactionCRProxy.Instance.Prepare(ref analogsDelta);
                //TransactionCMDProxy.Instance.Prepare(ref analogsDelta);
                transactionCESfProxy.Prepare(ref analogsDelta);
                transactionCRSfProxy.Prepare(ref analogsDelta);
                transactionCMDSfProxy.Prepare(ref analogsDelta);
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
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "An error occured during model update prepare!");
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
            TransactionNMSSfProxy transactionNMSSfProxy = new TransactionNMSSfProxy();
            TransactionCESfProxy transactionCESfProxy = new TransactionCESfProxy();
            TransactionCMDSfProxy transactionCMDSfProxy = new TransactionCMDSfProxy();
            TransactionCRSfProxy transactionCRSfProxy = new TransactionCRSfProxy();

            bool commitResultScadaCR;
            bool commitResultScadaCMD;

            bool commitResultSCADA = true;
            bool commitResultCE = true;

            //bool commitResultNMS = TransactionNMSProxy.Instance.Commit(deltaToApply);
            bool commitResultNMS = transactionNMSSfProxy.Commit(deltaToApply);
            if (toRespond == 3)
            {
                //commitResultScadaCR = TransactionCRProxy.Instance.Commit(deltaToApply);
                //commitResultScadaCMD = TransactionCMDProxy.Instance.Commit(deltaToApply);
                commitResultScadaCR = transactionCRSfProxy.Commit(deltaToApply);
                commitResultScadaCMD = transactionCMDSfProxy.Commit(deltaToApply);

                commitResultSCADA = commitResultScadaCMD && commitResultScadaCR;

                //commitResultCE = TransactionCEProxy.Instance.Commit(deltaToApply);
                commitResultCE = transactionCESfProxy.Commit(deltaToApply);
            }
            else if (toRespond == 2)
            {
                if (ceDeltaToApply.InsertOperations.Count != 0 || ceDeltaToApply.UpdateOperations.Count != 0)
                {
                    //commitResultCE = TransactionCEProxy.Instance.Commit(deltaToApply);
                    commitResultCE = transactionCESfProxy.Commit(deltaToApply);
                }
                else
                {
                    //commitResultScadaCR = TransactionCRProxy.Instance.Commit(deltaToApply);
                    //commitResultScadaCMD = TransactionCMDProxy.Instance.Commit(deltaToApply);
                    commitResultScadaCR = transactionCRSfProxy.Commit(deltaToApply);
                    commitResultScadaCMD = transactionCMDSfProxy.Commit(deltaToApply);
                }
            }

            if (commitResultNMS && commitResultSCADA && commitResultCE)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Commit phase finished!");
            }
            else
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Commit phase failed!");
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Start Rollback!");
                //TransactionNMSProxy.Instance.Rollback();
                //TransactionCRProxy.Instance.Rollback();
                //TransactionCMDProxy.Instance.Rollback();
                //TransactionCEProxy.Instance.Rollback();
                transactionNMSSfProxy.Rollback();
                transactionCRSfProxy.Rollback();
                transactionCMDSfProxy.Rollback();
                transactionCESfProxy.Rollback();
            }
            toRespond = 1;
            noRespone = 0;
        }

        #endregion Event
    }
}