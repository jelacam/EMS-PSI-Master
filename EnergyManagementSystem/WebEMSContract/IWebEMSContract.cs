using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WebEMSContract
{
    [ServiceContract]
    public interface IWebEMSContract
    {
        [OperationContract]
        void PublishOptimizationResults(List<MeasurementUI> result);
    }
}
