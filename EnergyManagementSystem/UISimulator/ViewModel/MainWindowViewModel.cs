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
        private IList<KeyValuePair<long, float>> simulationWindData;
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

                modbusClient.WriteSingleRegister(100, simulationWindData[i].Value);
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

        private void PopulateSimulationData()
        {
            SimulationData1 = new List<KeyValuePair<long, float>>();
            SimulationWindData = new List<KeyValuePair<long, float>>();
            for (int i = 0; i < DURATION; i++)
            {
                float value = (float)SimulationFunction(i);
                SimulationData1.Add(new KeyValuePair<long, float>(i, value));

                float windValue = (float)SimulateWind(i);
                SimulationWindData.Add(new KeyValuePair<long, float>(i, windValue));
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
    }
}
