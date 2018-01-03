//-----------------------------------------------------------------------
// <copyright file="SCADACommanding.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACommandingService
{
    using Common;
    using CommonMeasurement;
    using ServiceContracts;
    using NetworkModelService.DataModel.Meas;
    using SmoothModbus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    /// <summary>
    /// SCADACommanding class for accept data from CE and put data to simulator
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class SCADACommanding : IScadaCMDContract, ITransactionContract
    {
        /// <summary>
        /// Instance of ModbusClient
        /// </summary>
        private ModbusClient modbusClient;

        /// <summary>
        /// List for storing CMDAnalogLocation values
        /// </summary>
        private static List<AnalogLocation> listOfAnalog;

        private static List<AnalogLocation> listOfAnalogCopy;

        private UpdateResult updateResult;

        private ModelResourcesDesc modelResourcesDesc;

        private string message = string.Empty;

        /// <summary>
        /// TransactionCallback
        /// </summary>
        private ITransactionCallback transactionCallback;

		/// <summary>
		/// instance of ConvertorHelper class
		/// </summary>
		private ConvertorHelper convertorHelper;

        /// <summary>
        /// Constructor SCADACommanding class
        /// </summary>
        public SCADACommanding()
        {
            this.modbusClient = new ModbusClient("localhost", 502);
            this.modbusClient.Connect();

            listOfAnalog = new List<AnalogLocation>();
            listOfAnalogCopy = new List<AnalogLocation>();

			this.convertorHelper = new ConvertorHelper();
            modelResourcesDesc = new ModelResourcesDesc();
            //CreateCMDAnalogLocation();
        }

        #region Transaction

        /// <summary>
        /// Commit method
        /// </summary>
        /// <returns></returns>
        public bool Commit(Delta delta)
        {
            try
            {
                listOfAnalog.Clear();
                foreach (AnalogLocation alocation in listOfAnalogCopy)
                {
                    listOfAnalog.Add(alocation.Clone() as AnalogLocation);
                }

                listOfAnalogCopy.Clear();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "SCADA CMD Transaction: Commit phase successfully finished.");
                Console.WriteLine("Number of Analog values: {0}", listOfAnalog.Count);
                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "SCADA CMD Transaction: Failed to Commit changes. Message: {0}", e.Message);
                return false;
            }
        }

        /// <summary>
        /// Prepare method
        /// </summary>
        /// <param name="delta"></param>
        public UpdateResult Prepare(Delta delta)
        {
            try
            {
                transactionCallback = OperationContext.Current.GetCallbackChannel<ITransactionCallback>();
                updateResult = new UpdateResult();

                listOfAnalogCopy = new List<AnalogLocation>();

                // napravi kopiju od originala 
                foreach (AnalogLocation alocation in listOfAnalog)
                {
                    listOfAnalogCopy.Add(alocation.Clone() as AnalogLocation);
                }


                Analog analog = null;
                //int i = 0; // analog counter for address
                int i = listOfAnalogCopy.Count;

                foreach (ResourceDescription analogRd in delta.InsertOperations)
                {
                    analog = ResourcesDescriptionConverter.ConvertToAnalog(analogRd);

                    listOfAnalogCopy.Add(new AnalogLocation()
                    {
                        Analog = analog,
                        StartAddress = i * 2, // float value 4 bytes
                        Length = 2
                    });

                    i++;
                }

                updateResult.Message = "SCADA CMD Transaction Prepare finished.";
                updateResult.Result = ResultType.Succeeded;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "SCADA CMD Transaction Prepare finished successfully.");
                transactionCallback.Response("OK");
            }
            catch (Exception e)
            {
                updateResult.Message = "SCADA CMD Transaction Prepare finished.";
                updateResult.Result = ResultType.Failed;
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "SCADA CMD Transaction Prepare failed. Message: {0}", e.Message);
                transactionCallback.Response("ERROR");
            }

            return updateResult;
        }

        /// <summary>
        /// Rollback Method
        /// </summary>
        /// <returns></returns>
        public bool Rollback()
        {
            try
            {
                listOfAnalogCopy.Clear();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Transaction rollback successfully finished!");
                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Transaction rollback error. Message: {0}", e.Message);
                return false;
            }
        }

        #endregion Transaction

        /// <summary>
        /// Method instantiates the test data
        /// </summary>
        public void CreateCMDAnalogLocation()
        {
            // TODO treba izmeniti kad se napravi transakcija sa NMS-om
            listOfAnalog = new List<AnalogLocation>();
            for (int i = 0; i < 5; i++)
            {
                Analog analog = new Analog(10000 + i);
                analog.MinValue = 0;
                analog.MaxValue = 5;
                analog.PowerSystemResource = 20000 + i;
                listOfAnalog.Add(new AnalogLocation()
                {
                    Analog = analog,
                    StartAddress = i * 2,
                    Length = 2
                });
            }
        }

        /// <summary>
        /// Method implements integrity update logic for scada cr component
        /// </summary>
        /// <returns></returns>
        public bool InitiateIntegrityUpdate()
        {
            List<ModelCode> properties = new List<ModelCode>(10);
            ModelCode modelCode = ModelCode.ANALOG;
            int iteratorId = 0;
            int resourcesLeft = 0;
            int numberOfResources = 2;


            List<ResourceDescription> retList = new List<ResourceDescription>(5);
            try
            {
                properties = modelResourcesDesc.GetAllPropertyIds(modelCode);

                iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCode, properties);
                resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                    retList.AddRange(rds);
                    resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                }
                NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);
            }
            catch (Exception e)
            {
                message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCode, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return false;
            }


            listOfAnalog.Clear();

            try
            {
                int i = 0;
                foreach (ResourceDescription rd in retList)
                {
                    Analog analog = ResourcesDescriptionConverter.ConvertToAnalog(rd);
                    listOfAnalog.Add(new AnalogLocation()
                    {
                        Analog = analog,
                        StartAddress = i++ * 2,
                        Length = 2
                    });

                }
            }
            catch (Exception e)
            {
                message = string.Format("Conversion to Analog object failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return false;
            }

            message = string.Format("Integrity update: Number of Analog values: {0}", listOfAnalog.Count.ToString());
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            Console.WriteLine("Number of analog values: {0}", listOfAnalog.Count.ToString());
            return true;
        }

        /// <summary>
        /// Method accepts data from CE and put data to simulator
        /// </summary>
        /// <param name="measurements"></param>
        public bool SendDataToSimulator(List<MeasurementUnit> measurements)
        {
            for (int i = 0; i < measurements.Count; i++)
            {
                AnalogLocation analogLoc = listOfAnalog.Where(x => x.Analog.PowerSystemResource == measurements[i].Gid).SingleOrDefault();
                try
                {
                    float rawVal = convertorHelper.ConvertFromEGUToRawValue(measurements[i].CurrentValue, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue);
                    modbusClient.WriteSingleRegister((ushort)analogLoc.StartAddress, rawVal);
                    // modbusClient.WriteSingleRegister((ushort)analogLoc.StartAddress, measurements[i].CurrentValue);
                }
                catch (System.Exception ex)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method for initial write in simulator
        /// </summary>
        public void TestWrite()
        {
            for (int i = 0; i < listOfAnalog.Count; i++)
            {
                try
                {
                    modbusClient.WriteSingleRegister((ushort)listOfAnalog[i].StartAddress, i * 10 + 10);
                }
                catch (System.Exception ex)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Test connection method
        /// </summary>
        public void Test()
        {
            Console.WriteLine("Test method");
        }      
    }
}