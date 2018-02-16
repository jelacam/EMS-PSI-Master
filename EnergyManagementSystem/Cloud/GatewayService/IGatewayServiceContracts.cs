using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayService
{
    /// <summary>
    /// GatewayService implements thist interface
    /// </summary>
    internal interface IGatewayServiceContracts : ICeSubscribeContract,
                                                  ICePublishContract,
                                                  IAesSubscribeContract,
                                                  IAesPublishContract,
                                                  IAesIntegirtyContract
    {
    }
}