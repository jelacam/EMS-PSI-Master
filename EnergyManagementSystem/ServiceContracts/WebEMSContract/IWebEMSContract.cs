using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    [ServiceContract]
    public interface IWebEMSContract 
    {
        [OperationContract]
        void PublishOptimizationResults(List<MeasurementUI> result);
    }
}
