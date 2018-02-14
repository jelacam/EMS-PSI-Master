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
        #region Fields

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

        private readonly long DURATION = 24;
        private ModbusClient modbusClient;
        private ConvertorHelper convertorHelper;

        private object lockObj = new object();

        #endregion Fields

        public MainWindowViewModel()
        {
            ConnectToSimulator();
            convertorHelper = new ConvertorHelper();
            PopulateSimulationData();

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
            lock (lockObj)
            {
                for (int i = 0; i < DURATION; i++)
                {
                    modbusClient.WriteSingleRegister(0, SimulationData1[i].Value);
                    modbusClient.WriteSingleRegister(2, SimulationData2[i].Value);
                    modbusClient.WriteSingleRegister(4, SimulationData3[i].Value);
                    modbusClient.WriteSingleRegister(6, SimulationData4[i].Value);
                    modbusClient.WriteSingleRegister(8, SimulationData5[i].Value);
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
                    Thread.Sleep(5000);
                }
            }

            StartSimulation();
        }

        #region SimulationData properties

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

        #endregion SimulationData properties

        private void PopulateSimulationData()
        {
            SimulationData1 = new List<KeyValuePair<long, float>>();
            SimulationData2 = new List<KeyValuePair<long, float>>();
            SimulationData3 = new List<KeyValuePair<long, float>>();
            SimulationData4 = new List<KeyValuePair<long, float>>();
            SimulationData5 = new List<KeyValuePair<long, float>>();
            SimulationData6 = new List<KeyValuePair<long, float>>();
            SimulationData7 = new List<KeyValuePair<long, float>>();
            SimulationData8 = new List<KeyValuePair<long, float>>();
            SimulationData9 = new List<KeyValuePair<long, float>>();
            SimulationData10 = new List<KeyValuePair<long, float>>();
            SimulationData11 = new List<KeyValuePair<long, float>>();
            SimulationData12 = new List<KeyValuePair<long, float>>();
            SimulationData13 = new List<KeyValuePair<long, float>>();
            SimulationData14 = new List<KeyValuePair<long, float>>();
            SimulationData15 = new List<KeyValuePair<long, float>>();
            SimulationData16 = new List<KeyValuePair<long, float>>();
            SimulationData17 = new List<KeyValuePair<long, float>>();
            SimulationData18 = new List<KeyValuePair<long, float>>();
            SimulationData19 = new List<KeyValuePair<long, float>>();
            SimulationData20 = new List<KeyValuePair<long, float>>();

            SimulationWindData = new List<KeyValuePair<long, float>>();
            SimulationSunData = new List<KeyValuePair<long, float>>();

            float value = 0;
            float windValue = 0;
            float sunValue = 0;

            for (int i = 0; i < DURATION; i++)
            {
                value = (float)SimulationFunction1(i);
                SimulationData1.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction2(i);
                SimulationData2.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction3(i);
                SimulationData3.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction4(i);
                SimulationData4.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction5(i);
                SimulationData5.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction6(i);
                SimulationData6.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction7(i);
                SimulationData7.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction8(i);
                SimulationData8.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction9(i);
                SimulationData9.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction10(i);
                SimulationData10.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction11(i);
                SimulationData11.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction12(i);
                SimulationData12.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction13(i);
                SimulationData13.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction14(i);
                SimulationData14.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction15(i);
                SimulationData15.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction16(i);
                SimulationData16.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction17(i);
                SimulationData17.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction18(i);
                SimulationData18.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction19(i);
                SimulationData19.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationFunction20(i);
                SimulationData20.Add(new KeyValuePair<long, float>(i, value));

                windValue = (float)SimulateWind(i);
                SimulationWindData.Add(new KeyValuePair<long, float>(i, windValue));
                sunValue = (float)SimulateSun(i);
                SimulationSunData.Add(new KeyValuePair<long, float>(i, sunValue));
            }
        }

        #region Simulation functions

        private double SimulationFunction1(int x)
        {
            // y = -1.628408 + 4.600775 * x - 1.466326 * x ^ 2 + 0.1952566 * x ^ 3 - 0.00915698 * x ^ 4

            double v = 100 + 10 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));
            //double v = 1500 * (Math.Sin(x) * (Math.Sin(x) - 1) + Math.Cos(x) / 2);
            //double v = 1 / 10 * x;

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

        private float SimulationFunction2(int x)
        {
            double retVal = 0;
            retVal = 200 + 20 * (- 9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction3(int x)
        {
            double retVal = 0;
            retVal = 300 + 30 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction4(int x)
        {
            double retVal = 0;
            retVal = 400 + 40 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction5(int x)
        {
            double retVal = 0;
            retVal = 500 + 50 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction6(int x)
        {
            double retVal = 0;
            retVal = 600 + 60 * (- 9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction7(int x)
        {
            double retVal = 0;
            retVal = 700 + 70 * (- 9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction8(int x)
        {
            double retVal = 0;
            retVal = 800 + 80 * (- 9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction9(int x)
        {
            double retVal = 0;
            retVal = 900 + 90 * (- 9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction10(int x)
        {
            double retVal = 0;
            retVal = 1000 + 100 * (- 9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return (float)retVal;
        }

        private float SimulationFunction11(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1100 + 100*(1 / 10 * x);
            }
            else if (x > 8 && x <= 12)
            {
                retVal = 1100 + 100 * (x - 7 - 2 / 10);
            }
            else if (x > 12 && x <= 21)
            {
                retVal = 1100 + 100 * (5 - 1 / 5);
            }
            else if (x > 21 && x <= 24)
            {
                retVal = 1100 + 100 * ((-(3 * x) / 2) + 36 + 3 / 10);
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction12(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1200 + 100 * (1 / 11 * x);
            }
            else if (x > 8 && x <= 12)
            {
                retVal = 1200 + 100 * (x - 7 - 2 / 11);
            }
            else if (x > 12 && x <= 21)
            {
                retVal = 1200 + 100 * (5 - 1 / 4);
            }
            else if (x > 21 && x <= 24)
            {
                retVal = 1200 + 100 * ((-((float)1.8 * x)) + 36 + 3 / 10);
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction13(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1300 + 100 * (1 / 12 * x);
            }
            else if (x > 8 && x <= 12)
            {
                retVal = 1300 + 100 * (x - 7 - 2 / 12);
            }
            else if (x > 12 && x <= 21)
            {
                retVal = 1300 + 100 * (5 - 1 / 7);
            }
            else if (x > 21 && x <= 24)
            {
                retVal = 1300 + 100 * ((-((float)1.7 * x)) + 36 + 3 / 10);
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction14(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1400 + 100 * (1 / 13 * x);
            }
            else if (x > 8 && x <= 12)
            {
                retVal = 1400 + 100 * (x - 7 - 2 / 13);
            }
            else if (x > 12 && x <= 21)
            {
                retVal = 1400 + 100 * (5 - 1 / 8);
            }
            else if (x > 21 && x <= 24)
            {
                retVal = 1400 + 100 * ((-((float)1.8 * x)) + 36 + 3 / 13);
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction15(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1500 + 100 * (1 / 14 * x);
            }
            else if (x > 8 && x <= 12)
            {
                retVal = 1500 + 100 * (x - 7 - 2 / 14);
            }
            else if (x > 12 && x <= 21)
            {
                retVal = 1500 + 100 * (5 - 1 / 9);
            }
            else if (x > 21 && x <= 24)
            {
                retVal = 1500 + 100 * ((-((float)1.9 * x)) + 36 + 3 / 14);
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction16(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1600 + 4;
            }
            else if (x > 8 && x <= 10)
            {
                retVal = 1600 - 4 * x / 3 + 14 + 2 / 3;
            }
            else if (x > 10 && x <= 17)
            {
                retVal = 1600 + 1 + 1 / 3 / 9;
            }
            else if (x > 17 && x <= 19)
            {
                retVal = 1600 - 21 - 1 / 3 + 4 * x / 3;
            }
            else if (x > 19 && x <= 24)
            {
                retVal = 1600 + 4;
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction17(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1700 + 4;
            }
            else if (x > 8 && x <= 10)
            {
                retVal = 1700 - (float)1.3 * x + 14 + 2 / 3;
            }
            else if (x > 10 && x <= 17)
            {
                retVal = 1700 + 1 + 1 / 3 / 9;
            }
            else if (x > 17 && x <= 19)
            {
                retVal = 1700 - 21 - 1 / 3 + (float)1.3 * x;
            }
            else if (x > 19 && x <= 24)
            {
                retVal = 1700 + 4;
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction18(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1800 + 4;
            }
            else if (x > 8 && x <= 10)
            {
                retVal = 1800 - (float)1.3 * x + 14 + 2 / 3;
            }
            else if (x > 10 && x <= 17)
            {
                retVal = 1800 + 1 + 1 / 3 / 9;
            }
            else if (x > 17 && x <= 19)
            {
                retVal = 1800 - 21 - 1 / 3 + (float)1.3 * x;
            }
            else if (x > 19 && x <= 24)
            {
                retVal = 1800 + 4;
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction19(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 1900 + 4;
            }
            else if (x > 8 && x <= 10)
            {
                retVal = 1900 - (float)1.3 * x + 14 + 2 / 3;
            }
            else if (x > 10 && x <= 17)
            {
                retVal = 1900 + 1 + 1 / 3 / 9;
            }
            else if (x > 17 && x <= 19)
            {
                retVal = 1900 - 21 - 1 / 3 + (float)1.3 * x;
            }
            else if (x > 19 && x <= 24)
            {
                retVal = 1900 + 4;
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private float SimulationFunction20(int x)
        {
            float retVal = 0;

            if (x >= 0 && x <= 8)
            {
                retVal = 2000 + 4;
            }
            else if (x > 8 && x <= 10)
            {
                retVal = 2000 - (float)1.3 * x + 14 + 2 / 3;
            }
            else if (x > 10 && x <= 17)
            {
                retVal = 2000 + 1 + 1 / 3 / 9;
            }
            else if (x > 17 && x <= 19)
            {
                retVal = 2000 - 21 - 1 / 3 + (float)1.3 * x;
            }
            else if (x > 19 && x <= 24)
            {
                retVal = 2000 + 4;
            }

            if (retVal < 10)
            {
                retVal = 10;
            }
            else if (retVal > 4090)
            {
                retVal = 4090;
            }
            return retVal;
        }

        private double SimulateWind(int x)
        {
            return (float)Math.Sin(x / 10f) * 13.5f + 13.5f;
        }

        private double SimulateSun(int x)
        {
            return (float)Math.Sin(x / 10f) * 50f + 50f;
        }

        #endregion Simulation functions
    }
}