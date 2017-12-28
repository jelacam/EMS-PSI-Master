using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UIClient.PubSub
{

    public class CePubSubCallbackService : ICePubSubCallbackContract
    {
        public void OptimizationResults(float result)
        {
            Console.WriteLine("OperationContext id: {0}", OperationContext.Current.SessionId);
            Console.WriteLine(string.Format("OPTIMIZATION RESULT: {0}", result.ToString()));
            
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, "OPTIMIZATION RESULT: {0} | SessionID: {1}", result.ToString(), OperationContext.Current.SessionId);
        }
    }
}
