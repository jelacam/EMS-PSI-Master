using EMS.CommonMeasurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.CalculationEngineService.Helpers
{
    public static class CalculationHelper
    {

        /// <summary>
        /// Calculates consumption of consumers
        /// </summary>
        /// <param name="measurements">list of consumers</param>
        /// <returns>total consumption</returns>
        public static float CalculateConsumption(IEnumerable<MeasurementUnit> measurements)
        {
            float retVal = 0;
            foreach (var item in measurements)
            {
                retVal += item.CurrentValue;
            }

            return retVal;
        }

        /// <summary>
        /// Return what optimization is better
        /// </summary>
        /// <param name="measOptGA"></param>
        /// <param name="measOptLinear"></param>
        /// <returns>better optimiziation</returns>
        public static List<MeasurementUnit> ChooseBetterOptimization(List<MeasurementUnit> measOptGA, List<MeasurementUnit> measOptLinear, Dictionary<long, OptimisationModel> optModelMap)
        {
            if (measOptGA == null && measOptLinear == null)
            {
                return null;
            }

            if (measOptLinear == null)
            {
                return measOptGA;
            }

            if (measOptGA == null)
            {
                return measOptLinear;
            }

            if(measOptGA.Count != measOptLinear.Count)
            {
                throw new Exception("[Method = ChooseBetterOptimization] The number of optimized measurements is not the same for GeneticAlgorithm and Linear algorithm");
            }

            //TODO ispraviti kada se sredi optimizacioni model
            var sumGA = 0;
            var sumLinear = 0;
            for(int i = 0; i < measOptLinear.Count; i++)
            {
            }


            return measOptLinear;
        }

    }
}
