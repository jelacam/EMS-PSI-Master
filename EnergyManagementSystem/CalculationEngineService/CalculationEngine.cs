//-----------------------------------------------------------------------
// <copyright file="CalculationEngine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
    using Common;
    using CommonMeasurement;
    using GeneticAlgorithm;
    using Helpers;
    using LinearAlgorithm;
    using NetworkModelService.DataModel.Production;
    using NetworkModelService.DataModel.Wires;
    using PubSub;
    using ServiceContracts;
    using Simulation;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading;

    /// <summary>
    /// Class for CalculationEngine
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CalculationEngine : ITransactionContract
    {
        #region Fields

        private static List<ResourceDescription> internalSynchMachines;
        private static List<ResourceDescription> internalSynchMachinesCopy;
        private static List<ResourceDescription> internalEmsFuels;
        private static List<ResourceDescription> internalEmsFuelsCopy;
        private static List<ResourceDescription> internalEnergyConsumers;
        private static List<ResourceDescription> internalEnergyConsumersCopy;

        private static IDictionary<long, SynchronousMachine> synchronousMachines;
        private static IDictionary<long, EMSFuel> fuels;
        private static IDictionary<long, EnergyConsumer> energyConsumers;

        private object lockObj;

        private PublisherService publisher = null;
        private ITransactionCallback transactionCallback;
        private UpdateResult updateResult;
        private float minProduction;
        private float maxProduction;

        private float profit = 0;
        private float windProductionPct = 0;
        private float windProductionkW = 0;
        private float solarProductionPct = 0;
        private float solarProductionkW = 0;
        private float hydroProductionPct = 0;
        private float hydroProductionkW = 0;
        private float coalProductionPct = 0;
        private float coalProductionkW = 0;
        private float oilProductionPct = 0;
        private float oilProductionkW = 0;
        private float emissionCO2Renewable = 0;
        private float emissionCO2NonRenewable = 0;
        private float totalProduction = 0;
        private float totalCost = 0;

        //private float totalCostWithRenewable = 0;
        private float totalCostWithoutWindAndSolar = 0;

        private SynchronousMachineCurveModels generatorCharacteristics = new SynchronousMachineCurveModels();
        private Dictionary<string, SynchronousMachineCurveModel> generatorCurves;

        public SynchronousMachineCurveModels GeneratorCharacteristics
        {
            get { return generatorCharacteristics; }
            set { generatorCharacteristics = value; }
        }

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationEngine" /> class
        /// </summary>
        public CalculationEngine()
        {
            publisher = new PublisherService();
            generatorCurves = new Dictionary<string, SynchronousMachineCurveModel>();
            lockObj = new object();

            synchronousMachines = new Dictionary<long, SynchronousMachine>();
            fuels = new Dictionary<long, EMSFuel>();
            energyConsumers = new Dictionary<long, EnergyConsumer>();

            internalSynchMachines = new List<ResourceDescription>(5);
            internalSynchMachinesCopy = new List<ResourceDescription>(5);
            internalEmsFuels = new List<ResourceDescription>(5);
            internalEmsFuelsCopy = new List<ResourceDescription>(5);
            internalEnergyConsumers = new List<ResourceDescription>(5);
            internalEnergyConsumersCopy = new List<ResourceDescription>(5);

            GeneratorCharacteristics = LoadCharacteristics.Load();
        }

        #endregion Constructor

        #region Optimization methods

        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measEnergyConsumers">list of measurements for Energy Consumers</param>
        /// <param name="measGenerators">list of measurements for Generators</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool Optimize(List<MeasurementUnit> measEnergyConsumers, List<MeasurementUnit> measGenerators, float windSpeed, float sunlight)
        {
            bool result = false;
            totalProduction = 0;

            PublishConsumersToUI(measEnergyConsumers);

            Dictionary<long, OptimisationModel> optModelMap = GetOptimizationModelMap(measGenerators, windSpeed, sunlight);
            float powerOfConsumers = CalculationHelper.CalculateConsumption(measEnergyConsumers);

            List<MeasurementUnit> measurementsOptimized = DoOptimization(optModelMap, powerOfConsumers, windSpeed, sunlight);

            if (measurementsOptimized != null && measurementsOptimized.Count > 0)
            {
                totalProduction = measurementsOptimized.Sum(x => x.CurrentValue);

                if (WriteTotalProductionIntoDb(totalProduction, totalCost, totalCostWithoutWindAndSolar, profit, DateTime.Now, windProductionkW, windProductionPct, solarProductionkW, solarProductionPct, hydroProductionkW, hydroProductionPct, coalProductionkW, coalProductionPct, oilProductionkW, oilProductionPct))
                {
                    Console.WriteLine("The total production is recorded into history database.");
                }

                if (InsertMeasurementsIntoDb(measurementsOptimized))
                {
                    Console.WriteLine("Inserted {0} Measurement(s) into history database.", measurementsOptimized.Count);
                }

                if (WriteCO2EmissionIntoDb(emissionCO2NonRenewable, emissionCO2Renewable, DateTime.Now))
                {
                    Console.WriteLine("The CO2 emission is recorded into history database.");
                }

                PublishGeneratorsToUI(measurementsOptimized);

                try
                {
                    if (ScadaCMDProxy.Instance.SendDataToSimulator(measurementsOptimized))
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceInfo, "CE sent {0} optimized MeasurementUnit(s) to SCADACommanding.", measurementsOptimized.Count);
                        Console.WriteLine("CE sent {0} optimized MeasurementUnit(s) to SCADACommanding.", measurementsOptimized.Count);

                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                }
            }

            return result;
        }

        public void PopulateDatabase()
        {
            List<MeasurementUnit> measGenerators = new List<MeasurementUnit>();
            ConvertorHelper convHelper = new ConvertorHelper();
            foreach (var syncMach in synchronousMachines)
            {
                measGenerators.Add(new MeasurementUnit()
                {
                    CurrentValue = 0,
                    MaxValue = syncMach.Value.MaxQ,
                    MinValue = syncMach.Value.MinQ,
                    Gid = syncMach.Key,
                });
            }

            DateTime dateTime = new DateTime(2017, 8, 14);
            DateTime endTime = new DateTime(2018, 3, 17);
            int index = 0;
            DummySimulation simulation = new DummySimulation();
            Random rnd = new Random();
            float rndValue = ((float)rnd.NextDouble() + 1) / 3;
            while (dateTime < endTime)
            {
                if (index % 24 == 0)
                {
                    rndValue = ((float)rnd.NextDouble() + 1) / 3;
                }
                Console.WriteLine("Completed: {0} %", ((float)index / 10300f) * 100);

                float currentConsumption = simulation.GetCurrentConsumption(index % 24) / 4 - 1500;
                currentConsumption *= rndValue;
                if(currentConsumption < 500)
                {
                    currentConsumption = 500;
                }
                float windSpeed = (1 - rndValue) * simulation.GetWindSpeed(index % 24);
                float sunLight = rndValue * simulation.GetSunLight(index % 24);

                Dictionary<long, OptimisationModel> optModelMap = GetOptimizationModelMap(measGenerators, windSpeed, sunLight);

                var measurementsOptimized = DoOptimization(optModelMap, currentConsumption, windSpeed, sunLight);

                if (InsertMeasurementsIntoDb(measurementsOptimized, dateTime))
                {
                    //Console.WriteLine("Inserted {0} Measurement(s) into history database.", measurementsOptimized.Count);
                }

                if (WriteCO2EmissionIntoDb(emissionCO2NonRenewable, emissionCO2Renewable, dateTime))
                {
                    //Console.WriteLine("The CO2 emission is recorded into history database.");
                }

                totalProduction = measurementsOptimized.Sum(x => x.CurrentValue);

                if (WriteTotalProductionIntoDb(totalProduction, totalCost, totalCostWithoutWindAndSolar, profit, dateTime, windProductionkW, windProductionPct, solarProductionkW, solarProductionPct, hydroProductionkW, hydroProductionPct, coalProductionkW, coalProductionPct, oilProductionkW, oilProductionPct))
                {
                   // Console.WriteLine("The total production is recorded into history database.");
                }

                dateTime = dateTime.AddHours(1);
                index++;
            }

            Console.WriteLine("Completed: 100 %");
        }

        private List<MeasurementUnit> DoOptimization(Dictionary<long, OptimisationModel> optModelMap, float powerOfConsumers, float windSpeed, float sunlight)
        {
            try
            {
                Dictionary<long, OptimisationModel> optModelMapOptimizied = null;
                totalCost = 0;
                totalCostWithoutWindAndSolar = 0;
                profit = 0;
                windProductionkW = 0;
                windProductionPct = 0;
                solarProductionkW = 0;
                solarProductionPct = 0;
                hydroProductionkW = 0;
                hydroProductionPct = 0;
                coalProductionkW = 0;
                coalProductionPct = 0;
                oilProductionkW = 0;
                oilProductionPct = 0;
                emissionCO2Renewable = 0;
                emissionCO2NonRenewable = 0;

                if (PublisherService.OptimizationType.Equals(OptimizationType.Genetic))
                {
                    optModelMapOptimizied = CalculateWithGeneticAlgorithm(optModelMap, powerOfConsumers);
                }
                else
                {
                    optModelMapOptimizied = CalculateWithLinearAlgorithm(optModelMap, powerOfConsumers);
                }

                string algorithm = PublisherService.OptimizationType.Equals(OptimizationType.Genetic) ? "GENETIC" : "LINEAR";

                Console.WriteLine("\n--------------------------------------------------");
                Console.WriteLine("CE report: {0}", algorithm);
                Console.WriteLine("\tOptimized: {0}kW", powerOfConsumers);
                Console.WriteLine("\tCost: {0}$", totalCost);
                Console.WriteLine("\tCost without wind and solar generators: {0}$", totalCostWithoutWindAndSolar);
                Console.WriteLine("\tProfit: {0}$", profit);
                Console.WriteLine("\tCO2 production without wind and solar generators: {0}t", emissionCO2NonRenewable);
                Console.WriteLine("\tCO2 production: {0}t", emissionCO2Renewable);
                Console.WriteLine("\tWind production: {0}kW ({1}%)", windProductionkW, windProductionPct);
                Console.WriteLine("\tSolar production: {0}kW ({1}%)", solarProductionkW, solarProductionPct);
                Console.WriteLine("\tHydro production: {0}kW ({1}%)", hydroProductionkW, hydroProductionPct);
                Console.WriteLine("\tCoal production: {0}kW ({1}%)", coalProductionkW, coalProductionPct);
                Console.WriteLine("\tOil production: {0}kW ({1}%)", oilProductionkW, oilProductionPct);
                Console.WriteLine("--------------------------------------------------\n");

                return OptModelMapToListMeasUI(optModelMapOptimizied, PublisherService.OptimizationType);
            }
            catch (Exception e)
            {
                throw new Exception("[Method = DoOptimization] Exception = " + e.Message);
            }
        }

        private Dictionary<long, OptimisationModel> CalculateWithLinearAlgorithm(Dictionary<long, OptimisationModel> optModelMap, float powerOfConsumers)
        {
            LinearOptimization linearAlgorithm = new LinearOptimization(minProduction, maxProduction);
            Dictionary<long, OptimisationModel> optModelMapOptimizied = linearAlgorithm.Start(optModelMap, powerOfConsumers);
            totalCost = linearAlgorithm.Cost; // ukupna cena linearne optimizacije
            totalCostWithoutWindAndSolar = linearAlgorithm.CostWithoutWindAndSolar; // ukupna cena linearne optimizacije bez wind i solar
            profit = linearAlgorithm.Profit; // koliko je $ ustedjeno koriscenjem wind i solar
            windProductionPct = linearAlgorithm.PowerOfWindPct; // procenat proizvodnje wind u odnosu na ukupnu proizvodnju
            windProductionkW = linearAlgorithm.PowerOfWind; // kW proizvodnje wind u ukupnoj proizvodnji
            solarProductionPct = linearAlgorithm.PowerOfSolarPct;
            solarProductionkW = linearAlgorithm.PowerOfSolar;
            hydroProductionPct = linearAlgorithm.PowerOfHydroPct;
            hydroProductionkW = linearAlgorithm.PowerOfHydro;
            coalProductionPct = linearAlgorithm.PowerOfCoalPct;
            coalProductionkW = linearAlgorithm.PowerOfCoal;
            oilProductionPct = linearAlgorithm.PowerOfOilPct;
            oilProductionkW = linearAlgorithm.PowerOfOil;
            emissionCO2Renewable = linearAlgorithm.CO2; // CO2 emisija sa wind i solar izrazena u tonama
            emissionCO2NonRenewable = linearAlgorithm.CO2WithoutWindAndSolar; //CO2 emisija bez wind i solar izrazena u tonama

            return optModelMapOptimizied;
        }

        private Dictionary<long, OptimisationModel> CalculateWithGeneticAlgorithm(Dictionary<long, OptimisationModel> optModelMap, float powerOfConsumers)
        {
            Dictionary<long, OptimisationModel> optModelMapOptimizied;
            float powerOfConsumersWithoutRenewable = powerOfConsumers;

            Dictionary<long, OptimisationModel> optModelMapNonRenewable = new Dictionary<long, OptimisationModel>();
            Dictionary<long, OptimisationModel> optModelMapNonRenewableClone = new Dictionary<long, OptimisationModel>();
            windProductionkW = 0;
            foreach (var item in optModelMap)
            {
                if (item.Value.Renewable)
                {
                    item.Value.GenericOptimizedValue = item.Value.MaxPower;
                    powerOfConsumersWithoutRenewable -= item.Value.MaxPower;
                    if (item.Value.EmsFuel.FuelType.Equals(EmsFuelType.wind))
                    {
                        windProductionkW += item.Value.MaxPower;
                    }
                }
                else
                {
                    optModelMapNonRenewable.Add(item.Key, item.Value);
                }
            }
            float powerOfRenewable = powerOfConsumers - powerOfConsumersWithoutRenewable;

            //GAOptimization gaoNonRenewable = new GAOptimization(powerOfConsumers, optModelMapNonRenewable);
            //var optModelMapOptimiziedNonRenewable = gaoNonRenewable.StartAlgorithmWithReturn();

            GAOptimization gaoRenewable = new GAOptimization(powerOfConsumersWithoutRenewable, optModelMapNonRenewable);
            optModelMapOptimizied = gaoRenewable.StartAlgorithmWithReturn();

            foreach (var optModel in optModelMapNonRenewable)
            {
                optModelMapNonRenewableClone.Add(optModel.Key, optModel.Value.Clone());
            }

            if (optModelMapNonRenewableClone.Count == 0)
            {
                return optModelMap;
            }

            float totalOptimizedPower = CalculatePowerOfEach(optModelMap);

            var coalModel = optModelMapNonRenewableClone.FirstOrDefault(x => x.Value.EmsFuel.FuelType == EmsFuelType.coal);
            coalModel.Value.GenericOptimizedValue = optModelMapOptimizied[coalModel.Key].GenericOptimizedValue + windProductionkW;
            totalCostWithoutWindAndSolar = CalculateCost(optModelMapNonRenewableClone, OptimizationType.Genetic);
            totalCost = gaoRenewable.TotalCost;
            profit = totalCostWithoutWindAndSolar - totalCost;

            emissionCO2Renewable = gaoRenewable.EmissionCO2;
            emissionCO2NonRenewable = CalculateCO2(optModelMapNonRenewableClone);

            windProductionPct = 100 * windProductionkW / totalOptimizedPower;
            solarProductionPct = 100 * solarProductionkW / totalOptimizedPower;
            oilProductionPct = 100 * oilProductionkW / totalOptimizedPower;
            coalProductionPct = 100 * coalProductionkW / totalOptimizedPower;
            hydroProductionPct = 100 * hydroProductionkW / totalOptimizedPower;

            //foreach(var item in optModelMapOptimizied)
            //{
            //    optModelMap[item.Key].GenericOptimizedValue = item.Value.GenericOptimizedValue;
            //}
            return optModelMap;
        }

        private float CalculatePowerOfEach(Dictionary<long, OptimisationModel> optModelMap)
        {
            float optimizedTotalPower = 0;
            foreach (var optModel in optModelMap.Values)
            {
                optimizedTotalPower += optModel.LinearOptimizedValue;
                switch (optModel.EmsFuel.FuelType)
                {
                    case EmsFuelType.coal:
                        coalProductionkW += optModel.GenericOptimizedValue;
                        break;
                    case EmsFuelType.hydro:
                        hydroProductionkW += optModel.GenericOptimizedValue;
                        break;
                    case EmsFuelType.oil:
                        oilProductionkW += optModel.GenericOptimizedValue;
                        break;
                    case EmsFuelType.solar:
                        solarProductionkW += optModel.GenericOptimizedValue;
                        break;
                    case EmsFuelType.wind:
                        windProductionkW += optModel.GenericOptimizedValue;
                        break;
                    default:
                        break;
                }
            }

            return optimizedTotalPower;
        }

        private static float CalculateCost(Dictionary<long, OptimisationModel> optModelMap, OptimizationType optimizationType)
        {
            float cost = 0;
            foreach (var optModel in optModelMap.Values)
            {
                float price = optimizationType == OptimizationType.Genetic ?
                    optModel.CalculatePrice(optModel.GenericOptimizedValue) : optModel.CalculatePrice(optModel.LinearOptimizedValue);
                cost += price;
            }

            return cost;
        }

        public static float CalculateCO2(Dictionary<long, OptimisationModel> optModelMap)
        {
            float emCO2 = 0;
            foreach (var optModel in optModelMap.Values)
            {
                emCO2 += optModel.GenericOptimizedValue * optModel.EmissionFactor;
            }

            return emCO2;
        }

        private Dictionary<long, OptimisationModel> GetOptimizationModelMap(List<MeasurementUnit> measGenerators, float windSpeed, float sunlight)
        {
            lock (lockObj)
            {
                Dictionary<long, OptimisationModel> optModelMap = new Dictionary<long, OptimisationModel>();
                minProduction = 0;
                maxProduction = 0;
                FillGeneratorCurves();

                foreach (var measUnit in measGenerators)
                {
                    if (synchronousMachines.ContainsKey(measUnit.Gid))
                    {
                        SynchronousMachine sm = synchronousMachines[measUnit.Gid];
                        EMSFuel emsf = fuels[sm.Fuel];
                        if (generatorCurves[sm.Mrid] != null)
                        {
                            OptimisationModel om = new OptimisationModel(sm, emsf, measUnit, windSpeed, sunlight, generatorCurves[sm.Mrid]);
                            if (om.Managable)
                            {
                                maxProduction += om.MaxPower;
                                minProduction += om.MinPower;
                            }

                            optModelMap.Add(om.GlobalId, om);
                        }
                    }
                }

                return optModelMap;
            }
        }

        private void FillGeneratorCurves()
        {
            lock (lockObj)
            {
                if (synchronousMachines.Count > 0)
                {
                    generatorCurves.Clear();

                    foreach (SynchronousMachine item in synchronousMachines.Values)
                    {
                        generatorCurves.Add(item.Mrid.ToString(), null);
                    }

                    //if (generatorCurves.Count == GeneratorCharacteristics.Curves.Count)
                    {
                        for (int i = 0; i < generatorCurves.Count; i++)
                        {
                            string mrid = GeneratorCharacteristics.Curves[i].MRId;
                            SynchronousMachineCurveModel smcm = GeneratorCharacteristics.Curves[i];
                            generatorCurves[mrid] = smcm;
                        }
                    }
                }
            }
        }

        #endregion Optimization methods

        #region UI methods

        private List<MeasurementUnit> OptModelMapToListMeasUI(Dictionary<long, OptimisationModel> optModelMap, OptimizationType optType)
        {
            List<MeasurementUnit> retList = new List<MeasurementUnit>();
            foreach (var optModel in optModelMap)
            {
                float currValue = 0;

                if (optType == OptimizationType.Linear)
                {
                    currValue = optModel.Value.LinearOptimizedValue;
                }
                else if (optType == OptimizationType.Genetic)
                {
                    currValue = optModel.Value.GenericOptimizedValue;
                }

                retList.Add(new MeasurementUnit()
                {
                    Gid = optModel.Value.GlobalId,
                    MaxValue = optModel.Value.MaxPower,
                    MinValue = optModel.Value.MinPower,
                    OptimizationType = optType,
                    CurrentValue = currValue,
                    CurrentPrice = optModel.Value.Price
                });
            }

            return retList;
        }

        private void PublishGeneratorsToUI(List<MeasurementUnit> measurementsFromGenerators)
        {
            List<MeasurementUI> measListUI = new List<MeasurementUI>();
            foreach (var meas in measurementsFromGenerators)
            {
                MeasurementUI measUI = new MeasurementUI();
                measUI.Gid = meas.Gid;
                measUI.CurrentValue = meas.CurrentValue;
                measUI.TimeStamp = meas.TimeStamp;
                measUI.OptimizationType = (int)meas.OptimizationType;
                measUI.Price = meas.CurrentPrice;
                measListUI.Add(measUI);
            }
            publisher.PublishOptimizationResults(measListUI);
        }

        private void PublishConsumersToUI(List<MeasurementUnit> measurementsFromConsumers)
        {
            List<MeasurementUI> measUIList = new List<MeasurementUI>();
            foreach (var meas in measurementsFromConsumers)
            {
                MeasurementUI measUI = new MeasurementUI();
                measUI.Gid = meas.Gid;
                measUI.CurrentValue = meas.CurrentValue;
                measUI.TimeStamp = meas.TimeStamp;
                measUIList.Add(measUI);
            }
            publisher.PublishOptimizationResults(measUIList);
        }

        #endregion UI methods

        /// <summary>
        /// Insert data into history db
        /// </summary>
        /// <param name="measurements">List of measurements</param>
        /// <returns>Success</returns>
        public bool InsertMeasurementsIntoDb(List<MeasurementUnit> measurements)
        {
            bool success = true;

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertMeasurement", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (MeasurementUnit mu in measurements)
                        {
                            cmd.Parameters.Add("@gidMeasurement", SqlDbType.BigInt).Value = mu.Gid;
                            cmd.Parameters.Add("@timeMeasurement", SqlDbType.DateTime).Value = mu.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            cmd.Parameters.Add("@valueMeasurement", SqlDbType.Float).Value = mu.CurrentValue;
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    success = false;
                    string message = string.Format("Failed to insert new Measurement into database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return success;
        }

        private bool InsertMeasurementsIntoDb(List<MeasurementUnit> measurements, DateTime dateTime)
        {
            bool success = true;

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertMeasurement", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (MeasurementUnit mu in measurements)
                        {
                            cmd.Parameters.Add("@gidMeasurement", SqlDbType.BigInt).Value = mu.Gid;
                            cmd.Parameters.Add("@timeMeasurement", SqlDbType.DateTime).Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            cmd.Parameters.Add("@valueMeasurement", SqlDbType.Float).Value = mu.CurrentValue;
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    success = false;
                    string message = string.Format("Failed to insert new Measurement into database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return success;
        }

        /// <summary>
        /// Read measurements from history database
        /// </summary>
        /// <param name="gid">Global identifikator of object</param>
        public List<Tuple<double, DateTime>> ReadMeasurementsFromDb(long gid, DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, DateTime>> retVal = new List<Tuple<double, DateTime>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM HistoryMeasurement WHERE (GID=@gid) AND (MeasurementTime BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@gid", SqlDbType.BigInt).Value = gid;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, DateTime>(Convert.ToDouble(reader[3]), Convert.ToDateTime(reader[2])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Measurements from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        public bool WriteTotalProductionIntoDb(float totalProduction, float totalCost, float totalCostWithoutWindAndSolar, float profit, DateTime timeOfCalculation, float windProduction, float windProductionPercent, float solarProduction, float solarProductionPercent, float hydroProduction, float hydroProductionPercent, float coalProduction, float coalProductionPercent, float oilProduction, float oilProductionPercent)
        {
            bool success = true;

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertTotalProduction", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@totalProduction", SqlDbType.Float).Value = totalProduction;
                        cmd.Parameters.Add("@totalCost", SqlDbType.Float).Value = totalCost;
                        cmd.Parameters.Add("@totalCostWithoutWindAndSolar", SqlDbType.Float).Value = totalCostWithoutWindAndSolar;
                        cmd.Parameters.Add("@profit", SqlDbType.Float).Value = profit;
                        cmd.Parameters.Add("@timeOfCalculation", SqlDbType.DateTime).Value = timeOfCalculation.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@windProduction", SqlDbType.Float).Value = windProduction;
                        cmd.Parameters.Add("@windProductionPercent", SqlDbType.Float).Value = windProductionPercent;
                        cmd.Parameters.Add("@solarProduction", SqlDbType.Float).Value = solarProduction;
                        cmd.Parameters.Add("@solarProductionPercent", SqlDbType.Float).Value = solarProductionPercent;
                        cmd.Parameters.Add("@hydroProduction", SqlDbType.Float).Value = hydroProduction;
                        cmd.Parameters.Add("@hydroProductionPercent", SqlDbType.Float).Value = hydroProductionPercent;
                        cmd.Parameters.Add("@coalProduction", SqlDbType.Float).Value = coalProduction;
                        cmd.Parameters.Add("@coalProductionPercent", SqlDbType.Float).Value = coalProductionPercent;
                        cmd.Parameters.Add("@oilProduction", SqlDbType.Float).Value = oilProduction;
                        cmd.Parameters.Add("@oilProductionPercent", SqlDbType.Float).Value = oilProductionPercent;

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    success = false;
                    string message = string.Format("Failed to insert total production into database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return success;
        }

        /// <summary>
        /// Read total production from database
        /// </summary>
        /// <param name="startTime">Start time for period</param>
        /// <param name="endTime">End time for period</param>
        /// <returns>Tuple list od pair double and datetime for period</returns>
        public List<Tuple<double, DateTime>> ReadTotalProductionsFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, DateTime>> retVal = new List<Tuple<double, DateTime>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT TotalProduction,TimeOfCalculation FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, DateTime>(Convert.ToDouble(reader[0]), Convert.ToDateTime(reader[1])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Measurements from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Read wind farm saving data from database (total cost without wind farm, total cost with wind farm and profit)
        /// </summary>
        /// <param name="startTime">start time of period</param>
        /// <param name="endTime">end time of period</param>
        /// <returns>tuples of double, double, time (total cost, total cost with wind farm,)</returns>
        public List<Tuple<double, double, double, DateTime>> ReadWindFarmSavingDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double, double, DateTime>> retVal = new List<Tuple<double, double, double, DateTime>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT TotalCostWithoutWindAndSolar,TotalCost,Profit,TimeOfCalculation FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double, double, DateTime>(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1]), Convert.ToDouble(reader[2]), Convert.ToDateTime(reader[3])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Wind Farm Saving from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        public List<Tuple<double, double>> ReadWindFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double>> retVal = new List<Tuple<double, double>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT WindProduction, WindProductionPercent FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double>(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Wind Farm Production Data from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        public List<Tuple<double, double>> ReadSolarFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double>> retVal = new List<Tuple<double, double>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT SolarProduction, SolarProductionPercent FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double>(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Solar Farm Production Data from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        public List<Tuple<double, double>> ReadHydroFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double>> retVal = new List<Tuple<double, double>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT HydroProduction, HydroProductionPercent FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double>(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Hydro Farm Production Data from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        public List<Tuple<double, double>> ReadCoalFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double>> retVal = new List<Tuple<double, double>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT CoalProduction, CoalProductionPercent FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double>(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Coal Farm Production Data from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        public List<Tuple<double, double>> ReadOilFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double>> retVal = new List<Tuple<double, double>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT OilProduction, OilProductionPercent FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double>(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read Oil Farm Production Data from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        public List<Tuple<double, double, double, double, double, DateTime>> ReadIndividualFarmProductionDataFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double, double, double, double, DateTime>> retVal = new List<Tuple<double, double, double, double, double, DateTime>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT WindProduction, SolarProduction, HydroProduction, CoalProduction, OilProduction, TimeOfCalculation FROM TotalProduction WHERE (TimeOfCalculation BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double, double, double, double, DateTime>(Convert.ToDouble(reader[0]), Convert.ToDouble(reader[1]), Convert.ToDouble(reader[2]), Convert.ToDouble(reader[3]), Convert.ToDouble(reader[4]), Convert.ToDateTime(reader[5])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read individual farm production from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Write CO2 Emission into database
        /// </summary>
        /// <param name="nonRenewableEmissionValue">emission value without renewable generators</param>
        /// <param name="withRenewableEmissionValue">emission value with renewable generators</param>
        /// <param name="measurementTime">time of measurement</param>
        /// <returns>return true if success</returns>
        public bool WriteCO2EmissionIntoDb(float nonRenewableEmissionValue, float withRenewableEmissionValue, DateTime measurementTime)
        {
            bool success = true;

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertCO2Emission", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@nonRenewable", SqlDbType.Float).Value = nonRenewableEmissionValue;
                        cmd.Parameters.Add("@renewable", SqlDbType.Float).Value = withRenewableEmissionValue;
                        cmd.Parameters.Add("@timeOfMeasurement", SqlDbType.DateTime).Value = measurementTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    success = false;
                    string message = string.Format("Failed to insert co2 emission into database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return success;
        }

        /// <summary>
        /// Read CO2 emission values from database
        /// </summary>
        /// <param name="startTime">start time of period</param>
        /// <param name="endTime">end time of period</param>
        /// <returns>returns list of pair values</returns>
        public List<Tuple<double, double, DateTime>> ReadCO2EmissionFromDb(DateTime startTime, DateTime endTime)
        {
            List<Tuple<double, double, DateTime>> retVal = new List<Tuple<double, double, DateTime>>();

            using (SqlConnection connection = new SqlConnection(Config.Instance.ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM CO2Emission WHERE (MeasurementTime BETWEEN @startTime AND @endTime)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retVal.Add(new Tuple<double, double, DateTime>(Convert.ToDouble(reader[1]), Convert.ToDouble(reader[2]), Convert.ToDateTime(reader[3])));
                        }
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    string message = string.Format("Failed read CO2 emission from database. {0}", e.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    Console.WriteLine(message);
                }
            }

            return retVal;
        }

        #region Fill and Clear data

        /// <summary>
        /// Fills data
        /// </summary>
        private void FillData()
        {
            lock (lockObj)
            {
                synchronousMachines.Clear();
                fuels.Clear();
                energyConsumers.Clear();

                foreach (ResourceDescription rd in internalSynchMachines)
                {
                    SynchronousMachine sm = ResourcesDescriptionConverter.ConvertTo<SynchronousMachine>(rd);
                    synchronousMachines.Add(sm.GlobalId, sm);
                }

                FillGeneratorCurves();

                foreach (ResourceDescription rd in internalEmsFuels)
                {
                    EMSFuel emsf = ResourcesDescriptionConverter.ConvertTo<EMSFuel>(rd);
                    fuels.Add(emsf.GlobalId, emsf);
                }

                foreach (ResourceDescription rd in internalEnergyConsumers)
                {
                    EnergyConsumer ec = ResourcesDescriptionConverter.ConvertTo<EnergyConsumer>(rd);
                    energyConsumers.Add(ec.GlobalId, ec);
                }
            }
        }

        /// <summary>
        /// Clears data
        /// </summary>
        private void ClearData()
        {
            lock (lockObj)
            {
                synchronousMachines.Clear();
                fuels.Clear();
                energyConsumers.Clear();
                generatorCurves.Clear();
            }
        }

        #endregion Fill and Clear data

        #region IntegrityUpdate

        /// <summary>
        /// Implements integrity update logic for scada cr component
        /// </summary>
        /// <returns></returns>
        public bool InitiateIntegrityUpdate()
        {
            //lock (lockObj)
            {
                ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

                List<ModelCode> properties = new List<ModelCode>(10);
                ModelCode modelCodeEmsFuel = ModelCode.EMSFUEL;
                ModelCode modelCodeSynchronousMachine = ModelCode.SYNCHRONOUSMACHINE;
                ModelCode modelCodeEnergyConsumer = ModelCode.ENERGYCONSUMER;

                int iteratorId = 0;
                int resourcesLeft = 0;
                int numberOfResources = 2;
                string message = string.Empty;

                List<ResourceDescription> retList = new List<ResourceDescription>(5);

                #region getting SynchronousMachine

                try
                {
                    // first get all synchronous machines from NMS
                    properties = modelResourcesDesc.GetAllPropertyIds(modelCodeSynchronousMachine);

                    iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeSynchronousMachine, properties);
                    resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                    while (resourcesLeft > 0)
                    {
                        List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                        retList.AddRange(rds);
                        resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                    }
                    NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);

                    // add synchronous machines to internal collection
                    internalSynchMachines.Clear();
                    internalSynchMachines.AddRange(retList);
                }
                catch (Exception e)
                {
                    message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCodeSynchronousMachine, e.Message);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);

                    Console.WriteLine("Trying again...");
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "Trying again...");
                    NetworkModelGDAProxy.Instance = null;
                    Thread.Sleep(1000);
                    InitiateIntegrityUpdate();
                }

                message = string.Format("Integrity update: Number of {0} values: {1}", modelCodeSynchronousMachine.ToString(), internalSynchMachines.Count.ToString());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCodeSynchronousMachine.ToString(), internalSynchMachines.Count.ToString());

                // clear retList for getting new model from NMS
                retList.Clear();

                properties.Clear();
                iteratorId = 0;
                resourcesLeft = 0;

                #endregion getting SynchronousMachine

                #region getting EMSFuel

                try
                {
                    // second get all ems fuels from NMS
                    properties = modelResourcesDesc.GetAllPropertyIds(modelCodeEmsFuel);

                    iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeEmsFuel, properties);
                    resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                    while (resourcesLeft > 0)
                    {
                        List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                        retList.AddRange(rds);
                        resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                    }
                    NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);

                    // add ems fuels to internal collection
                    internalEmsFuels.Clear();
                    internalEmsFuels.AddRange(retList);
                }
                catch (Exception e)
                {
                    message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCodeEmsFuel, e.Message);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    return false;
                }

                message = string.Format("Integrity update: Number of {0} values: {1}", modelCodeEmsFuel.ToString(), internalEmsFuels.Count.ToString());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCodeEmsFuel.ToString(), internalEmsFuels.Count.ToString());

                // clear retList for getting new model from NMS
                retList.Clear();

                #endregion getting EMSFuel

                #region getting EnergyConsumer

                try
                {
                    // third get all enenrgy consumers from NMS
                    properties = modelResourcesDesc.GetAllPropertyIds(modelCodeEnergyConsumer);

                    iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeEnergyConsumer, properties);
                    resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                    while (resourcesLeft > 0)
                    {
                        List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                        retList.AddRange(rds);
                        resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                    }
                    NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);

                    // add energy consumer to internal collection
                    internalEnergyConsumers.Clear();
                    internalEnergyConsumers.AddRange(retList);
                }
                catch (Exception e)
                {
                    message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCodeEnergyConsumer, e.Message);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    return false;
                }

                message = string.Format("Integrity update: Number of {0} values: {1}", modelCodeEnergyConsumer.ToString(), internalEnergyConsumers.Count.ToString());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCodeEnergyConsumer.ToString(), internalEnergyConsumers.Count.ToString());

                // clear retList
                retList.Clear();

                #endregion getting EnergyConsumer

                FillData();
                return true;
            }
        }

        #endregion IntegrityUpdate

        #region Transaction

        public UpdateResult Prepare(ref Delta delta)
        {
            try
            {
                transactionCallback = OperationContext.Current.GetCallbackChannel<ITransactionCallback>();
                updateResult = new UpdateResult();

                // napravi kopiju od originala
                internalEmsFuelsCopy.Clear();
                foreach (ResourceDescription rd in internalEmsFuels)
                {
                    internalEmsFuelsCopy.Add(rd.Clone() as ResourceDescription);
                }

                internalSynchMachinesCopy.Clear();
                foreach (ResourceDescription rd in internalSynchMachines)
                {
                    internalSynchMachinesCopy.Add(rd.Clone() as ResourceDescription);
                }

                internalEnergyConsumersCopy.Clear();
                foreach (ResourceDescription rd in internalEnergyConsumers)
                {
                    internalEnergyConsumersCopy.Add(rd.Clone() as ResourceDescription);
                }

                foreach (ResourceDescription rd in delta.InsertOperations)
                {
                    foreach (Property prop in rd.Properties)
                    {
                        if (ModelCodeHelper.GetTypeFromModelCode(prop.Id).Equals(EMSType.EMSFUEL))
                        {
                            internalEmsFuelsCopy.Add(rd);
                            break;
                        }
                        else if (ModelCodeHelper.GetTypeFromModelCode(prop.Id).Equals(EMSType.SYNCHRONOUSMACHINE))
                        {
                            internalSynchMachinesCopy.Add(rd);
                            break;
                        }
                        else if (ModelCodeHelper.GetTypeFromModelCode(prop.Id).Equals(EMSType.ENERGYCONSUMER))
                        {
                            internalEnergyConsumersCopy.Add(rd);
                            break;
                        }
                    }
                }

                foreach (ResourceDescription rd in delta.UpdateOperations)
                {
                    foreach (Property prop in rd.Properties)
                    {
                        if (ModelCodeHelper.GetTypeFromModelCode(prop.Id).Equals(EMSType.EMSFUEL))
                        {
                            foreach (ResourceDescription res in internalEmsFuelsCopy)
                            {
                                if (rd.Id.Equals(res.Id))
                                {
                                    foreach (Property p in res.Properties)
                                    {
                                        if (prop.Id.Equals(p.Id))
                                        {
                                            p.PropertyValue = prop.PropertyValue;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (ModelCodeHelper.GetTypeFromModelCode(prop.Id).Equals(EMSType.SYNCHRONOUSMACHINE))
                        {
                            foreach (ResourceDescription res in internalSynchMachinesCopy)
                            {
                                if (rd.Id.Equals(res.Id))
                                {
                                    foreach (Property p in res.Properties)
                                    {
                                        if (prop.Id.Equals(p.Id))
                                        {
                                            p.PropertyValue = prop.PropertyValue;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (ModelCodeHelper.GetTypeFromModelCode(prop.Id).Equals(EMSType.ENERGYCONSUMER))
                        {
                            foreach (ResourceDescription res in internalEnergyConsumersCopy)
                            {
                                if (rd.Id.Equals(res.Id))
                                {
                                    foreach (Property p in res.Properties)
                                    {
                                        if (prop.Id.Equals(p.Id))
                                        {
                                            p.PropertyValue = prop.PropertyValue;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                updateResult.Message = "CE Transaction Prepare finished.";
                updateResult.Result = ResultType.Succeeded;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "CETransaction Prepare finished successfully.");
                transactionCallback.Response("OK");
            }
            catch (Exception e)
            {
                updateResult.Message = "CE Transaction Prepare finished.";
                updateResult.Result = ResultType.Failed;
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "CE Transaction Prepare failed. Message: {0}", e.Message);
                transactionCallback.Response("ERROR");
            }

            return updateResult;
        }

        public bool Commit()
        {
            try
            {
                internalSynchMachines.Clear();
                foreach (ResourceDescription rd in internalSynchMachinesCopy)
                {
                    internalSynchMachines.Add(rd.Clone() as ResourceDescription);
                }
                internalSynchMachinesCopy.Clear();

                internalEmsFuels.Clear();
                foreach (ResourceDescription rd in internalEmsFuelsCopy)
                {
                    internalEmsFuels.Add(rd.Clone() as ResourceDescription);
                }
                internalEmsFuelsCopy.Clear();

                internalEnergyConsumers.Clear();
                foreach (ResourceDescription rd in internalEnergyConsumersCopy)
                {
                    internalEnergyConsumers.Add(rd.Clone() as ResourceDescription);
                }
                internalEnergyConsumersCopy.Clear();

                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "CE Transaction: Commit phase successfully finished.");
                Console.WriteLine("Number of EMSFuels values: {0}", internalEmsFuels.Count);
                Console.WriteLine("Number of SynchronousMachines values: {0}", internalSynchMachines.Count);
                Console.WriteLine("Number of Energy Consumers values: {0}", internalSynchMachines.Count);

                FillData();
                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "CE Transaction: Failed to Commit changes. Message: {0}", e.Message);
                return false;
            }
        }

        public bool Rollback()
        {
            try
            {
                internalEmsFuelsCopy.Clear();
                internalSynchMachinesCopy.Clear();
                internalEnergyConsumersCopy.Clear();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "CE Transaction rollback successfully finished!");
                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "CE Transaction rollback error. Message: {0}", e.Message);
                return false;
            }
        }

        #endregion Transaction
    }
}