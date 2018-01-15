//-----------------------------------------------------------------------
// <copyright file="CalculationEngine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using CommonMeasurement;
    using EMS.Common;
    using EMS.ServiceContracts;
    using PubSub;
    using Microsoft.SolverFoundation.Common;
    using Microsoft.SolverFoundation.Services;
    using NetworkModelService.DataModel.Wires;
    using NetworkModelService.DataModel.Production;
    using System.ServiceModel;
    using System.Linq;

    /// <summary>
    /// Class for CalculationEngine
    /// </summary>
    public class CalculationEngine : ITransactionContract
    {
        #region Fields

        private float powerOfConsumers = 230;
        private float totalCostLinear = 0;

        SolverContext context = SolverContext.GetContext();

        private List<OptimisationModel> loms;
        private List<MeasurementUnit> helpMU = new List<MeasurementUnit>();
        private float minProduction = 0;
        private float maxProduction = 0;

        private static List<ResourceDescription> internalSynchMachines = new List<ResourceDescription>(5);
        private static List<ResourceDescription> internalEmsFuels = new List<ResourceDescription>(5);
        private static List<ResourceDescription> internalEnergyConsumers = new List<ResourceDescription>(5);


        private static List<ResourceDescription> internalSynchMachinesCopy = new List<ResourceDescription>(5);
        private static List<ResourceDescription> internalEmsFuelsCopy = new List<ResourceDescription>(5);
        private static List<ResourceDescription> internalEnergyConsumersCopy = new List<ResourceDescription>(5);


        private IDictionary<long, SynchronousMachine> dSynchronousMachine;
        private IDictionary<long, EMSFuel> dEMSFuel;
        private IDictionary<long, EnergyConsumer> energyConsumers;

        private object lockObj = new object();


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
            //internalEmsFuels = new List<ResourceDescription>(5);
            //internalSynchMachines = new List<ResourceDescription>(5);
            //internalEmsFuelsCopy = new List<ResourceDescription>(5);
            //internalSynchMachinesCopy = new List<ResourceDescription>(5);

        }

        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measurements">list of measurements which should be optimized</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool Optimize(List<MeasurementUnit> measurements)
        {
            
            // List<MeasurementUnit> l = this.LinearOptimization(measurements);
            IEnumerable<MeasurementUnit> measurementFromEnergyConsumers = SeparateEnergyConsumers(measurements);
            IEnumerable<MeasurementUnit> measurementFromGenerators = SeparateGenerators(measurements);
           // this.HelpFunction();
           // List<MeasurementUnit> l = this.LinearOptimization(helpMU);

            bool alarmOptimized = true;
            bool result = false;
            if (measurements != null)
            {
                if (measurements.Count > 0)
                {
                    Console.WriteLine("CE: Optimize");
                    for (int i = 0; i < measurements.Count; i++)
                    {
                        //measurements[i].CurrentValue = measurements[i].CurrentValue * 2;

                        alarmOptimized = this.CheckForOptimizedAlarms(measurements[i].CurrentValue, measurements[i].MinValue, measurements[i].MaxValue, measurements[i].Gid);
                        if (alarmOptimized == false)
                        {
                            CommonTrace.WriteTrace(CommonTrace.TraceInfo, "gid: {0} value: {1}", measurements[i].Gid, measurements[i].CurrentValue);
                            Console.WriteLine("gid: {0} value: {1}", measurements[i].Gid, measurements[i].CurrentValue);
                        }
                        else
                        {
                            CommonTrace.WriteTrace(CommonTrace.TraceInfo, "gid: {0} value: {1} ALARM!", measurements[i].Gid, measurements[i].CurrentValue);
                            Console.WriteLine("gid: {0} value: {1} ALARM!", measurements[i].Gid, measurements[i].CurrentValue);
                        }

                        MeasurementUI measUI = new MeasurementUI()
                        {
                            Gid = measurements[i].Gid,
                            AlarmType = alarmOptimized ? "Alarm while optimizing" : string.Empty,
                            MeasurementValue = measurements[i].CurrentValue
                        };

                        try
                        {
                            publisher.PublishOptimizationResults(measUI);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    if (alarmOptimized == false)
                    {
                        result = true;
                    }

                    try
                    {
                        if (ScadaCMDProxy.Instance.SendDataToSimulator(measurements))
                        {
                            CommonTrace.WriteTrace(CommonTrace.TraceInfo, "CE sent {0} optimized MeasurementUnit(s) to SCADACommanding.", measurements.Count);
                            Console.WriteLine("CE sent {0} optimized MeasurementUnit(s) to SCADACommanding.", measurements.Count);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                        CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                    }
                }
            }

            return result;
        }

        private IEnumerable<MeasurementUnit> SeparateGenerators(List<MeasurementUnit> measurements)
        {
            return measurements.Where(x => dSynchronousMachine.ContainsKey(x.Gid));
        }

        private IEnumerable<MeasurementUnit> SeparateEnergyConsumers(List<MeasurementUnit> measurements)
        {
            return measurements.Where(x => energyConsumers.ContainsKey(x.Gid));
        }
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

        #region Necessary data

        /// <summary>
        /// Fills data
        /// </summary>
        private void FillData()
        {
            lock (lockObj)
            {
                this.dSynchronousMachine = new Dictionary<long, SynchronousMachine>();
                this.dEMSFuel = new Dictionary<long, EMSFuel>();
                this.energyConsumers = new Dictionary<long, EnergyConsumer>();

                foreach (ResourceDescription rd in internalSynchMachines)
                {
                    SynchronousMachine sm = ResourcesDescriptionConverter.ConvertTo<SynchronousMachine>(rd);
                    this.dSynchronousMachine.Add(sm.GlobalId, sm);
                }

                foreach (ResourceDescription rd in internalEmsFuels)
                {
                    EMSFuel emsf = ResourcesDescriptionConverter.ConvertTo<EMSFuel>(rd);
                    this.dEMSFuel.Add(emsf.GlobalId, emsf);
                }

                foreach (ResourceDescription rd in internalEnergyConsumers)
                {
                    EnergyConsumer ec = ResourcesDescriptionConverter.ConvertTo<EnergyConsumer>(rd);
                    this.energyConsumers.Add(ec.GlobalId, ec);
                }

                this.loms = new List<OptimisationModel>();
            }
        }

        /// <summary>
        /// Clears data
        /// </summary>
        private void ClearData()
        {
            lock (lockObj)
            {
                this.dSynchronousMachine.Clear();
                this.dEMSFuel.Clear();
                this.helpMU.Clear();
                this.loms.Clear();
                this.minProduction = 0;
                this.maxProduction = 0;
            }
        }

        #endregion Necessary data

        private void HelpFunction()
        {
            EMSFuel f1 = new EMSFuel(10);
            f1.FuelType = EmsFuelType.oli;
            f1.UnitPrice = 20;
            this.dEMSFuel.Add(f1.GlobalId, f1);
            EMSFuel f2 = new EMSFuel(20);
            f2.FuelType = EmsFuelType.hydro;
            f2.UnitPrice = 15;
            this.dEMSFuel.Add(f2.GlobalId, f2);

            SynchronousMachine sm1 = new SynchronousMachine(1);
            sm1.Active = true;
            sm1.Fuel = 10;
            sm1.MaxQ = 100;
            sm1.MinQ = 10;
            sm1.RatedS = 50;
            this.dSynchronousMachine.Add(sm1.GlobalId, sm1);
            this.minProduction += sm1.MinQ;
            this.maxProduction += sm1.MaxQ;
            SynchronousMachine sm2 = new SynchronousMachine(2);
            sm2.Active = true;
            sm2.Fuel = 20;
            sm2.MaxQ = 200;
            sm2.MinQ = 50;
            sm2.RatedS = 100;
            this.dSynchronousMachine.Add(sm2.GlobalId, sm2);
            this.minProduction += sm2.MinQ;
            this.maxProduction += sm2.MaxQ;

            MeasurementUnit mu1 = new MeasurementUnit();
            mu1.CurrentValue = 30;
            mu1.Gid = sm1.GlobalId;
            mu1.MaxValue = sm1.MaxQ;
            mu1.MinValue = sm1.MinQ;
            mu1.OptimizedLinear = 0;
            mu1.OptimizedGeneric = 0;
            this.helpMU.Add(mu1);
            MeasurementUnit mu2 = new MeasurementUnit();
            mu2.CurrentValue = 130;
            mu2.Gid = sm2.GlobalId;
            mu2.MaxValue = sm2.MaxQ;
            mu2.MinValue = sm2.MinQ;
            mu2.OptimizedLinear = 0;
            mu2.OptimizedGeneric = 0;
            this.helpMU.Add(mu2);
        }

        private List<MeasurementUnit> LinearOptimization(List<MeasurementUnit> measurements)
        {
            lock (lockObj)
            {
                if (measurements.Count > 0)
                {
                    Model model = context.CreateModel();

                    Dictionary<long, Decision> decisions = new Dictionary<long, Decision>();
                    List<Constraint> constraints = new List<Constraint>();

                    for (int i = 0; i < measurements.Count; i++)
                    {
                        if (dSynchronousMachine.ContainsKey(measurements[i].Gid))
                        {
                            SynchronousMachine sm = dSynchronousMachine[measurements[i].Gid];
                            EMSFuel emsf = dEMSFuel[sm.Fuel];
                            OptimisationModel om = new OptimisationModel(sm, emsf, measurements[i]);
                            loms.Add(om);

                            Decision d = new Decision(Domain.RealNonnegative, "d" + om.GlobalId.ToString());
                            model.AddDecision(d);
                            decisions.Add(om.GlobalId, d);
                        }
                    }

                    Decision help;
                    string goal = "";
                    string limit = "limit";
                    string production = minProduction.ToString() + "<=";

                    for (int i = 0; i < loms.Count; i++)
                    {
                        help = decisions[loms[i].GlobalId];

                        Term tLimit = loms[i].MinPower <= help <= loms[i].MaxPower;
                        model.AddConstraint(limit + loms[i].GlobalId, tLimit);

                        production += help.ToString() + "+";

                        goal += help.ToString() + "*" + loms[i].Price.ToString() + "+";
                    }

                    production = production.Substring(0, production.Length - 1);
                    production += "<=" + maxProduction.ToString();
                    // Term tProduction = decisions[1] + decisions[2] <= maxProduction;
                    model.AddConstraint("production", production);

                    goal = goal.Substring(0, goal.Length - 1);
                    // Term tGoal = decisions[1] * loms[0].Price + decisions[2] * loms[1].Price;
                    model.AddGoal("cost", GoalKind.Minimize, goal);

                    Solution solution = context.Solve(new SimplexDirective());
                    Report report = solution.GetReport();
                    Console.Write("{0}", report);

                    string name = "";
                    foreach (var item in model.Decisions)
                    {
                        for (int i = 0; i < measurements.Count; i++)
                        {
                            name = item.Name.Substring(1);
                            if (Int64.Parse(name) == measurements[i].Gid)
                            {
                                measurements[i].OptimizedLinear = float.Parse(item.ToString());
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
        /// Method implements integrity update logic for scada cr component
        /// </summary>
        /// <returns></returns>
        public bool InitiateIntegrityUpdate()
        {
            lock (lockObj)
            {
                ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

                List<ModelCode> properties = new List<ModelCode>(10);
                ModelCode modelCodeEmsFuel = ModelCode.EMSFUEL;
                ModelCode modelCodeSynchM = ModelCode.SYNCHRONOUSMACHINE;
                ModelCode modelCodeEnrgyConsum = ModelCode.ENERGYCONSUMER;

                int iteratorId = 0;
                int resourcesLeft = 0;
                int numberOfResources = 2;
                string message = string.Empty;

                List<ResourceDescription> retList = new List<ResourceDescription>(5);

                try
                {
                    // first get all synchronous machines from NMS
                    properties = modelResourcesDesc.GetAllPropertyIds(modelCodeSynchM);

                    iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeSynchM, properties);
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
                    message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCodeSynchM, e.Message);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    return false;
                }

                message = string.Format("Integrity update: Number of {0} values: {1}", modelCodeSynchM.ToString(), internalSynchMachines.Count.ToString());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCodeSynchM.ToString(), internalSynchMachines.Count.ToString());

                //GETTING EMSFUEL

                // clear retList for getting new model from NMS
                retList.Clear();

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
                
                //GETTING EnergyConsumers
                
                // clear retList for getting new model from NMS
                retList.Clear();
                
                try
                {
                    // get all enenrgy consumers from NMS
                    properties = modelResourcesDesc.GetAllPropertyIds(modelCodeEnrgyConsum);

                    iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeEnrgyConsum, properties);
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
                    message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCodeEnrgyConsum, e.Message);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    return false;
                }

                message = string.Format("Integrity update: Number of {0} values: {1}", modelCodeEnrgyConsum.ToString(), internalEnergyConsumers.Count.ToString());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCodeEnrgyConsum.ToString(), internalEnergyConsumers.Count.ToString());


                FillData();
                return true;
            }
        }

        #endregion IntegrityUpdate

        #region Transaction

        public UpdateResult Prepare(Delta delta)
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

                foreach (ResourceDescription rd in internalSynchMachines)
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

                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "CE Transaction: Commit phase successfully finished.");
                Console.WriteLine("Number of EMSFuels values: {0}", internalEmsFuels.Count);
                Console.WriteLine("Number of SynchronousMachines values: {0}", internalSynchMachines.Count);
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