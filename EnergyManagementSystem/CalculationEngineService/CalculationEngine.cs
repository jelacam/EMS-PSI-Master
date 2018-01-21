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
    using System.Linq;
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

    /// <summary>
    /// Class for CalculationEngine
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CalculationEngine : ITransactionContract
    {
        #region Fields

        private SolverContext context;

        private float totalCostLinear;
        private float totalCostGeneric;

        private List<OptimisationModel> loms;
        private List<MeasurementUnit> helpMU = new List<MeasurementUnit>();
        private float powerOfConsumers = 0;

        private static List<ResourceDescription> internalSynchMachines;
        private static List<ResourceDescription> internalSynchMachinesCopy;
        private static List<ResourceDescription> internalEmsFuels;
        private static List<ResourceDescription> internalEmsFuelsCopy;
        private static List<ResourceDescription> internalEnergyConsumers;
        private static List<ResourceDescription> internalEnergyConsumersCopy;

        private IDictionary<long, SynchronousMachine> synchronousMachines;
        private IDictionary<long, EMSFuel> fuels;
        private IDictionary<long, EnergyConsumer> energyConsumers;

        private object lockObj;

        private PublisherService publisher = null;
        private ITransactionCallback transactionCallback;
        private UpdateResult updateResult;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationEngine" /> class
        /// </summary>
        public CalculationEngine()
        {
            publisher = new PublisherService();

            totalCostLinear = 0;
            totalCostGeneric = 0;

            lockObj = new object();
            context = SolverContext.GetContext();
            loms = new List<OptimisationModel>();
            powerOfConsumers = 0;

            synchronousMachines = new Dictionary<long, SynchronousMachine>();
            fuels = new Dictionary<long, EMSFuel>();
            energyConsumers = new Dictionary<long, EnergyConsumer>();

            internalSynchMachines = new List<ResourceDescription>(5);
            internalSynchMachinesCopy = new List<ResourceDescription>(5);
            internalEmsFuels = new List<ResourceDescription>(5);
            internalEmsFuelsCopy = new List<ResourceDescription>(5);
            internalEnergyConsumers = new List<ResourceDescription>(5);
            internalEnergyConsumersCopy = new List<ResourceDescription>(5);
        }

        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measEnergyConsumers">list of measurements for Energy Consumers</param>
        /// <param name="measGenerators">list of measurements for Generators</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool Optimize(List<MeasurementUnit> measEnergyConsumers, List<MeasurementUnit> measGenerators)
        {
            bool result = false;

            PublishConsumersToUI(measEnergyConsumers);

            Dictionary<long, OptimisationModel> optModelMap = GetOptimizationModelMap(measGenerators);
            powerOfConsumers = CalculationHelper.CalculateConsumption(measEnergyConsumers);

            List<MeasurementUnit> measurementsOptimizedGA = null;
            List<MeasurementUnit> measurementsOptimizedLinear = null;

            Console.WriteLine("CE: Optimize {0}\n", powerOfConsumers);
            // measurementsOptimizedGA = DoGeneticAlgorithm(optModelMap, powerOfConsumers);

            measurementsOptimizedLinear = LinearOptimization(measGenerators, powerOfConsumers);

            List<MeasurementUnit> measurementsOptimized = CalculationHelper.ChooseBetterOptimization(measurementsOptimizedGA, measurementsOptimizedLinear, optModelMap);

            if (measurementsOptimized != null && measurementsOptimized.Count > 0)
            {
                if (InsertMeasurementsIntoDb(measurementsOptimized))
                {
                    Console.WriteLine("Inserted {0} Measurement(s) into history database.", measurementsOptimized.Count);
                }

                PublishGeneratorsToUI(ref measurementsOptimized);

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

        private List<MeasurementUnit> DoGeneticAlgorithm(Dictionary<long, OptimisationModel> optModelMap, float necessaryEnergy)
        {
            try
            {
                GAOptimization gao = new GAOptimization(necessaryEnergy, optModelMap);

                List<MeasurementUnit> optimized = gao.StartAlgorithmWithReturn();
                return optimized;
            }
            catch (Exception e)
            {
                throw new Exception("[Mehtod = DoGeneticAlgorithm] Exception = " + e.Message);
            }
        }

        private Dictionary<long, OptimisationModel> GetOptimizationModelMap(List<MeasurementUnit> measGenerators)
        {
            Dictionary<long, OptimisationModel> optModelMap = new Dictionary<long, OptimisationModel>();

            foreach (var measUnit in measGenerators)
            {
                if (synchronousMachines.ContainsKey(measUnit.Gid))
                {
                    SynchronousMachine sm = synchronousMachines[measUnit.Gid];
                    EMSFuel emsf = fuels[sm.Fuel];
                    OptimisationModel om = new OptimisationModel(sm, emsf, measUnit);

                    if (emsf.FuelType == EmsFuelType.wind || emsf.FuelType == EmsFuelType.solar)
                    {
                        continue;
                    }
                    optModelMap.Add(om.GlobalId, om);
                }
            }

            return optModelMap;
        }

        private void PublishGeneratorsToUI(ref List<MeasurementUnit> measurementsFromGenerators)
        {
            List<MeasurementUI> measListUI = new List<MeasurementUI>();
            foreach (var meas in measurementsFromGenerators)
            {
                MeasurementUI measUI = new MeasurementUI();
                measUI.Gid = meas.Gid;
                measUI.CurrentValue = meas.OptimizedLinear;
                measUI.TimeStamp = meas.TimeStamp;

                // setovanje current vrednosti na vrednost optimizovanu linearnim algoritmom
                meas.CurrentValue = meas.OptimizedLinear;

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
                            cmd.Parameters.Add("@valueMeasurement", SqlDbType.Float).Value = mu.OptimizedLinear;
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

        #region Checking alarms

        /// <summary>
        /// Checking for alarms on optimized value
        /// </summary>
        /// <param name="value">value to check</param>
        /// <param name="minOptimized">low limit</param>
        /// <param name="maxOptimized">high limit</param>
        /// <param name="gid">gid of measurement</param>
        /// <returns>True if alarm occures</returns>
        private bool CheckForOptimizedAlarms(float value, float minOptimized, float maxOptimized, long gid)
        {
            bool retVal = false;
            if (value < minOptimized)
            {
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low optimized limit on gid: {0}", gid);
                Console.WriteLine("Alarm on low optimized limit on gid: {0}", gid);
            }

            if (value > maxOptimized)
            {
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high optimized limit on gid: {0} - Sinal value: {1}", gid, value);
                Console.WriteLine("Alarm on high optimized limit on gid: {0}  - Sinal value: {1}", gid, value);
            }

            return retVal;
        }

        #endregion Checking alarms

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

                helpMU.Clear();

                loms.Clear();

                powerOfConsumers = 0;
            }
        }

        #endregion Fill and Clear data

        private void HelpFunction()
        {
            lock (lockObj)
            {
                synchronousMachines.Clear();
                fuels.Clear();
                energyConsumers.Clear();

                EMSFuel f1 = new EMSFuel(1);
                f1.FuelType = EmsFuelType.oli;
                f1.UnitPrice = 20;
                fuels.Add(f1.GlobalId, f1);
                EMSFuel f2 = new EMSFuel(2);
                f2.FuelType = EmsFuelType.hydro;
                f2.UnitPrice = 15;
                fuels.Add(f2.GlobalId, f2);

                SynchronousMachine sm1 = new SynchronousMachine(10);
                sm1.Active = true;
                sm1.Fuel = f1.GlobalId;
                sm1.MaxQ = 100;
                sm1.MinQ = 10;
                sm1.RatedS = 50;
                synchronousMachines.Add(sm1.GlobalId, sm1);
                SynchronousMachine sm2 = new SynchronousMachine(20);
                sm2.Active = true;
                sm2.Fuel = f2.GlobalId;
                sm2.MaxQ = 200;
                sm2.MinQ = 50;
                sm2.RatedS = 100;
                synchronousMachines.Add(sm2.GlobalId, sm2);
                SynchronousMachine sm3 = new SynchronousMachine(30);
                sm3.Active = false;
                sm3.Fuel = f2.GlobalId;
                sm3.MaxQ = 150;
                sm3.MinQ = 70;
                sm3.RatedS = 100;
                synchronousMachines.Add(sm3.GlobalId, sm3);

                EnergyConsumer ec1 = new EnergyConsumer(100);
                ec1.PFixed = 100;
                ec1.QFixed = 100;
                energyConsumers.Add(ec1.GlobalId, ec1);
                EnergyConsumer ec2 = new EnergyConsumer(200);
                ec2.PFixed = 200;
                ec2.QFixed = 20;
                energyConsumers.Add(ec2.GlobalId, ec2);

                MeasurementUnit mu1 = new MeasurementUnit();
                mu1.CurrentValue = 30;
                mu1.Gid = sm1.GlobalId;
                mu1.MaxValue = sm1.MaxQ;
                mu1.MinValue = sm1.MinQ;
                mu1.OptimizedLinear = 0;
                mu1.OptimizedGeneric = 0;
                helpMU.Add(mu1);
                MeasurementUnit mu2 = new MeasurementUnit();
                mu2.CurrentValue = 130;
                mu2.Gid = sm2.GlobalId;
                mu2.MaxValue = sm2.MaxQ;
                mu2.MinValue = sm2.MinQ;
                mu2.OptimizedLinear = 0;
                mu2.OptimizedGeneric = 0;
                helpMU.Add(mu2);
                MeasurementUnit mu3 = new MeasurementUnit();
                mu3.CurrentValue = 100;
                mu3.Gid = sm3.GlobalId;
                mu3.MaxValue = sm3.MaxQ;
                mu3.MinValue = sm3.MinQ;
                mu3.OptimizedLinear = 0;
                mu3.OptimizedGeneric = 0;
                helpMU.Add(mu3);
                MeasurementUnit mu4 = new MeasurementUnit();
                mu4.CurrentValue = 160;
                mu4.Gid = ec1.GlobalId;
                helpMU.Add(mu4);
                MeasurementUnit mu5 = new MeasurementUnit();
                mu5.CurrentValue = 105;
                mu5.Gid = ec2.GlobalId;
                helpMU.Add(mu5);
            }
        }

        private List<MeasurementUnit> LinearOptimization(List<MeasurementUnit> measurements, float consumption)
        {
            lock (lockObj)
            {
                if (measurements.Count() > 0)
                {
                    Model model = context.CreateModel();

                    Dictionary<long, Decision> decisions = new Dictionary<long, Decision>();
                    loms.Clear();
                    float maxProduction = 0;
                    float minProduction = 0;

                    for (int i = 0; i < measurements.Count(); i++)
                    {
                        if (synchronousMachines.ContainsKey(measurements[i].Gid))
                        {
                            SynchronousMachine sm = synchronousMachines[measurements[i].Gid];
                            EMSFuel emsf = fuels[sm.Fuel];
                            OptimisationModel om = new OptimisationModel(sm, emsf, measurements[i]);
                            loms.Add(om);

                            Decision d = new Decision(Domain.RealNonnegative, "d" + om.GlobalId.ToString());
                            model.AddDecision(d);
                            decisions.Add(om.GlobalId, d);

                            if (om.Managable != 0)
                            {
                                maxProduction += om.MaxPower;
                                minProduction += om.MinPower;
                            }
                        }
                    }

                    if (loms.Count() > 0)
                    {
                        if (0 <= consumption && consumption <= maxProduction)
                        {
                            Decision help;
                            string goal = "";
                            string limit = "limit";
                            string production = consumption.ToString() + "<=";

                            for (int i = 0; i < loms.Count; i++)
                            {
                                help = decisions[loms[i].GlobalId];

                                if (loms[i].Managable == 0)
                                {
                                    Term tLimit = 0 <= help <= 0;
                                    model.AddConstraint(limit + loms[i].GlobalId, tLimit);
                                }
                                else
                                {
                                    Term tLimit = loms[i].MinPower <= help <= loms[i].MaxPower;
                                    model.AddConstraint(limit + loms[i].GlobalId, tLimit);
                                }

                                production += help.ToString() + "+";

                                goal += help.ToString() + "*" + loms[i].Price.ToString() + "+";
                            }

                            production = production.Substring(0, production.Length - 1);
                            production += "<=" + maxProduction.ToString();
                            model.AddConstraint("production", production);

                            goal = goal.Substring(0, goal.Length - 1);
                            model.AddGoal("cost", GoalKind.Minimize, goal);

                            Solution solution = context.Solve(new SimplexDirective());
                            Report report = solution.GetReport();
                            Console.Write("{0}", report);

                            totalCostLinear = float.Parse(model.Goals.FirstOrDefault().ToDouble().ToString());

                            string name = "";
                            foreach (var item in model.Decisions)
                            {
                                for (int i = 0; i < measurements.Count(); i++)
                                {
                                    name = item.Name.Substring(1);
                                    if (Int64.Parse(name) == measurements[i].Gid)
                                    {
                                        measurements[i].OptimizedLinear = float.Parse(item.ToDouble().ToString());
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    context.ClearModel();
                }
            }

            return measurements;
        }

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
                    return false;
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