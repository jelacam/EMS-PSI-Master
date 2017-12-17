using EMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    [ServiceContract(CallbackContract = typeof(ITransactionCallback))]
    public interface ITransactionContract
    {
        [OperationContract(IsOneWay = false)]
        void Prepare(Delta delta);

        [OperationContract(IsOneWay = false)]
        bool Commit();

        [OperationContract(IsOneWay = false)]
        bool Rollback();
    }

    public interface ITransactionCallback
    {
        [OperationContract(IsOneWay = true)]
        void Response(string message);
    }
}