using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    /// <summary>
    /// Wcf contract for UI integirty update with existing alarms on AlarmsEvents Service
    /// </summary>
    [ServiceContract]
    public interface IAesIntegirtyContract
    {
        /// <summary>
        /// Initiates integity update 
        /// </summary>
        /// <returns>List of existing alarm object on AES</returns>
        [OperationContract]
        List<AlarmHelper> InitiateIntegrityUpdate();
    }
}
