using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.CalculationEngineService
{
    public class CeToUI : ICalculationEngineUIContract
    {
        /// <summary>
        /// CalculationEngine instance
        /// </summary>
        private static CalculationEngine ce = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrToCe" /> class
        /// </summary>
        public CeToUI()
        {
        }

        /// <summary>
        /// Sets CalculationEngine of the entity
        /// </summary>
        public static CalculationEngine CalculationEngine
        {
            set
            {
                ce = value;
            }
        }

        public List<Tuple<double, double, DateTime>> GetCO2Emission(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double, DateTime>> retList = new List<Tuple<double, double, DateTime>>();

            try
            {
                retList = ce.ReadCO2EmissionFromDb(startTime, endTime);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[CeToUI] Error GetCO2 emission {0}", ex.Message);
            }

            return retList;
        }

        public List<Tuple<double, DateTime>> GetHistoryMeasurements(long gid, DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, DateTime>> retList = new List<Tuple<double, DateTime>>();
            try
            {
                retList = ce.ReadMeasurementsFromDb(gid, startTime, endTime);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[CeToUI] Error GetHistoryMeasurements {0}", ex.Message);
            }

            return retList;
        }

        public List<Tuple<double, DateTime>> GetTotalProduction(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, DateTime>> retList = new List<Tuple<double, DateTime>>();

            try
            {
                retList = ce.ReadTotalProductionsFromDb(startTime, endTime);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[CeToUI] Error GetTotalProduction {0}", ex.Message);
            }

            return retList;
        }

        public List<Tuple<double, double, double, DateTime>> ReadWindFarmSavingDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double, double, DateTime>> retList = new List<Tuple<double, double, double, DateTime>>();

            try
            {
                retList = ce.ReadWindFarmSavingDataFromDb(startTime, endTime);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[CeToUI] Error ReadWindFarmSavingDataFromD {0}", ex.Message);
            }

            return retList;
        }

        public List<Tuple<double, double>> ReadWindFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double>> retList = new List<Tuple<double, double>>();

            try
            {
                retList = ce.ReadWindFarmProductionDataFromDb(startTime, endTime);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[CeToUI] Error ReadWindFarmSavingDataFromD {0}", ex.Message);
            }

            return retList;
        }

        public List<Tuple<double, double, double, double, double, DateTime>> ReadIndividualFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double, double, double, double, DateTime>> retList = new List<Tuple<double, double, double, double, double, DateTime>>();

            try
            {
                retList = ce.ReadIndividualFarmProductionDataFromDb(startTime, endTime);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "[CeToUI] Error ReadIndividualFarmProductionDataFromDb {0}", ex.Message);
            }

            return retList;
        }
    }
}