using SmoothModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACollectingServiceSelfHost
{
	class Program
	{
		static void Main(string[] args)
		{
			ModbusClient modbusClient = new ModbusClient("localhost", 502);
			modbusClient.Connect();
			while (true)
			{
				var veqe = modbusClient.ReadCoils(0, 3);
			}
		}
	}
}
