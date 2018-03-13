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
        public static UpdateResult updateResult = new UpdateResult();

        public UpdateResult ModelUpdate(Delta delta)
        {
            TransactionCESfProxy transactionCESfProxy = new TransactionCESfProxy();
            TransactionCMDSfProxy transactionCMDSfProxy = new TransactionCMDSfProxy();
            TransactionCRSfProxy transactionCRSfProxy = new TransactionCRSfProxy();
            TransactionNMSSfProxy transactionNMSSfProxy = new TransactionNMSSfProxy();

            deltaToApply = delta;
            noRespone = 0;
            toRespond = 1;
            // delta object for caclculation engine - contains EMSFuels and SynchronousMachines
            Delta ceDelta = new Delta();

            updateResult = new UpdateResult();

            List<long> idToRemove = new List<long>(10);

            Delta analogsDelta = delta.SeparateDeltaForEMSType(EMSType.ANALOG);
            Delta emsFuelsDelta = delta.SeparateDeltaForEMSType(EMSType.EMSFUEL);
            Delta synchMachsDelta = delta.SeparateDeltaForEMSType(EMSType.SYNCHRONOUSMACHINE);
            Delta energyConsDelta = delta.SeparateDeltaForEMSType(EMSType.ENERGYCONSUMER);

            ceDelta = emsFuelsDelta + synchMachsDelta + energyConsDelta;

            if (analogsDelta.InsertOperations.Count != 0 || analogsDelta.UpdateOperations.Count != 0)
            {
                toRespond += 2;
            }
            if (ceDelta.InsertOperations.Count != 0 || ceDelta.UpdateOperations.Count != 0)
            {
                toRespond++;
            }
            try
            {
                // first transaction - send delta to NMS
                try
                {
                    updateResult = transactionNMSSfProxy.Prepare(ref delta);
                }
                catch (Exception e)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "Transacion: NMS Prepare phase failed; Message: {0}", e.Message);
                    updateResult.Message = "Transaction: Failed to apply delta on Network Model Service";
                    updateResult.Result = ResultType.Failed;
                    return updateResult;
                }
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
                        try
                        {
                            transactionCESfProxy.Prepare(ref ceDelta);
                        }
                        catch (Exception e)
                        {
                            CommonTrace.WriteTrace(CommonTrace.TraceError, "Transacion: CE Prepare phase failed; Message: {0}", e.Message);
                            updateResult.Message = "Transaction: Failed to apply delta on Calculation Engine Service";
                            updateResult.Result = ResultType.Failed;
                            return updateResult;
                        }
                    }
                    else
                    {
                        try
                        {
                            transactionCRSfProxy.Prepare(ref analogsDelta);
                            transactionCMDSfProxy.Prepare(ref analogsDelta);
                        }
                        catch (Exception e)
                        {
                            CommonTrace.WriteTrace(CommonTrace.TraceError, "Transacion: SCADA Prepare phase failed; Message: {0}", e.Message);
                            updateResult.Message = "Transaction: Failed to apply delta on SCADA CR and CMD Services";
                            updateResult.Result = ResultType.Failed;
                            return updateResult;
                        }
                    }
                }
                else if (toRespond == 3)
                {
                    // second transaction - send ceDelta to CE, analogDelta to SCADA
                    try
                    {
                        transactionCRSfProxy.Prepare(ref analogsDelta);
                        transactionCMDSfProxy.Prepare(ref analogsDelta);
                    }
                    catch (Exception e)
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceError, "Transacion: Prepare phase failed for SCADA Services; Message: {0}", e.Message);
                        updateResult.Message = "Transaction: Failed to apply delta on SCADA Services";
                        updateResult.Result = ResultType.Failed;
                        return updateResult;
                    }
                }
                else if (toRespond == 4)
                {
                    try
                    {
                        transactionCESfProxy.Prepare(ref ceDelta);
                        transactionCRSfProxy.Prepare(ref analogsDelta);
                        transactionCMDSfProxy.Prepare(ref analogsDelta);
                    }
                    catch (Exception e)
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceError, "Transacion: Prepare phase failed for CE or SCADA Services; Message: {0}", e.Message);
                        updateResult.Message = "Transaction: Failed to apply delta on Calculation Engine or SCADA Services";
                        updateResult.Result = ResultType.Failed;
                        return updateResult;
                    }
                }
            }
            catch (Exception e)
            {
                // ako se neki exception desio prilikom transakcije - radi rollback
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Transaction failed; Message: {0}", e.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Start Rollback!");
                transactionCMDSfProxy.Rollback();
                transactionCRSfProxy.Rollback();
                transactionCESfProxy.Rollback();
                transactionNMSSfProxy.Rollback();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback finished!");
            }
            Thread.Sleep(2000);
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
            bool commitResultNMS = false;

            try
            {
                if (toRespond == 4)
                {
                    commitResultScadaCR = false;
                    commitResultScadaCMD = false;
                    commitResultCE = false;
                    commitResultScadaCR = transactionCRSfProxy.Commit();
                    commitResultScadaCMD = transactionCMDSfProxy.Commit();

                    commitResultSCADA = commitResultScadaCMD && commitResultScadaCR;

                    commitResultCE = transactionCESfProxy.Commit();

                    if (!commitResultScadaCR)
                    {
                        updateResult.Message += String.Format("\nCommit phase failed for SCADA Krunching Service");
                    }
                    else
                    {
                        updateResult.Message += String.Format("\nChanges successfully applied on SCADA Krunching Service");
                    }
                    if (!commitResultScadaCMD)
                    {
                        updateResult.Message += String.Format("\nCommit phase failed for SCADA Commanding Service");
                    }
                    else
                    {
                        updateResult.Message += String.Format("\nChanges successfully applied on SCADA Commanding Service");
                    }
                    if (!commitResultCE)
                    {
                        updateResult.Message += String.Format("\nCommit phase failed for Calculation Engine Service");
                    }
                    else
                    {
                        updateResult.Message += String.Format("\nChanges successfully applied on Calculation Engine Service");
                    }
                }
                else if (toRespond == 2)
                {
                    commitResultCE = false;
                    commitResultCE = transactionCESfProxy.Commit();

                    if (!commitResultCE)
                    {
                        updateResult.Message += String.Format("\nCommit phase failed for Calculation Engine Service");
                    }
                    else
                    {
                        updateResult.Message += String.Format("\nChanges successfully applied on Calculation Engine Service");
                    }
                }
                else if (toRespond == 3)
                {
                    commitResultScadaCR = false;
                    commitResultScadaCMD = false;
                    commitResultScadaCR = TransactionCRProxy.Instance.Commit();
                    commitResultScadaCMD = TransactionCMDProxy.Instance.Commit();

                    if (!commitResultScadaCR)
                    {
                        updateResult.Message += String.Format("\nCommit phase failed for SCADA Krunching Service");
                    }
                    else
                    {
                        updateResult.Message += String.Format("\nChanges successfully applied on SCADA Krunching Service");
                    }

                    if (!commitResultScadaCMD)
                    {
                        updateResult.Message += String.Format("\nCommit phase failed for SCADA Commanding Service");
                    }
                    else
                    {
                        updateResult.Message += String.Format("\nChanges successfully applied on SCADA Commanding Service");
                    }
                }
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Transaction Manager Service failed in Commit phase.\nResult message: {0}\nException message: {1}", updateResult.Message, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Starting Rollback phase ... ");
                try
                {
                    TransactionNMSProxy.Instance.Rollback();
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for NMS successfully finished.");
                }
                catch (Exception exc)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for NMS - Message: {0}", exc.Message);
                }
                try
                {
                    TransactionCRProxy.Instance.Rollback();
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for SCADA KR successfully finished.");
                }
                catch (Exception exc)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for SCADA KR - Message: {0}", exc.Message);
                }
                try
                {
                    TransactionCMDProxy.Instance.Rollback();
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for SCADA CMD successfully finished.");
                }
                catch (Exception exc)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for SCADA CMD - Message: {0}", exc.Message);
                }
                try
                {
                    TransactionCEProxy.Instance.Rollback();
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for CE successfully finished.");
                }
                catch (Exception exc)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Rollback for CE - Message: {0}", exc.Message);
                }
            }

            if (commitResultCE && commitResultSCADA)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Commit phase for all services succeeded. Starting commit for NMS!");
                commitResultNMS = transactionNMSSfProxy.Commit();
            }
            if (commitResultNMS && commitResultSCADA && commitResultCE)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Commit phase finished successfully for all services!");
            }
            else
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Commit phase failed!");
                updateResult.Message += string.Format("\nCommit pahse failed");
                updateResult.GlobalIdPairs.Clear();
            }
            updateResult.Message += String.Format("\n\nApply finished");
            toRespond = 1;
            noRespone = 0;
        }

        #endregion Event
    }
}