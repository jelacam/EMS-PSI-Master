//-----------------------------------------------------------------------
// <copyright file="CalculationEngine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.ServiceModel;
    using CommonMeasurement;
    using EMS.Common;
    using EMS.ServiceContracts;
    using PubSub;
    using Microsoft.SolverFoundation.Services;
    using NetworkModelService.DataModel.Wires;
    using NetworkModelService.DataModel.Production;
    using GeneticAlgorithm;
    using Helpers;
    using LinearAlgorithm;
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
		private float emissionCO2 = 0;

        private SynchronousMachineCurveModels generatorCharacteristics = new SynchronousMachineCurveModels();
        private Dictionary<string, SynchronousMachineCurveModel> generatorCurves;

        public SynchronousMachineCurveModels GeneratorCharacteristics
        {
            get { return generatorCharacteristics; }
            set { generatorCharacteristics = value; }
        }

        #endregion Fields

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

        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measEnergyConsumers">list of measurements for Energy Consumers</param>
        /// <param name="measGenerators">list of measurements for Generators</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool Optimize(List<MeasurementUnit> measEnergyConsumers, List<MeasurementUnit> measGenerators, float windSpeed, float sunlight)
        {
            bool result = false;

            PublishConsumersToUI(measEnergyConsumers);

            Dictionary<long, OptimisationModel> optModelMap = GetOptimizationModelMap(measGenerators, windSpeed, sunlight);
            float powerOfConsumers = CalculationHelper.CalculateConsumption(measEnergyConsumers);

            List<MeasurementUnit> measurementsOptimized = DoOptimization(optModelMap, powerOfConsumers, windSpeed, sunlight);

            if (measurementsOptimized != null && measurementsOptimized.Count > 0)
            {
                if (InsertMeasurementsIntoDb(measurementsOptimized))
                {
                    Console.WriteLine("Inserted {0} Measurement(s) into history database.", measurementsOptimized.Count);
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

        private List<MeasurementUnit> DoOptimization(Dictionary<long, OptimisationModel> optModelMap, float powerOfConsumers, float windSpeed, float sunlight)
        {
            try
            {
                Dictionary<long, OptimisationModel> optModelMapOptimizied = null;
                float totalCost = -1;
                if (PublisherService.OptimizationType == OptimizationType.Genetic)
                {
                    GAOptimization gao = new GAOptimization(powerOfConsumers, optModelMap);
                    optModelMapOptimizied = gao.StartAlgorithmWithReturn();
                    totalCost = gao.TotalCost;
                }
                else if (PublisherService.OptimizationType == OptimizationType.Linear)
                {
                    LinearOptimization linearAlgorithm = new LinearOptimization(minProduction, maxProduction);
                    optModelMapOptimizied = linearAlgorithm.Start(optModelMap, powerOfConsumers);
                    totalCost = linearAlgorithm.TotalCost; // ukupna cena linearne optimizacije
                    profit = linearAlgorithm.Profit; // koliko je $ ustedjeno koriscenjem vetrogeneratora
                    windProductionPct = linearAlgorithm.WindOptimizedPctLinear; // procenat proizvodnje vetrogeneratora u odnosu na ukupnu proizvodnju
                    windProductionkW = linearAlgorithm.WindOptimizedLinear; // kW proizvodnje vetrogeneratora u ukupnoj proizvodnji
					emissionCO2 = linearAlgorithm.CO2Emission; // smanjenje CO2 emisije izrazeno u tonama
                }
                else
                {
                    return DoNotOptimized(optModelMap, powerOfConsumers);
                }
                Console.WriteLine("CE: Optimize {0}kW", powerOfConsumers);
                Console.WriteLine("CE: TotalCost {0}$\n", totalCost);
                return OptModelMapToListMeasUI(optModelMapOptimizied, PublisherService.OptimizationType);
            }
            catch (Exception e)
            {
                throw new Exception("[Mehtod = DoGeneticAlgorithm] Exception = " + e.Message);
            }
        }

        private List<MeasurementUnit> DoNotOptimized(Dictionary<long, OptimisationModel> optModelMap, float powerOfConsumers)
        {
            List<MeasurementUnit> retList = new List<MeasurementUnit>();
            foreach (OptimisationModel optModel in optModelMap.Values)
            {
                float power = 0;
                if (powerOfConsumers >= optModel.MaxPower)
                {
                    power = optModel.MaxPower;
                    powerOfConsumers -= power;
                }
                else
                {
                    power = powerOfConsumers;
                    powerOfConsumers = 0;
                }

                retList.Add(new MeasurementUnit()
                {
                    CurrentValue = power,
                    Gid = optModel.GlobalId,
                    MaxValue = optModel.MaxPower,
                    MinValue = optModel.MinPower,
                    OptimizationType = OptimizationType.None,
                });
            }

            if (powerOfConsumers > 0)
            {
                Console.WriteLine("[Method = DoNotOptimized] Nesto ne valja ovde");
            }

            return retList;
        }

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
                    CurrentValue = currValue
                });
            }

            return retList;
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
                            if (om.Managable != 0)
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
                        generatorCurves.Add(item.Mrid, null);
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

        #region Database methods

        /// <summary>
        /// Insert data into history db
        /// </summary>
        /// <param name="measurements">List of measurements</param>
        /// <returns>Success</returns>
        private bool InsertMeasurementsIntoDb(List<MeasurementUnit> measurements)
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

        #endregion Database methods

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
            lock (lockObj)
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

                // getting SynchronousMachine
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

                // getting EMSFuel
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

                // getting EnergyConsumer
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
                                if(rd.Id.Equals(res.Id))
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

        public bool Commit(Delta delta)
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