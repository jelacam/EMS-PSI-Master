using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using EMS.ServiceContracts.ServiceFabricProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayService.CalculationEngine
{
    public class OptimizationAlgorithm : IOptimizationAlgorithmContract
    {
        public bool ChooseOptimization(OptimizationType optimizationType)
        {
            CeOptimizationSfProxy ceOptimizationSfProxy = new CeOptimizationSfProxy();
            return ceOptimizationSfProxy.ChooseOptimization(optimizationType);
        }
    }
}