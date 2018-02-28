using EMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
    public class CalculationEngineUIProxy : ICalculationEngineUIContract, IDisposable
    {
        /// <summary>
        /// proxy object
        /// </summary>
        private static ICalculationEngineUIContract proxy;

        /// <summary>
        /// ChannelFactory object
        /// </summary>
        private static ChannelFactory<ICalculationEngineUIContract> factory;

        /// <summary>
        /// Gets or sets instance of ICalculationEngineContract
        /// </summary>
        public static ICalculationEngineUIContract Instance
        {
            get
            {
                if (proxy != null)
                {
                    if (!((ICommunicationObject)proxy).State.Equals(CommunicationState.Opened))
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Creating new channel for CalculationEngineUIProxy");
                        factory = new ChannelFactory<ICalculationEngineUIContract>("*");
                        proxy = factory.CreateChannel();
                    }
                }
                if (proxy == null)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Creating new channel for CalculationEngineUIProxy");
                    factory = new ChannelFactory<ICalculationEngineUIContract>("*");
                    proxy = factory.CreateChannel();
                }

                return proxy;
            }

            set
            {
                if (proxy == null)
                {
                    proxy = value;
                }
            }
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (factory != null)
                {
                    factory = null;
                }
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("Communication exception: {0}", ce.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("CE proxy exception: {0}", e.Message);
            }
        }

        public List<Tuple<double, DateTime>> GetHistoryMeasurements(long gid, DateTime startTime, DateTime endTime)
        {
            return proxy.GetHistoryMeasurements(gid, startTime, endTime);
        }

        public List<Tuple<double, DateTime>> GetTotalProduction(DateTime startTime, DateTime endTime)
        {
            return proxy.GetTotalProduction(startTime, endTime);
        }

        public List<Tuple<double, double, DateTime>> GetCO2Emission(DateTime startTime, DateTime endTime)
        {
            System.Diagnostics.Debugger.Launch();
            List<Tuple<double, double, DateTime>> co2Emission = new List<Tuple<double, double, DateTime>>();
            try
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Client request for CO2 emission for time period: {0} - {1}", startTime.ToString(), endTime.ToString());
                co2Emission = proxy.GetCO2Emission(startTime, endTime);
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Timeout Exception for CalculationEngineUIProxy. Message: {0}", e.Message);
            }

            return co2Emission;
        }

        public List<Tuple<double, double, double>> ReadWindFarmSavingDataFromDb(DateTime startTime, DateTime endTime)
        {
            return proxy.ReadWindFarmSavingDataFromDb(startTime, endTime);
        }

        public List<Tuple<double, double>> ReadWindFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            return proxy.ReadWindFarmProductionDataFromDb(startTime, endTime);
        }
    }
}