using EMS.ServiceContracts;
using EMS.ServiceContracts.ServiceFabricProxy;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayService.CEHistory
{
    public class CeHistoryData : ICalculationEngineUIContract
    {
        public List<Tuple<double, double, DateTime>> GetCO2Emission(DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - GetCO2Emission");
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();
            return calculationEngineHistorySfProxy.GetCO2Emission(startTime, endTime);
        }

        public List<Tuple<double, DateTime>> GetHistoryMeasurements(long gid, DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - GetHistoryMeasurements");
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();
            return calculationEngineHistorySfProxy.GetHistoryMeasurements(gid, startTime, endTime);
        }

        public List<Tuple<double, DateTime>> GetTotalProduction(DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - GetTotalProduction");
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();
            return calculationEngineHistorySfProxy.GetTotalProduction(startTime, endTime);
        }

        public List<Tuple<double, double, double, double, double>> ReadIndividualFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - ReadIndividualFarmProductionDataFromDb");
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();
            return calculationEngineHistorySfProxy.ReadIndividualFarmProductionDataFromDb(startTime, endTime);
        }

        public List<Tuple<double, double, double>> ReadWindFarmSavingDataFromDb(DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - ReadWindFarmSavingDataFromDb");
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();
            return calculationEngineHistorySfProxy.ReadWindFarmSavingDataFromDb(startTime, endTime);
        }
    }
}