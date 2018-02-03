using System;
using EMS.CIMAdapter.Manager;
using CIM.Model;
using EMS.Common;
using System.Collections.Generic;
using EMS.ServiceContracts;

namespace EMS.CIMAdapter.Importer
{
    /// <summary>
    /// EMSImporter
    /// </summary>
    public class EMSImporter
    {
        /// <summary> Singleton </summary>
        private static EMSImporter regImporter = null;

        private static object singletoneLock = new object();

        private ConcreteModel concreteModel;
        private Delta delta;
        private ImportHelper importHelper;
        private TransformAndLoadReport report;

        #region Properties

        /// <summary>
        /// Return EMSImporter instance as singletone object
        /// </summary>
        public static EMSImporter Instance
        {
            get
            {
                if (regImporter == null)
                {
                    lock (singletoneLock)
                    {
                        if (regImporter == null)
                        {
                            regImporter = new EMSImporter();
                            regImporter.Reset();
                        }
                    }
                }
                return regImporter;
            }
        }

        /// <summary>
        /// Return delta object
        /// </summary>
        public Delta NMSDelta
        {
            get
            {
                return delta;
            }
        }

        #endregion Properties

        /// <summary>
        /// Reset method for EMSImporter objects
        /// </summary>
        public void Reset()
        {
            concreteModel = null;
            delta = new Delta();
            importHelper = new ImportHelper();
            report = null;
        }

        /// <summary>
        /// Method create EMSImporter objects and call for ConvertModel and PopulateDelta
        /// </summary>
        /// <param name="cimConcreteModel">Concrete model</param>
        /// <returns></returns>
        public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
        {
            LogManager.Log("Importing EMS Elements...", LogLevel.Info);
            report = new TransformAndLoadReport();
            concreteModel = cimConcreteModel;
            delta.ClearDeltaOperations();

            if ((concreteModel != null) && (concreteModel.ModelMap != null))
            {
                try
                {
                    // convert into DMS elements
                    ConvertModelAndPopulateDelta();
                }
                catch (Exception ex)
                {
                    string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
                    LogManager.Log(message);
                    report.Report.AppendLine(ex.Message);
                    report.Success = false;
                }
            }
            LogManager.Log("Importing EMS Elements - END.", LogLevel.Info);
            return report;
        }

        /// <summary>
        /// Method performs conversion of network elements from CIM based concrete model into DMS model.
        /// </summary>
        private void ConvertModelAndPopulateDelta()
        {
            LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            //// import all concrete model types (DMSType enum)
            
            ImportEnergyConsumer();
            ImportEMSFuel();
            ImportSynchronousMachine();
            ImportAnalog();

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
        }

        #region Import


        private void ImportEMSFuel()
        {
            SortedDictionary<string, object> cimEMSFuels = concreteModel.GetAllObjectsOfType("EMS.EMSFuel");
            if (cimEMSFuels != null)
            {
                foreach (KeyValuePair<string, object> cimEMSFuelPair in cimEMSFuels)
                {
                    EMS.EMSFuel emsFuel = cimEMSFuelPair.Value as EMS.EMSFuel;

                    ResourceDescription rd = CreateEMSFuel(emsFuel);

                    if (rd != null)
                    {
                        int iteratorId = 0;
                        int resourcesLeft = 0;
                        int numberOfResources = 2;
                        string message = string.Empty;
                        bool contains = false;

                        ModelCode modelCodeEMSFuel = ModelCode.EMSFUEL;
                        List<ModelCode> properties = new List<ModelCode>();
                        ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
                        List<ResourceDescription> retList = new List<ResourceDescription>();

                        properties = modelResourcesDesc.GetAllPropertyIds(modelCodeEMSFuel);
                        iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeEMSFuel, properties);
                        resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                        while (resourcesLeft > 0)
                        {
                            List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                            retList.AddRange(rds);
                            resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                        }
                        NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);

                        foreach(ResourceDescription res in retList)
                        {
                            foreach(Property pr in res.Properties)
                            {
                                if(pr.PropertyValue.StringValue.Equals(emsFuel.MRID))
                                {
                                    contains = true;
                                    rd.Id = res.Id;
                                }
                            }
                        }

                        if (contains)
                        {
                            delta.AddDeltaOperation(DeltaOpType.Update, rd, true);
                            report.Report.Append("EMSFuel ID = ").Append(emsFuel.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                        else
                        {
                            delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                            report.Report.Append("EMSFuel ID = ").Append(emsFuel.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                    }
                    else
                    {
                        report.Report.Append("EMSFuel ID = ").Append(emsFuel.ID).AppendLine(" FAILED to be converted");
                    }
                }
            }
        }

        private ResourceDescription CreateEMSFuel(EMSFuel emsFuel)
        {
            ResourceDescription rd = null;
            if (emsFuel != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)EMSType.EMSFUEL, importHelper.CheckOutIndexForDMSType(EMSType.EMSFUEL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(emsFuel.ID, gid);

                EMSConverter.PopulateEmsFuelProperties(emsFuel, rd, importHelper, report);
            }
            return rd;
        }


        /// <summary>
        /// Method import EnergyConsumer objects into delta
        /// </summary>
        private void ImportEnergyConsumer()
        {
            SortedDictionary<string, object> cimEnergyConsumers = concreteModel.GetAllObjectsOfType("EMS.EnergyConsumer");
            if (cimEnergyConsumers != null)
            {
                foreach (KeyValuePair<string, object> cimEnergyConsumerPair in cimEnergyConsumers)
                {
                    EMS.EnergyConsumer energyConsumer = cimEnergyConsumerPair.Value as EMS.EnergyConsumer;

                    ResourceDescription rd = CreateEnergyConsumer(energyConsumer);

                    if (rd != null)
                    {
                        int iteratorId = 0;
                        int resourcesLeft = 0;
                        int numberOfResources = 2;
                        string message = string.Empty;
                        bool contains = false;

                        ModelCode modelCodeEnergyConsumer = ModelCode.ENERGYCONSUMER;
                        List<ModelCode> properties = new List<ModelCode>();
                        ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
                        List<ResourceDescription> retList = new List<ResourceDescription>();

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

                        foreach (ResourceDescription res in retList)
                        {
                            foreach (Property pr in res.Properties)
                            {
                                if (pr.PropertyValue.StringValue.Equals(energyConsumer.MRID))
                                {
                                    contains = true;
                                    rd.Id = res.Id;
                                }
                            }
                        }

                        if (contains)
                        {
                            delta.AddDeltaOperation(DeltaOpType.Update, rd, true);
                            report.Report.Append("EnergyConsumer ID = ").Append(energyConsumer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                        else
                        {
                            delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                            report.Report.Append("EnergyConsumer ID = ").Append(energyConsumer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                        
                    }
                    else
                    {
                        report.Report.Append("EnergyConsumer ID = ").Append(energyConsumer.ID).AppendLine(" FAILED to be converted");
                    }
                }
            }
        }

        /// <summary>
        /// Method creates EnergyConsumer object as ResourceDescription and calls method for populate EnergyConsumer properties
        /// </summary>
        /// <param name="energyConsumer">EnergyConsumer object from concrete model</param>
        /// <returns>EnergyConsumer object as ResourceDescription</returns>
        private ResourceDescription CreateEnergyConsumer(EMS.EnergyConsumer energyConsumer)
        {
            ResourceDescription rd = null;
            if (energyConsumer != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)EMSType.ENERGYCONSUMER, importHelper.CheckOutIndexForDMSType(EMSType.ENERGYCONSUMER));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(energyConsumer.ID, gid);

                EMSConverter.PopulateEnergyConsumerProperties(energyConsumer, rd, importHelper, report);
            }
            return rd;
        }

        /// <summary>
        /// Method import SynchronousMachine objects into delta
        /// </summary>
        private void ImportSynchronousMachine()
        {
            SortedDictionary<string, object> cimSynchronousMachines = concreteModel.GetAllObjectsOfType("EMS.SynchronousMachine");
            if (cimSynchronousMachines != null)
            {
                foreach (KeyValuePair<string, object> cimSynchronousMachinePair in cimSynchronousMachines)
                {
                    EMS.SynchronousMachine synchronousMachine = cimSynchronousMachinePair.Value as EMS.SynchronousMachine;

                    ResourceDescription rd = CreateSynchronousMachine(synchronousMachine);

                    if (rd != null)
                    {
                        int iteratorId = 0;
                        int resourcesLeft = 0;
                        int numberOfResources = 2;
                        string message = string.Empty;
                        bool contains = false;

                        ModelCode modelCodeSynchronusMachine = ModelCode.SYNCHRONOUSMACHINE;
                        List<ModelCode> properties = new List<ModelCode>();
                        ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
                        List<ResourceDescription> retList = new List<ResourceDescription>();

                        properties = modelResourcesDesc.GetAllPropertyIds(modelCodeSynchronusMachine);
                        iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeSynchronusMachine, properties);
                        resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                        while (resourcesLeft > 0)
                        {
                            List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                            retList.AddRange(rds);
                            resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                        }
                        NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);

                        foreach (ResourceDescription res in retList)
                        {
                            foreach (Property pr in res.Properties)
                            {
                                if (pr.PropertyValue.StringValue.Equals(synchronousMachine.MRID))
                                {
                                    contains = true;
                                    rd.Id = res.Id;
                                }
                            }
                        }

                        if (contains)
                        {
                            delta.AddDeltaOperation(DeltaOpType.Update, rd, true);
                            report.Report.Append("SynchronousMachine ID = ").Append(synchronousMachine.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                        else
                        {
                            delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                            report.Report.Append("SynchronousMachine ID = ").Append(synchronousMachine.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                       
                    }
                    else
                    {
                        report.Report.Append("SynchronousMachine ID = ").Append(synchronousMachine.ID).AppendLine(" FAILED to be converted");
                    }
                }
            }
        }

        /// <summary>
        ///  Method creates SynchronousMachine object as ResourceDescription and calls method for populate SynchronousMachine properties
        /// </summary>
        /// <param name="synchronousMachine">SynchronousMachine object from concrete model</param>
        /// <returns>SynchronousMachine object as ResourceDescription</returns>
        private ResourceDescription CreateSynchronousMachine(EMS.SynchronousMachine synchronousMachine)
        {
            ResourceDescription rd = null;
            if (synchronousMachine != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)EMSType.SYNCHRONOUSMACHINE, importHelper.CheckOutIndexForDMSType(EMSType.SYNCHRONOUSMACHINE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(synchronousMachine.ID, gid);

                EMSConverter.PopulateSynchronousMachineProperties(synchronousMachine, rd, importHelper, report);
            }
            return rd;
        }

        /// <summary>
        ///  Method import Analog objects into delta
        /// </summary>
        private void ImportAnalog()
        {
            SortedDictionary<string, object> cimAnalogs = concreteModel.GetAllObjectsOfType("EMS.Analog");
            if (cimAnalogs != null)
            {
                foreach (KeyValuePair<string, object> cimAnalogPair in cimAnalogs)
                {
                    EMS.Analog analog = cimAnalogPair.Value as EMS.Analog;

                    ResourceDescription rd = CreateAnalog(analog);

                    if (rd != null)
                    {
                        int iteratorId = 0;
                        int resourcesLeft = 0;
                        int numberOfResources = 2;
                        string message = string.Empty;
                        bool contains = false;

                        ModelCode modelCodeAnalog = ModelCode.ANALOG;
                        List<ModelCode> properties = new List<ModelCode>();
                        ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
                        List<ResourceDescription> retList = new List<ResourceDescription>();

                        properties = modelResourcesDesc.GetAllPropertyIds(modelCodeAnalog);
                        iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCodeAnalog, properties);
                        resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                        while (resourcesLeft > 0)
                        {
                            List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                            retList.AddRange(rds);
                            resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                        }
                        NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);

                        foreach (ResourceDescription res in retList)
                        {
                            foreach (Property pr in res.Properties)
                            {
                                if (pr.PropertyValue.StringValue.Equals(analog.MRID))
                                {
                                    contains = true;
                                    rd.Id = res.Id;
                                }
                            }
                        }

                        if (contains)
                        {
                            delta.AddDeltaOperation(DeltaOpType.Update, rd, true);
                            report.Report.Append("Analog ID = ").Append(analog.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                        else
                        {
                            delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                            report.Report.Append("Analog ID = ").Append(analog.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                        }
                        
                    }
                    else
                    {
                        report.Report.Append("Analog ID = ").Append(analog.ID).AppendLine(" FAILED to be converted");
                    }
                }
            }
        }

        /// <summary>
        /// Method creates v object as ResourceDescription and calls method for populate Analog properties
        /// </summary>
        /// <param name="analog">Analog object from concrete model</param>
        /// <returns>Analog object as ResourceDescription</returns>
        private ResourceDescription CreateAnalog(EMS.Analog analog)
        {
            ResourceDescription rd = null;
            if (analog != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)EMSType.ANALOG, importHelper.CheckOutIndexForDMSType(EMSType.ANALOG));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(analog.ID, gid);

                EMSConverter.PopulateAnalogProperties(analog, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Import
    }
}