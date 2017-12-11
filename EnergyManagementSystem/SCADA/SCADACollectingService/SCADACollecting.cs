//-----------------------------------------------------------------------
// <copyright file="SCADACollecting.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACollectingService
{
	using ServiceContracts;
	using SmoothModbus;

	/// <summary>
	/// SCADACollecting component logic
	/// </summary>
	public class SCADACollecting : IScadaCLContract
	{
		/// <summary>
		/// Instance of ModbusClient
		/// </summary>
		private ModbusClient modbusClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="SCADACollecting" /> class
		/// </summary>
		public SCADACollecting()
		{
			this.modbusClient = new ModbusClient("localhost", 502);
			this.modbusClient.Connect();
		}

		/// <summary>
		/// Method for getting data values from simulator
		/// </summary>
		/// <returns> true if values are successfully returned </returns>
		public bool GetDataFromSimulator()
		{
			var values = this.modbusClient.ReadHoldingRegisters(0, 5);
			ScadaCRProxy.Instance.SendValues(values);

			return true;
		}
	}
}
