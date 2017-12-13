//-----------------------------------------------------------------------
// <copyright file="SCADACrunching.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACrunchingService
{
	using System;
	using System.ServiceModel;
	using EMS.ServiceContracts;
	using SmoothModbus;
	using System.Collections.Generic;
	using NetworkModelService.DataModel.Meas;
	using CommonMeasurement;
	using Common;

	/// <summary>
	/// SCADACrunching component logic
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class SCADACrunching : IScadaCRContract
	{
		private List<AnalogLocation> listOfAnalog;

		public SCADACrunching()
		{
			//TODO treba izmeniti kad se napravi transakcija sa NMS-om
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
					StartAddress = i * 4, // flaot value 4bytes
					Length = 4
				});
			}
		}

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
			Console.WriteLine("Byte count: {0}", arrayLength);


			List<MeasurementUnit> listOfMeasUnit = new List<MeasurementUnit>();
			foreach (AnalogLocation analogLoc in listOfAnalog)
			{
				// startIndex = 2 because first two bytes a metadata
				float[] values = ModbusHelper.GetValueFromByteArray<float>(value, analogLoc.Length, 2 + analogLoc.StartAddress);
				CheckForAlarms(values[0], analogLoc.Analog);

				MeasurementUnit measUnit = new MeasurementUnit();
				measUnit.Gid = analogLoc.Analog.PowerSystemResource;
				measUnit.CurrentValue = values[0];
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
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Successfuly sent list to CE, list with {0} items.", listOfMeasUnit.Count);
				Console.WriteLine("Successfuly sent list to CE, list with {0} items.", listOfMeasUnit.Count);
			}

			return isSuccess;
		}

		private void CheckForAlarms(float value, Analog analog)
		{
			if (value < analog.MinValue || value > analog.MaxValue)
			{
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Alarm on Gid = {0}", analog.GlobalId);
				Console.WriteLine("Alarm on Gid = {0}", analog.GlobalId);

			}
		}

		/// <summary>
		/// Test method
		/// </summary>
		public void Test()
		{
			Console.WriteLine("Test");
		}
	}
}
