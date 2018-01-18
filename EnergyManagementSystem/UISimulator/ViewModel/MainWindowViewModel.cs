using EMS.CommonMeasurement;
using SmoothModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UISimulator.ViewModel
{
    public class MainWindowViewModel
    {
        private IList<KeyValuePair<long, float>> simulationData1;
        private IList<KeyValuePair<long, float>> simulationData2;
        private IList<KeyValuePair<long, float>> simulationData3;
        private readonly long DURATION = 100;
        private ModbusClient modbusClient;
        private ConvertorHelper convertorHelper;

        public MainWindowViewModel()
        {
            try
            {
                modbusClient = new ModbusClient("localhost", 502);
                modbusClient.Connect();
            }
            catch (Exception ex )
            {
                throw ex;
            }
            convertorHelper = new ConvertorHelper();
            PopulateSimulationData();
            SimulationData2 = SimulationData1;
            SimulationData3 = SimulationData1;

            Task task = new Task(StartSimulation);
            task.Start();
        }

        private void StartSimulation()
        {
            for(int i = 0; i < DURATION; i++)
            {
                modbusClient.WriteSingleRegister(0, simulationData1[i].Value);
                modbusClient.WriteSingleRegister(2, simulationData2[i].Value);
                modbusClient.WriteSingleRegister(4, SimulationData3[i].Value);
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

        private void PopulateSimulationData()
        {
            SimulationData1 = new List<KeyValuePair<long, float>>();
            for (int i = 0; i < DURATION; i++)
            {
                float value = (float)SimulationFunction(i);
                SimulationData1.Add(new KeyValuePair<long, float>(i, value));
            }
        }

        private double SimulationFunction(int x)
        {
            double v = 3000 * (Math.Sin(x) * (Math.Sin(x) - 1) + Math.Cos(x) / 2)  ;
            if(v < 100)
            {
                v = 100;
            }
            return v;
        }
    }
}
