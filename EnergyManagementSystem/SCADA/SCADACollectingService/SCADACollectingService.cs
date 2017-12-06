using SmoothModbus;
using System;

namespace SCADACollectingService
{
	public class SCADACollectingService:IDisposable
	{
		ModbusClient modbusClient;

		public SCADACollectingService(string ipAddress, int port)
		{
			modbusClient = new ModbusClient();
			modbusClient.Connect(ipAddress, port);
		}

		public byte[] ReadHoldingRegisters()
		{
			return modbusClient.ReadInputRegisters(0, 10);
		}

		public void Dispose()
		{
			modbusClient.Disconnect();
		}
	}
}
