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
            List<Tuple<double, double, DateTime>> ret = new List<Tuple<double, double, DateTime>>();
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();

            Task task = Task.Run(() => ret = calculationEngineHistorySfProxy.GetCO2Emission(startTime, endTime));
            task.Wait();

            return ret;
        }

        public List<Tuple<double, DateTime>> GetHistoryMeasurements(long gid, DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - GetHistoryMeasurements");
            List<Tuple<double, DateTime>> ret = new List<Tuple<double, DateTime>>();
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();

            Task task = Task.Run(() => ret = calculationEngineHistorySfProxy.GetHistoryMeasurements(gid, startTime, endTime));
            task.Wait();

            return ret;
        }

        public List<Tuple<double, DateTime>> GetTotalProduction(DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - GetTotalProduction");
            List<Tuple<double, DateTime>> ret = new List<Tuple<double, DateTime>>();
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();

            Task task = Task.Run(() => ret = calculationEngineHistorySfProxy.GetTotalProduction(startTime, endTime));
            task.Wait();

            return ret;
        }

        public List<Tuple<double, double, double, double, double, DateTime>> ReadIndividualFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - ReadIndividualFarmProductionDataFromDb");
            List<Tuple<double, double, double, double, double, DateTime>> ret = new List<Tuple<double, double, double, double, double, DateTime>>();
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();

            Task task = Task.Run(() => ret = calculationEngineHistorySfProxy.ReadIndividualFarmProductionDataFromDb(startTime, endTime));
            task.Wait();

            return ret;
        }

        public List<Tuple<double, double, double>> ReadWindFarmSavingDataFromDb(DateTime startTime, DateTime endTime)
        {
            ServiceEventSource.Current.Message("CE History Data - ReadWindFarmSavingDataFromDb");
            List<Tuple<double, double, double>> ret = new List<Tuple<double, double, double>>();
            CalculationEngineHistorySfProxy calculationEngineHistorySfProxy = new CalculationEngineHistorySfProxy();

            Task task = Task.Run(() => ret = calculationEngineHistorySfProxy.ReadWindFarmSavingDataFromDb(startTime, endTime));
            task.Wait();
            return ret;
        }
    }
}