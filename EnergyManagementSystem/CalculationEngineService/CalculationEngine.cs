//-----------------------------------------------------------------------
// <copyright file="CalculationEngine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
    using System;
    using System.Collections.Generic;
    using CommonMeasurement;
    using EMS.Common;
    using EMS.ServiceContracts;
    using PubSub;
    using Microsoft.SolverFoundation.Common;
    using Microsoft.SolverFoundation.Services;
    using NetworkModelService.DataModel.Wires;
    using NetworkModelService.DataModel.Production;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// Class for CalculationEngine
    /// </summary>
    public class CalculationEngine
    {
        private Dictionary<long, SynchronousMachine> generators = new Dictionary<long, SynchronousMachine>();
        private float powerOfConsumers = 34567;
        private float totalCostLinear = 0;
        private SolverContext context = SolverContext.GetContext();
        private Model model;
        private List<OptimisationModel> loms;

        private List<ResourceDescription> internalSynchMachines;
        private List<ResourceDescription> internalEmsFuels;

        private Dictionary<long, SynchronousMachine> dSynchronousMachine;
        private Dictionary<long, EMSFuel> dEMSFuel;

        private object lockObj = new object();

        private PublisherService publisher = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationEngine" /> class
        /// </summary>
        public CalculationEngine()
        {
            publisher = new PublisherService();
            internalEmsFuels = new List<ResourceDescription>(5);
            internalSynchMachines = new List<ResourceDescription>(5);
            this.dSynchronousMachine = new Dictionary<long, SynchronousMachine>();
            this.dEMSFuel = new Dictionary<long, EMSFuel>();
        }

        /// <summary>
        /// Optimization algorithm
        /// </summary>
        /// <param name="measurements">list of measurements which should be optimized</param>
        /// <returns>returns true if optimization was successful</returns>
        public bool Optimize(List<MeasurementUnit> measurements)
        {
            this.FillData();
            //List<MeasurementUnit> l = this.LinearOptimization(measurements);

            bool alarmOptimized = true;
            bool result = false;
            if (measurements != null)
            {
                if (measurements.Count > 0)
                {
                    if (InsertMeasurementsIntoDb(measurements))
                    {
                        Console.WriteLine("Inserted {0} Measurement(s) into history database.", measurements.Count);
                    }

                    Console.WriteLine("CE: Optimize");
                    for (int i = 0; i < measurements.Count; i++)
                    {
                        measurements[i].CurrentValue = measurements[i].CurrentValue * 2;

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

            this.ClearData();
            return result;
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
                            cmd.Parameters.Add("@timeMeasurement", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
        public List<Tuple<double,DateTime>> ReadMeasurementsFromDb(long gid, DateTime startTime, DateTime endTime)
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

        #endregion


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

        #region Necessary data

        /// <summary>
        /// Fills data
        /// </summary>
        private void FillData()
        {
            lock (lockObj)
            {
                foreach (ResourceDescription rd in this.internalSynchMachines)
                {
                    SynchronousMachine sm = ResourcesDescriptionConverter.ConvertToSynchronousMachine(rd);
                    this.dSynchronousMachine.Add(sm.GlobalId, sm);
                }

                foreach (ResourceDescription rd in this.internalEmsFuels)
                {
                    EMSFuel emsf = ResourcesDescriptionConverter.ConvertToEMSFuel(rd);
                    this.dEMSFuel.Add(emsf.GlobalId, emsf);
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
                this.loms.Clear();
            }
        }

        #endregion

        private List<MeasurementUnit> LinearOptimization(List<MeasurementUnit> measurements)
        {
            lock (lockObj)
            {
                if (measurements.Count > 0)
                {
                    model = context.CreateModel();

                    bool alarmOptimized = false;

                    Dictionary<string, Decision> decisions = new Dictionary<string, Decision>();

                    for (int i = 0; i < measurements.Count; i++)
                    {
                        if (dSynchronousMachine.ContainsKey(measurements[i].Gid))
                        {
                            SynchronousMachine sm = dSynchronousMachine[measurements[i].Gid];
                            EMSFuel emsf = dEMSFuel[sm.Fuel];

                            OptimisationModel om = new OptimisationModel(sm, emsf, measurements[i]);
                            loms.Add(om);

                            Decision d = new Decision(Domain.RealNonnegative, "d" + om.GlobalId.ToString());
                            decisions.Add("d" + om.GlobalId.ToString(), d);
                            model.AddDecision(d);
                        }
                    }

                    string limitMax = "limitMax";
                    string limitMin = "limitMin";
                    string managable = "";
                    string goal = "";
                    string help = "";

                    for (int i = 0; i < loms.Count; i++)
                    {
                        help = "d" + loms[i].GlobalId;
                        Term tLimitMax = help + "<=" + loms[i].MaxPower;
                        model.AddConstraint(limitMax + loms[i].GlobalId, tLimitMax);

                        Term tLimitMin = help + ">=" + loms[i].MinPower;
                        model.AddConstraint(limitMin + loms[i].GlobalId, tLimitMin);

                        managable += help + "*" + loms[i].Managable.ToString() + "+";

                        goal += help + "*" + loms[i].Price.ToString() + "+";
                    }
                    managable = managable.Substring(0, managable.Length - 1);
                    managable += "<=" + this.powerOfConsumers.ToString();
                    Term tManagable = managable;
                    model.AddConstraint("managable", tManagable);

                    goal = goal.Substring(0, goal.Length - 1);
                    Term tGoal = goal;
                    model.AddGoal("cost", GoalKind.Minimize, tGoal);

                    Solution solution = context.Solve(new SimplexDirective());
                    Report report = solution.GetReport();
                    Console.Write("{0}", report);

                    string name = "";
                    foreach (var item in model.Decisions)
                    {
                        for (int i = 0; i < measurements.Count; i++)
                        {
                            name = item.Name.Substring(1);
                            if (Int64.Parse(item.Name) == measurements[i].Gid)
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

                message = string.Format("Integrity update: Number of {0} values: {1}", modelCodeEmsFuel.ToString(), internalSynchMachines.Count.ToString());
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCodeEmsFuel.ToString(), internalSynchMachines.Count.ToString());
                return true;
            }
        }

        #endregion 
    }
}