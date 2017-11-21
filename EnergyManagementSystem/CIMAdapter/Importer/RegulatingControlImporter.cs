using System;
using EMS.CIMAdapter.Manager;
using CIM.Model;
using EMS.Common;

namespace EMS.CIMAdapter.Importer
{
	/// <summary>
	/// RegulatingControlImporter
	/// </summary>
	public class RegulatingControlImporter
    {
        /// <summary> Singleton </summary>
        private static RegulatingControlImporter regImporter = null;
        private static object singletoneLock = new object();

        private ConcreteModel concreteModel;
        private Delta delta;
        private ImportHelper importHelper;
        private TransformAndLoadReport report;


        #region Properties
        public static RegulatingControlImporter Instance
        {
            get
            {
                if (regImporter == null)
                {
                    lock (singletoneLock)
                    {
                        if (regImporter == null)
                        {
                            regImporter = new RegulatingControlImporter();
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

        #endregion Import
    }
}
