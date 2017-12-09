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

		ModbusClient modbusClient;

		public SCADACollecting()
		{
			modbusClient = new ModbusClient("localhost",502);
			modbusClient.Connect();
		}

        /// <summary>
        /// Method for getting data values from simulator
        /// </summary>
        /// <returns> true if values are successfully returned </returns>
        public bool GetDataFromSimulator()
        {
			var values = modbusClient.ReadHoldingRegisters(0, 5);
			return true;
        }
    }
}
