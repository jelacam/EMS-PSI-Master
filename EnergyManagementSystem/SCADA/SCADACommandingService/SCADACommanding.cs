using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;

namespace EMS.Services.SCADACommandingService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class SCADACommanding : IScadaCMDContract, ITransactionContract 
    {
        private ITransactionCallback transactionCallback;

        #region Transaction
        public bool Commit()
        {
            throw new NotImplementedException();
        }

        public void Prepare(Delta delta)
        {
            transactionCallback = OperationContext.Current.GetCallbackChannel<ITransactionCallback>();
            transactionCallback.Response("Primio Commanding");
        }

        public bool Rollback()
        {
            throw new NotImplementedException();
        }
        #endregion Transactions

        public void Test()
        {
            Console.WriteLine("Test method");


        }
    }
}
