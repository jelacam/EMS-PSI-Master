using System;
using EMS.CIMAdapter.Manager;
using CIM.Model;
using EMS.Common;
using System.Collections.Generic;

namespace EMS.CIMAdapter.Importer
{
	/// <summary>
	/// RegulatingControlImporter
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

        public Delta NMSDelta
        {
            get
            {
                return delta;
            }
        }
        #endregion Properties


        public void Reset()
        {
            concreteModel = null;
            delta = new Delta();
            importHelper = new ImportHelper();
            report = null;
        }

        public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
        {
            LogManager.Log("Importing RegulatingControl Elements...", LogLevel.Info);
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
            LogManager.Log("Importing RegulatingControl Elements - END.", LogLevel.Info);
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
            ImportSynchronousMachine();
            ImportAnalog();

            /*ImportReactiveCapabilityCurves();
            ImportCurveDatas();
            ImportTerminals();
            ImportRegulatingControls();
            ImportShuntCompensators();
            ImportFrequencyConverter();
            ImportStaticVarCompensators();
            ImportSynchronousMachines();
            ImportControls();*/

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
        }

        #region Import

        private void ImportEnergyConsumer()
        {
            
            SortedDictionary<string, object> cimEnergyConsumers = concreteModel.GetAllObjectsOfType("EMS.EnergyConsumer");
            if (cimEnergyConsumers != null)
            {
                foreach (KeyValuePair<string, object> cimEnergyConsumerPair in cimEnergyConsumers)
                {
                    EMS.EnergyConsumer energyConsumer = cimEnergyConsumerPair as EMS.EnergyConsumer;

                    ResourceDescription rd = CreateEnergyConsumer(energyConsumer);

                    if(rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("EnergyConsumer ID = ").Append(energyConsumer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("EnergyConsumer ID = ").Append(energyConsumer.ID).AppendLine(" FAILED to be converted");
                    }
                }
            }
        }

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

        private void ImportSynchronousMachine()
        {

            SortedDictionary<string, object> cimSynchronousMachines = concreteModel.GetAllObjectsOfType("EMS.SynchronousMachine");
            if (cimEnergyConsumers != null)
            {
                foreach (KeyValuePair<string, object> cimSynchronousMachinePair in cimSynchronousMachines)
                {
                    EMS.SynchronousMachine synchronousMachine = cimSynchronousMachinePair as EMS.SynchronousMachine;

                    ResourceDescription rd = CreateSynchronousMachine(synchronousMachine);

                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("SynchronousMachine ID = ").Append(synchronousMachine.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("SynchronousMachine ID = ").Append(synchronousMachine.ID).AppendLine(" FAILED to be converted");
                    }
                }
            }
        }

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

        private void ImportAnalog()
        {

            SortedDictionary<string, object> cimAnalogs = concreteModel.GetAllObjectsOfType("EMS.Analogs");
            if (cimAnalogs != null)
            {
                foreach (KeyValuePair<string, object> cimAnalogPair in cimAnalogs)
                {
                    EMS.Analog analog = cimAnalogPair as EMS.Analog;

                    ResourceDescription rd = CreateAnalog(analog);

                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Analog ID = ").Append(analog.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Analog ID = ").Append(analog.ID).AppendLine(" FAILED to be converted");
                    }
                }
            }
        }

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
