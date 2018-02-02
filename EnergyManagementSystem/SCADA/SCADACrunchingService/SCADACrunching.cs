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
    using System.Xml.Serialization;
    using System.IO;
    using System.Threading;

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

        private static Dictionary<long, float> previousGeneratorAnalogs;
        private static Dictionary<long, int> flatLineAlarmCounter;

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

        private readonly int FLAT_LINE_ALARM_TIMEOUT = 5;

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

            previousGeneratorAnalogs = new Dictionary<long, float>(10);
            flatLineAlarmCounter = new Dictionary<long, int>(10);
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

                ScadaConfiguration scECA = new ScadaConfiguration();
                scECA.AnalogsList = energyConsumersAnalogs;
                XmlSerializer serializer = new XmlSerializer(typeof(ScadaConfiguration));
                StreamWriter writer = new StreamWriter("ScadaConfigECA.xml");
                serializer.Serialize(writer, scECA);

                ScadaConfiguration scGA = new ScadaConfiguration();
                scGA.AnalogsList = generatorAnalogs;
                XmlSerializer serializer2 = new XmlSerializer(typeof(ScadaConfiguration));
                StreamWriter writer2 = new StreamWriter("ScadaConfigGA.xml");
                serializer2.Serialize(writer2, scGA);

                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "SCADA CR Transaction: Failed to Commit changes. Message: {0}", e.Message);
                return false;
            }
        }

        public UpdateResult Prepare(ref Delta delta)
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
                    }
                    else
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
            int windByteLength = 4;
			int sunByteLength = 4;
            byte[] windData = new byte[windByteLength];
			byte[] sunData = new byte[sunByteLength];
			byte[] data = new byte[arrayLength - windByteLength - sunByteLength];

            Console.WriteLine("Byte count: {0}", arrayLength);

			Array.Copy(value, 2, data, 0, arrayLength - windByteLength-sunByteLength);
			Array.Copy(value, 2 + arrayLength - windByteLength - sunByteLength, windData, 0, windByteLength);
			Array.Copy(value, 2 + arrayLength - sunByteLength, sunData, 0, sunByteLength);

			List<MeasurementUnit> enConsumMeasUnits = ParseDataToMeasurementUnit(energyConsumersAnalogs, data, 0);
            List<MeasurementUnit> generatorMeasUnits = ParseDataToMeasurementUnit(generatorAnalogs, data, 0);

            float windSpeed = GetWindSpeed(windData, windByteLength);
			float sunlight = GetSunlight(sunData, sunByteLength);

            bool isSuccess = false;
            try
            {
                isSuccess = CalculationEngineProxy.Instance.OptimisationAlgorithm(enConsumMeasUnits, generatorMeasUnits, windSpeed, sunlight);
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

        private float GetWindSpeed(byte[] windData, int byteLength)
        {
            float[] values = ModbusHelper.GetValueFromByteArray<float>(windData, byteLength);
            return values[0];
        }

		private float GetSunlight(byte[] sunData, int byteLength)
		{
			float[] values = ModbusHelper.GetValueFromByteArray<float>(sunData, byteLength);
			return values[0];
		}

		private List<MeasurementUnit> ParseDataToMeasurementUnit(List<AnalogLocation> analogList, byte[] value, int startAddress)
        {
            List<MeasurementUnit> retList = new List<MeasurementUnit>();
            foreach (AnalogLocation analogLoc in analogList)
            {
                float[] values = ModbusHelper.GetValueFromByteArray<float>(value, analogLoc.LengthInBytes, startAddress + analogLoc.StartAddress * 2); // 2 jer su registri od 2 byte-a

                bool alarmRaw = this.CheckForRawAlarms(values[0], convertorHelper.MinRaw, convertorHelper.MaxRaw, analogLoc.Analog.PowerSystemResource);
                float eguVal = convertorHelper.ConvertFromRawToEGUValue(values[0], analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue);
                bool alarmEGU = this.CheckForEGUAlarms(eguVal, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue, analogLoc.Analog.PowerSystemResource);

                bool flatlineAlarm = CheckForFlatlineAlarm(analogLoc, eguVal);

                if (flatlineAlarm)
                {
                    AlarmHelper alarmH = new AlarmHelper(analogLoc.Analog.PowerSystemResource, eguVal, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue, DateTime.Now);
                    alarmH.Type = AlarmType.FLATLINE;
                    alarmH.Persistent = PersistentState.Nonpersistent;
                    alarmH.Message = string.Format("{0:X} in Flatline state for {1} iteration. Value = {2}", analogLoc.Analog.PowerSystemResource, FLAT_LINE_ALARM_TIMEOUT, eguVal);
                    AlarmsEventsProxy.Instance.AddAlarm(alarmH);
                }

                // na signalu vise nema alarma - update state
                // sa Active na Cleared
                if (!alarmRaw && !alarmEGU)
                {
                    AlarmsEventsProxy.Instance.UpdateStatus(analogLoc, State.Cleared);
                }

                // nema alarma - generisi event za promenu vrednosti
                //if (!alarmRaw && !alarmEGU)
                //{
                //    AlarmsEventsProxy.Instance.AddAlarm(new AlarmHelper(
                //        gid: analogLoc.Analog.GlobalId,
                //        value: eguVal,
                //        minValue: analogLoc.Analog.MinValue,
                //        maxValue: analogLoc.Analog.MaxValue,
                //        timeStamp: DateTime.Now
                //    ));
                //}

                if (analogLoc.Analog.Mrid.Equals("Analog_sm_10"))
                {
                    using (var txtWriter = new StreamWriter("PointsReport.txt", true))
                    {
                        txtWriter.WriteLine(" [" + DateTime.Now + "] " + " The value for " + analogLoc.Analog.Mrid + " before the conversion was: " + values[0] + ", and after:" + eguVal);
                    }
                }

                MeasurementUnit measUnit = new MeasurementUnit();
                measUnit.Gid = analogLoc.Analog.PowerSystemResource;
                measUnit.MinValue = analogLoc.Analog.MinValue;
                measUnit.MaxValue = analogLoc.Analog.MaxValue;
                measUnit.CurrentValue = eguVal;
                measUnit.TimeStamp = DateTime.Now;
                retList.Add(measUnit);

                // kreiraj kolekciju prethodnih vrednosti
                previousGeneratorAnalogs[analogLoc.Analog.GlobalId] = eguVal;
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

                Console.WriteLine("Trying again...");
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Trying again...");
                NetworkModelGDAProxy.Instance = null;
                Thread.Sleep(1000);
                InitiateIntegrityUpdate();

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
                    }
                    else
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

                ScadaConfiguration scECA = new ScadaConfiguration();
                scECA.AnalogsList = energyConsumersAnalogs;
                XmlSerializer serializer = new XmlSerializer(typeof(ScadaConfiguration));
                StreamWriter writer = new StreamWriter("ScadaConfigECA.xml");
                serializer.Serialize(writer, scECA);

                ScadaConfiguration scGA = new ScadaConfiguration();
                scGA.AnalogsList = generatorAnalogs;
                XmlSerializer serializer2 = new XmlSerializer(typeof(ScadaConfiguration));
                StreamWriter writer2 = new StreamWriter("ScadaConfigGA.xml");
                serializer2.Serialize(writer2, scGA);
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
            //float alarmMin = minRaw + (float)Math.Round((20f / 100f) * minRaw);
            //float alarmMax = minRaw - (float)Math.Round((20f / 100f) * minRaw);
            AlarmHelper ah = new AlarmHelper(gid, value, minRaw, maxRaw, DateTime.Now);
            if (value < minRaw)
            {
                ah.Type = AlarmType.RAW_MIN;
                ah.Severity = SeverityLevel.CRITICAL;
                ah.Message = string.Format("Value on input signal: {0:X} lower than minimum expected value", gid);
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low raw limit on gid: {0:X}", gid);
                Console.WriteLine("Alarm on low raw limit on gid: {0:X}", gid);
            }

            if (value > maxRaw)
            {
                ah.Type = AlarmType.RAW_MAX;
                ah.Severity = SeverityLevel.CRITICAL;
                ah.Message = string.Format("Value on input signal: {0:X} higher than maximum expected value", gid);
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high raw limit on gid: {0:X}", gid);
                Console.WriteLine("Alarm on high raw limit on gid: {0:X}", gid);
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
            float alarmMin = minEGU + (float)Math.Round((20f / 100f) * minEGU);
            float alarmMax = maxEGU - (float)Math.Round((20f / 100f) * maxEGU);

            float highMin = minEGU + (float)Math.Round((5f / 100f) * minEGU);
            float highMax = maxEGU - (float)Math.Round((5f / 100f) * maxEGU);
            AlarmHelper ah = new AlarmHelper(gid, value, minEGU, maxEGU, DateTime.Now);

            if (value < highMin)
            {
                ah.Type = AlarmType.EGU_MIN;
                ah.Severity = SeverityLevel.MEDIUM;
                ah.Message = string.Format("Value on input signal: {0:X} lower than minimum expected value", gid);
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low egu limit on gid: {0:X}", gid);
                Console.WriteLine("Alarm on low egu limit on gid: {0:X}", gid);
            }
            else if (value < alarmMin)
            {
                ah.Type = AlarmType.EGU_MIN;
                ah.Severity = SeverityLevel.MINOR;
                ah.Message = string.Format("Value on input signal: {0:X} lower than minimum expected value", gid);
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low egu limit on gid: {0:X}", gid);
                Console.WriteLine("Alarm on low egu limit on gid: {0:X}", gid);
            }

            if (value > highMax)
            {
                ah.Type = AlarmType.EGU_MAX;
                ah.Severity = SeverityLevel.HIGH;
                ah.Message = string.Format("Value on input signal: {0:X} higher than maximum expected value", gid);
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high egu limit on gid: {0:X}", gid);
                Console.WriteLine("Alarm on high egu limit on gid: {0:X}", gid);
            }
            else if (value > alarmMax)
            {
                ah.Type = AlarmType.EGU_MAX;
                ah.Severity = SeverityLevel.MAJOR;
                ah.Message = string.Format("Value on input signal: {0:X} higher than maximum expected value", gid);
                AlarmsEventsProxy.Instance.AddAlarm(ah);
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high egu limit on gid: {0:X}", gid);
                Console.WriteLine("Alarm on high egu limit on gid: {0:X}", gid);
            }

            return retVal;
        }

        private bool CheckForFlatlineAlarm(AnalogLocation analogLoc, float value)
        {
            float previousValue;
            if (previousGeneratorAnalogs.TryGetValue(analogLoc.Analog.GlobalId, out previousValue))
            {
                if (value == previousValue)
                {
                    int counter;
                    if (flatLineAlarmCounter.TryGetValue(analogLoc.Analog.GlobalId, out counter))
                    {
                        flatLineAlarmCounter[analogLoc.Analog.GlobalId]++;
                    }
                    else
                    {
                        flatLineAlarmCounter[analogLoc.Analog.GlobalId] = 0;
                    }

                    if (flatLineAlarmCounter[analogLoc.Analog.GlobalId] == FLAT_LINE_ALARM_TIMEOUT)
                    {
                        flatLineAlarmCounter[analogLoc.Analog.GlobalId] = 0;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}