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
        /// list for storing AnalogLocation values for generators
        /// </summary>
        private static List<AnalogLocation> generatorAnalogs;

        /// <summary>
        /// list for storing AnalogLocation values for energyConsumers
        /// </summary>
        private static List<AnalogLocation> energyConsumersAnalogs;

        /// <summary>
        /// list for storing copy of AnalogLocation values
        /// </summary>
        private static List<AnalogLocation> generatorAnalogsCopy;

        /// <summary>
        /// list for storing copy of AnalogLocation values
        /// </summary>
        private static List<AnalogLocation> energyConsumersAnalogsCopy;

        private UpdateResult updateResult;

        private ModelResourcesDesc modelResourcesDesc;

        private ITransactionCallback transactionCallback;

        /// <summary>
        /// instance of ConvertorHelper class
        /// </summary>
        private ConvertorHelper convertorHelper;

        /// <summary>
        private string message = string.Empty;
        private readonly int START_ADDRESS_GENERATOR = 50;

        /// Initializes a new instance of the <see cref="SCADACrunching" /> class
        /// </summary>
        public SCADACrunching()
        {
            this.convertorHelper = new ConvertorHelper();

            generatorAnalogs = new List<AnalogLocation>();
            generatorAnalogsCopy = new List<AnalogLocation>();

            energyConsumersAnalogs = new List<AnalogLocation>();
            energyConsumersAnalogsCopy = new List<AnalogLocation>();


            modelResourcesDesc = new ModelResourcesDesc();
        }

        #region Transaction

        public bool Commit(Delta delta)
        {
            try
            {
                generatorAnalogs.Clear();
                energyConsumersAnalogs.Clear();

                foreach (AnalogLocation alocation in generatorAnalogsCopy)
                {
                    generatorAnalogs.Add(alocation.Clone() as AnalogLocation);
                }

                foreach (AnalogLocation alocation in energyConsumersAnalogsCopy)
                {
                    energyConsumersAnalogs.Add(alocation.Clone() as AnalogLocation);
                }

                generatorAnalogsCopy.Clear();
                energyConsumersAnalogsCopy.Clear();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "SCADA CR Transaction: Commit phase successfully finished.");
                Console.WriteLine("Number of generator Analog values: {0}", generatorAnalogs.Count);
                Console.WriteLine("Number of energy consumer Analog values: {0}", energyConsumersAnalogs.Count);
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

                generatorAnalogsCopy.Clear();
                energyConsumersAnalogsCopy.Clear();

                // napravi kopiju od originala 
                foreach (AnalogLocation alocation in generatorAnalogs)
                {
                    generatorAnalogsCopy.Add(alocation.Clone() as AnalogLocation);
                }

                foreach (AnalogLocation alocation in energyConsumersAnalogs)
                {
                    energyConsumersAnalogsCopy.Add(alocation.Clone() as AnalogLocation);
                }

                Analog analog = null;

                foreach (ResourceDescription analogRd in delta.InsertOperations)
                {
                    analog = ResourcesDescriptionConverter.ConvertTo<Analog>(analogRd);

                    if ((EMSType)ModelCodeHelper.ExtractTypeFromGlobalId(analog.PowerSystemResource) == EMSType.ENERGYCONSUMER)
                    {
                        energyConsumersAnalogsCopy.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = energyConsumersAnalogsCopy.Count * 2, // float value 4 bytes
                            Length = 2,
                            LengthInBytes = 4
                        });
                    }else
                    {
                        generatorAnalogsCopy.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = START_ADDRESS_GENERATOR + generatorAnalogsCopy.Count * 2, // float value 4 bytes
                            Length = 2,
                            LengthInBytes = 4
                        });
                    }
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
                generatorAnalogsCopy.Clear();
                energyConsumersAnalogsCopy.Clear();
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
            string function = Enum.GetName(typeof(FunctionCode), value[0]);
            Console.WriteLine("Function executed: {0}", function);

            int arrayLength = value[1];
            byte[] data = new byte[arrayLength];

            Console.WriteLine("Byte count: {0}", arrayLength);

            Array.Copy(value, 2, data, 0, arrayLength );

            List <MeasurementUnit> enConsumMeasUnits = ParseDataToMeasurementUnit(energyConsumersAnalogs, data, 0);
            List <MeasurementUnit> generatorMeasUnits = ParseDataToMeasurementUnit(generatorAnalogs, data, START_ADDRESS_GENERATOR);

            bool isSuccess = false;
            try
            {
                isSuccess = CalculationEngineProxy.Instance.OptimisationAlgorithm(enConsumMeasUnits,generatorMeasUnits);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
            }

            if (isSuccess)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Successfuly sent items to CE.");
                Console.WriteLine("Successfuly sent items to CE.");
            }

            return isSuccess;
        }

        private List<MeasurementUnit> ParseDataToMeasurementUnit(List<AnalogLocation> generatorAnalogs, byte[] value, int startAddress)
        {
            List<MeasurementUnit> retList = new List<MeasurementUnit>();
            foreach (AnalogLocation analogLoc in generatorAnalogs)
            {
                float[] values = ModbusHelper.GetValueFromByteArray<float>(value, analogLoc.LengthInBytes, startAddress + analogLoc.StartAddress * 2); // 2 jer su registri od 2 byte-a

                bool alarmRaw = this.CheckForRawAlarms(values[0], convertorHelper.MinRaw, convertorHelper.MaxRaw, analogLoc.Analog.PowerSystemResource);
                float eguVal = convertorHelper.ConvertFromRawToEGUValue(values[0], analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue);
                bool alarmEGU = this.CheckForEGUAlarms(eguVal, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue, analogLoc.Analog.PowerSystemResource);

                MeasurementUnit measUnit = new MeasurementUnit();
                measUnit.Gid = analogLoc.Analog.PowerSystemResource;
                measUnit.MinValue = analogLoc.Analog.MinValue;
                measUnit.MaxValue = analogLoc.Analog.MaxValue;
                measUnit.CurrentValue = eguVal;
                retList.Add(measUnit);
            }

            return retList;
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

            try
            {
                foreach (ResourceDescription rd in retList)
                {
                    Analog analog = ResourcesDescriptionConverter.ConvertTo<Analog>(rd);

                    if ((EMSType)ModelCodeHelper.ExtractTypeFromGlobalId(analog.PowerSystemResource) == EMSType.ENERGYCONSUMER)
                    {
                        energyConsumersAnalogs.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = energyConsumersAnalogs.Count * 2,
                            Length = 2,
                            LengthInBytes = 4
                        });
                    }else
                    {
                        generatorAnalogs.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = START_ADDRESS_GENERATOR + generatorAnalogs.Count * 2,
                            Length = 2,
                            LengthInBytes = 4
                        });
                    }

                }
            }
            catch (Exception e)
            {
                message = string.Format("Conversion to Analog object failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return false;
            }

            message = string.Format("Integrity update: Number of {0} values: {1}", modelCode.ToString(), retList.Count.ToString());
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCode.ToString(), retList.Count.ToString());
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