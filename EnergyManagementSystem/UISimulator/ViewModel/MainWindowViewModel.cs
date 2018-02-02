using EMS.CommonMeasurement;
using SmoothModbus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UISimulator.ViewModel
{
    public class MainWindowViewModel
    {
        private IList<KeyValuePair<long, float>> simulationData1;
        private IList<KeyValuePair<long, float>> simulationData2;
        private IList<KeyValuePair<long, float>> simulationData3;

		private IList<KeyValuePair<long, float>> simulationData4;
		private IList<KeyValuePair<long, float>> simulationData5;
		private IList<KeyValuePair<long, float>> simulationData6;
		private IList<KeyValuePair<long, float>> simulationData7;
		private IList<KeyValuePair<long, float>> simulationData8;
		private IList<KeyValuePair<long, float>> simulationData9;
		private IList<KeyValuePair<long, float>> simulationData10;
		private IList<KeyValuePair<long, float>> simulationData11;
		private IList<KeyValuePair<long, float>> simulationData12;
		private IList<KeyValuePair<long, float>> simulationData13;
		private IList<KeyValuePair<long, float>> simulationData14;
		private IList<KeyValuePair<long, float>> simulationData15;
		private IList<KeyValuePair<long, float>> simulationData16;
		private IList<KeyValuePair<long, float>> simulationData17;
		private IList<KeyValuePair<long, float>> simulationData18;
		private IList<KeyValuePair<long, float>> simulationData19;
		private IList<KeyValuePair<long, float>> simulationData20;

		private IList<KeyValuePair<long, float>> simulationWindData;
		private IList<KeyValuePair<long, float>> simulationSunData;
		private readonly long DURATION = 100;
        private ModbusClient modbusClient;
        private ConvertorHelper convertorHelper;

        public MainWindowViewModel()
        {
            ConnectToSimulator();
            convertorHelper = new ConvertorHelper();
            PopulateSimulationData();
            SimulationData2 = SimulationData1;
            SimulationData3 = SimulationData1;

			SimulationData4 = SimulationData1;
			SimulationData5 = SimulationData1;
			SimulationData6 = SimulationData1;
			SimulationData7 = SimulationData1;
			SimulationData8 = SimulationData1;
			SimulationData9 = SimulationData1;
			SimulationData10 = SimulationData1;
			SimulationData11 = SimulationData1;
			SimulationData12= SimulationData1;
			SimulationData13 = SimulationData1;
			SimulationData14 = SimulationData1;
			SimulationData15 = SimulationData1;
			SimulationData16 = SimulationData1;
			SimulationData17 = SimulationData1;
			SimulationData18 = SimulationData1;
			SimulationData19 = SimulationData1;
			SimulationData20 = SimulationData1;

			Task task = new Task(StartSimulation);
            task.Start();
        }

        private void ConnectToSimulator()
        {
            try
            {
                modbusClient = new ModbusClient("localhost", 502);
                modbusClient.Connect();
            }
            catch (SocketException)
            {
                Thread.Sleep(2000);
                ConnectToSimulator();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void StartSimulation()
        {
            for (int i = 0; i < DURATION; i++)
            {
                modbusClient.WriteSingleRegister(0, simulationData1[i].Value);
                modbusClient.WriteSingleRegister(2, simulationData2[i].Value);
                modbusClient.WriteSingleRegister(4, SimulationData3[i].Value);

				modbusClient.WriteSingleRegister(6, simulationData4[i].Value);
				modbusClient.WriteSingleRegister(8, simulationData5[i].Value);
				modbusClient.WriteSingleRegister(10, simulationData6[i].Value);
				modbusClient.WriteSingleRegister(12, simulationData7[i].Value);
				modbusClient.WriteSingleRegister(14, simulationData8[i].Value);
				modbusClient.WriteSingleRegister(16, simulationData9[i].Value);
				modbusClient.WriteSingleRegister(18, simulationData10[i].Value);
				modbusClient.WriteSingleRegister(20, simulationData11[i].Value);
				modbusClient.WriteSingleRegister(22, simulationData12[i].Value);
				modbusClient.WriteSingleRegister(24, simulationData13[i].Value);
				modbusClient.WriteSingleRegister(26, simulationData14[i].Value);
				modbusClient.WriteSingleRegister(28, simulationData15[i].Value);
				modbusClient.WriteSingleRegister(30, simulationData16[i].Value);
				modbusClient.WriteSingleRegister(32, simulationData17[i].Value);
				modbusClient.WriteSingleRegister(34, simulationData18[i].Value);
				modbusClient.WriteSingleRegister(36, simulationData19[i].Value);
				modbusClient.WriteSingleRegister(38, simulationData20[i].Value);

				modbusClient.WriteSingleRegister(100, simulationWindData[i].Value);
				modbusClient.WriteSingleRegister(102, simulationSunData[i].Value);
                Thread.Sleep(1);
            }

            StartSimulation();
        }

        public IList<KeyValuePair<long, float>> SimulationData1
        {
            get
            {
                return simulationData1;
            }

            set
            {
                simulationData1 = value;
            }
        }

        public IList<KeyValuePair<long, float>> SimulationData2
        {
            get
            {
                return simulationData2;
            }

            set
            {
                simulationData2 = value;
            }
        }

        public IList<KeyValuePair<long, float>> SimulationData3
        {
            get
            {
                return simulationData3;
            }

            set
            {
                simulationData3 = value;
            }
        }
		public IList<KeyValuePair<long, float>> SimulationData4
		{
			get
			{
				return simulationData4;
			}

			set
			{
				simulationData4 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData5
		{
			get
			{
				return simulationData5;
			}

			set
			{
				simulationData5 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData6
		{
			get
			{
				return simulationData6;
			}

			set
			{
				simulationData6 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData7
		{
			get
			{
				return simulationData7;
			}

			set
			{
				simulationData7 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData8
		{
			get
			{
				return simulationData8;
			}

			set
			{
				simulationData8 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData9
		{
			get
			{
				return simulationData9;
			}

			set
			{
				simulationData9 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData10
		{
			get
			{
				return simulationData10;
			}

			set
			{
				simulationData10 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData11
		{
			get
			{
				return simulationData11;
			}

			set
			{
				simulationData11 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData12
		{
			get
			{
				return simulationData12;
			}

			set
			{
				simulationData12 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData13
		{
			get
			{
				return simulationData13;
			}

			set
			{
				simulationData13 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData14
		{
			get
			{
				return simulationData14;
			}

			set
			{
				simulationData14 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData15
		{
			get
			{
				return simulationData15;
			}

			set
			{
				simulationData15 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData16
		{
			get
			{
				return simulationData16;
			}

			set
			{
				simulationData16 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData17
		{
			get
			{
				return simulationData17;
			}

			set
			{
				simulationData17 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData18
		{
			get
			{
				return simulationData18;
			}

			set
			{
				simulationData18 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData19
		{
			get
			{
				return simulationData19;
			}

			set
			{
				simulationData19 = value;
			}
		}
		public IList<KeyValuePair<long, float>> SimulationData20
		{
			get
			{
				return simulationData20;
			}

			set
			{
				simulationData20 = value;
			}
		}

		public IList<KeyValuePair<long, float>> SimulationWindData
        {
            get
            {
                return simulationWindData;
            }

            set
            {
                simulationWindData = value;
            }
        }

		public IList<KeyValuePair<long, float>> SimulationSunData
		{
			get
			{
				return simulationSunData;
			}

			set
			{
				simulationSunData = value;
			}
		}

		private void PopulateSimulationData()
        {
            SimulationData1 = new List<KeyValuePair<long, float>>();
            SimulationWindData = new List<KeyValuePair<long, float>>();
			SimulationSunData = new List<KeyValuePair<long, float>>();
            for (int i = 0; i < DURATION; i++)
            {
                float value = (float)SimulationFunction(i);
                SimulationData1.Add(new KeyValuePair<long, float>(i, value));

                float windValue = (float)SimulateWind(i);
				float sunValue = (float)SimulateSun(i);
                SimulationWindData.Add(new KeyValuePair<long, float>(i, windValue));
				SimulationSunData.Add(new KeyValuePair<long, float>(i, sunValue));
            }
        }

        private double SimulationFunction(int x)
        {

            double v = 1500 * (Math.Sin(x) * (Math.Sin(x) - 1) + Math.Cos(x) / 2);
			if (v < 10)
			{
				v = 10;
			}
			else if (v > 4095)
			{
				v = 4095;
			}

			return v;
		}

        private double SimulateWind(int x)
        {
            return Math.Sin((float)x / 10f) * 13.5 + 13.5;
        }

		private double SimulateSun(int x)
		{
			return Math.Sin((float)x / 10f) * 50 + 50;
		}
	}
}
