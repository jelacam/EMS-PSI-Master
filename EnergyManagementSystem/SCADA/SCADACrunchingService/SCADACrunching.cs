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
        private List<AnalogLocation> listOfAnalog;

        /// <summary>
        /// list for storing copy of AnalogLocation values
        /// </summary>
        private List<AnalogLocation> listOfAnalogCopy;

        private UpdateResult updateResult;

        private ITransactionCallback transactionCallback;

		/// <summary>
		/// instance of ConvertorHelper class
		/// </summary>
		private ConvertorHelper convertorHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="SCADACrunching" /> class
		/// </summary>
		public SCADACrunching()
        {
			this.convertorHelper = new ConvertorHelper();

			// TODO treba izmeniti kad se napravi transakcija sa NMS-om
			this.listOfAnalog = new List<AnalogLocation>();
            this.listOfAnalogCopy = new List<AnalogLocation>();

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

                this.listOfAnalogCopy = new List<AnalogLocation>();
                Analog analog = null;
                int i = 0; // analog counter for address

                foreach (ResourceDescription analogRd in delta.InsertOperations)
                {
                    analog = ResourcesDescriptionConverter.ConvertToAnalog(analogRd);

                    this.listOfAnalogCopy.Add(new AnalogLocation()
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
                this.listOfAnalogCopy.Clear();
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
            foreach (AnalogLocation analogLoc in this.listOfAnalog)
            {
                // startIndex = 2 because first two bytes a metadata
                float[] values = ModbusHelper.GetValueFromByteArray<float>(value, analogLoc.Length * 2, 2 + analogLoc.StartAddress * 2);

                alarmRaw = this.CheckForRawAlarms(values[0], convertorHelper.MinRaw, convertorHelper.MaxRaw, analogLoc.Analog.PowerSystemResource);
                if (alarmRaw == false)
                {
                    eguVal = convertorHelper.ConvertFromRawToEGUValue(values[0], analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue);
                    alarmEGU = this.CheckForEGUAlarms(eguVal, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue, analogLoc.Analog.PowerSystemResource);

                    if (alarmEGU == false)
                    {
                        MeasurementUnit measUnit = new MeasurementUnit();
                        measUnit.Gid = analogLoc.Analog.PowerSystemResource;
                        measUnit.MinValue = analogLoc.Analog.MinValue;
                        measUnit.MaxValue = analogLoc.Analog.MaxValue;
                        measUnit.CurrentValue = eguVal;
                        // measUnit.CurrentValue = values[0];
                        listOfMeasUnit.Add(measUnit);
                    }
                }
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
            Console.WriteLine("Test");
        }

        /// <summary>
        /// Checking for alarms on egu value
        /// </summary>
        /// <param name="value">value to check</param>
        /// <param name="minRaw">low limit</param>
        /// <param name="maxRaw">high limit</param>
        /// <param name="gid">gid of measurement</param>
        /// <returns></returns>
        private bool CheckForRawAlarms(float value, float minRaw, float maxRaw, long gid)
        {
            bool retVal = false;
            if (value < minRaw)
            {
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low raw limit on gid: {0}", gid);
                Console.WriteLine("Alarm on low raw limit on gid: {0}", gid);
            }

            if (value > maxRaw)
            {
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
        /// <returns></returns>
        private bool CheckForEGUAlarms(float value, float minEGU, float maxEGU, long gid)
        {
            bool retVal = false;
            if (value < minEGU)
            {
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on low egu limit on gid: {0}", gid);
                Console.WriteLine("Alarm on low egu limit on gid: {0}", gid);
            }

            if (value > maxEGU)
            {
                retVal = true;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on high egu limit on gid: {0}", gid);
                Console.WriteLine("Alarm on high egu limit on gid: {0}", gid);
            }

            return retVal;
        }
    }
}