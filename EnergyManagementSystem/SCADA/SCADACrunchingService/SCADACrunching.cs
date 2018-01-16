//-----------------------------------------------------------------------
// <copyright file="SCADACrunching.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACrunchingService
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Common;
    using CommonMeasurement;
    using NetworkModelService.DataModel.Meas;
    using ServiceContracts;
    using SmoothModbus;

    /// <summary>
    /// SCADACrunching component logic
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class SCADACrunching : IScadaCRContract, ITransactionContract
    {
        /// <summary>
        /// list for storing AnalogLocation values
        /// </summary>
        private static List<AnalogLocation> listOfAnalog;

        /// <summary>
        /// list for storing copy of AnalogLocation values
        /// </summary>
        private static List<AnalogLocation> listOfAnalogCopy;

        private UpdateResult updateResult;

        private ModelResourcesDesc modelResourcesDesc;

        private ITransactionCallback transactionCallback;

        /// <summary>
        /// instance of ConvertorHelper class
        /// </summary>
        private ConvertorHelper convertorHelper;

        /// <summary>
        private string message = string.Empty;

        /// Initializes a new instance of the <see cref="SCADACrunching" /> class
        /// </summary>
        public SCADACrunching()
        {
            this.convertorHelper = new ConvertorHelper();

            // TODO treba izmeniti kad se napravi transakcija sa NMS-om
            listOfAnalog = new List<AnalogLocation>();
            listOfAnalogCopy = new List<AnalogLocation>();

            //for (int i = 0; i < 5; i++)
            //{
            //    Analog analog = new Analog(10000 + i);
            //    analog.MinValue = 0;
            //    analog.MaxValue = 5;
            //    analog.PowerSystemResource = 20000 + i;
            //    this.listOfAnalog.Add(new AnalogLocation()
            //    {
            //        Analog = analog,
            //        StartAddress = i * 2, // flaot value 4bytes
            //        Length = 2
            //    });
            //}

            modelResourcesDesc = new ModelResourcesDesc();
        }

        #region Transaction

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
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "SCADA CR Transaction: Commit phase successfully finished.");
                Console.WriteLine("Number of Analog values: {0}", listOfAnalog.Count);
                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "SCADA CR Transaction: Failed to Commit changes. Message: {0}", e.Message);
                return false;
            }
        }

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

                updateResult.Message = "SCADA CR Transaction Prepare finished.";
                updateResult.Result = ResultType.Succeeded;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "SCADA CR Transaction Prepare finished successfully.");
                transactionCallback.Response("OK");
            }
            catch (Exception e)
            {
                updateResult.Message = "SCADA CR Transaction Prepare finished.";
                updateResult.Result = ResultType.Failed;
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "SCADA CR Transaction Prepare failed. Message: {0}", e.Message);
                transactionCallback.Response("ERROR");
            }

            return updateResult;
        }

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
        /// SendValues method implementation
        /// </summary>
        /// <param name="value">values to send</param>
        /// <returns>returns true if success</returns>
        public bool SendValues(byte[] value)
        {
            float eguVal;
            bool alarmRaw;
            bool alarmEGU;

            string function = Enum.GetName(typeof(FunctionCode), value[0]);
            Console.WriteLine("Function executed: {0}", function);

            int arrayLength = value[1];
            Console.WriteLine("Byte count: {0}", arrayLength);

            List<MeasurementUnit> listOfMeasUnit = new List<MeasurementUnit>();
            foreach (AnalogLocation analogLoc in listOfAnalog)
            {
                // startIndex = 2 because first two bytes a metadata
                float[] values = ModbusHelper.GetValueFromByteArray<float>(value, analogLoc.Length * 2, 2 + analogLoc.StartAddress * 2);

                alarmRaw = this.CheckForRawAlarms(values[0], convertorHelper.MinRaw, convertorHelper.MaxRaw, analogLoc.Analog.PowerSystemResource);
                eguVal = convertorHelper.ConvertFromRawToEGUValue(values[0], analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue);
                alarmEGU = this.CheckForEGUAlarms(eguVal, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue, analogLoc.Analog.PowerSystemResource);

                MeasurementUnit measUnit = new MeasurementUnit();
                measUnit.Gid = analogLoc.Analog.PowerSystemResource;
                measUnit.MinValue = analogLoc.Analog.MinValue;
                measUnit.MaxValue = analogLoc.Analog.MaxValue;
                measUnit.CurrentValue = eguVal;
                listOfMeasUnit.Add(measUnit);
            }

            bool isSuccess = false;
            try
            {
                isSuccess = CalculationEngineProxy.Instance.OptimisationAlgorithm(listOfMeasUnit);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
            }

            if (isSuccess)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Successfuly sent list with {0} items to CE.", listOfMeasUnit.Count);
                Console.WriteLine("Successfuly sent list with {0} items to CE.", listOfMeasUnit.Count);
            }

            return isSuccess;
        }

        /// <summary>
        /// Test method
        /// </summary>
        public void Test()
        {
            Console.WriteLine("SCADA Crunching: Test method");
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

            message = string.Format("Integrity update: Number of {0} values: {1}", modelCode.ToString(), listOfAnalog.Count.ToString());
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCode.ToString(), listOfAnalog.Count.ToString());
            return true;
        }

        /// <summary>
        /// Checking for alarms on egu value
        /// </summary>
        /// <param name="value">value to check</param>
        /// <param name="minRaw">low limit</param>
        /// <param name="maxRaw">high limit</param>
        /// <param name="gid">gid of measurement</param>
        /// <returns>returns true if alarm exists</returns>
        private bool CheckForRawAlarms(float value, float minRaw, float maxRaw, long gid)
        {
            bool retVal = false;
            AlarmHelper ah = new AlarmHelper(gid, value, minRaw, maxRaw, DateTime.Now);
            if (value < minRaw)
            {
                ah.Type = AlarmType.rawMin;
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low raw limit on gid: {0}", gid);
                Console.WriteLine("Alarm on low raw limit on gid: {0}", gid);
            }

            if (value > maxRaw)
            {
                ah.Type = AlarmType.rawMax;
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high raw limit on gid: {0}", gid);
                Console.WriteLine("Alarm on high raw limit on gid: {0}", gid);
            }

            return retVal;
        }

        /// <summary>
        /// Checking for alarms on egu value
        /// </summary>
        /// <param name="value">value to check</param>
        /// <param name="minEGU">low limit</param>
        /// <param name="maxEGU">high limit</param>
        /// <param name="gid">gid of measurement</param>
        /// <returns>returns true if alarm exists</returns>
        private bool CheckForEGUAlarms(float value, float minEGU, float maxEGU, long gid)
        {
            bool retVal = false;
            AlarmHelper ah = new AlarmHelper(gid, value, minEGU, maxEGU, DateTime.Now);
            if (value < minEGU)
            {
                ah.Type = AlarmType.eguMin;
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low egu limit on gid: {0}", gid);
                Console.WriteLine("Alarm on low egu limit on gid: {0}", gid);
            }

            if (value > maxEGU)
            {
                ah.Type = AlarmType.eguMax;
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high egu limit on gid: {0}", gid);
                Console.WriteLine("Alarm on high egu limit on gid: {0}", gid);
            }

            return retVal;
        }
    }
}