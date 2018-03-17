using EMS.CommonMeasurement;
using SmoothModbus;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace UISimulator.ViewModel
{
	public class MainWindowViewModel
    {
		#region Fields

		private List<SimulationData> simulationDataList;
       
        private readonly long DURATION = 24;
        private ModbusClient modbusClient;
        private ConvertorHelper convertorHelper;

        private object lockObj = new object();

		public List<SimulationData> SimulationDataList
		{
			get
			{
				return simulationDataList;
			}

			set
			{
				simulationDataList = value;
			}
		}



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
					for(int j = 0; j < 20; j++)
					{
						modbusClient.WriteSingleRegister((ushort)(j * 2), SimulationDataList[j].Data[i].Value);
					}

					modbusClient.WriteSingleRegister(100, SimulationDataList[20].Data[i].Value); //SimulationWindData
					modbusClient.WriteSingleRegister(102, SimulationDataList[21].Data[i].Value); //SimulationSunData
                    Thread.Sleep(3000);
                }
            }

            StartSimulation();
        }

		private void PopulateSimulationData()
        {
			SimulationDataList = new List<SimulationData>();
			SimulationDataList.Add( new SimulationData("Household1"));
            SimulationDataList.Add( new SimulationData("Household2"));
            SimulationDataList.Add( new SimulationData("Household3"));
            SimulationDataList.Add( new SimulationData("Household4"));
            SimulationDataList.Add( new SimulationData("Household5"));
            SimulationDataList.Add( new SimulationData("Household6"));
            SimulationDataList.Add( new SimulationData("Household7"));
            SimulationDataList.Add( new SimulationData("Household8"));
            SimulationDataList.Add( new SimulationData("Household9"));
            SimulationDataList.Add( new SimulationData("Household10"));
            SimulationDataList.Add( new SimulationData("Administration1"));
            SimulationDataList.Add( new SimulationData("Administration2"));
            SimulationDataList.Add( new SimulationData("Administration3"));
            SimulationDataList.Add( new SimulationData("Administration4"));
            SimulationDataList.Add( new SimulationData("Administration5"));
            SimulationDataList.Add( new SimulationData("Industry1"));
            SimulationDataList.Add( new SimulationData("Industry2"));
            SimulationDataList.Add( new SimulationData("Industry3"));
            SimulationDataList.Add( new SimulationData("Industry4"));
			SimulationDataList.Add( new SimulationData("Industry5"));

			SimulationDataList.Add( new SimulationData("SimulationWindData"));
			SimulationDataList.Add( new SimulationData("SimulationSunData"));

            float value = 0;
            float windValue = 0;
            float sunValue = 0;

			for (int i = 0; i < DURATION; i++)
			{
				value = (float)SimulationHelper.SimulationFunction1(i);
				SimulationDataList[0].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction2(i);
				SimulationDataList[1].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction3(i);
				SimulationDataList[2].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction4(i);
				SimulationDataList[3].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction5(i);
				SimulationDataList[4].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction6(i);
				SimulationDataList[5].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction7(i);
				SimulationDataList[6].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction8(i);
				SimulationDataList[7].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction9(i);
				SimulationDataList[8].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction10(i);
				SimulationDataList[9].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction11(i);
				SimulationDataList[10].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction12(i);
				SimulationDataList[11].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction13(i);
				SimulationDataList[12].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction14(i);
				SimulationDataList[13].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction15(i);
				SimulationDataList[14].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction16(i);
				SimulationDataList[15].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction17(i);
				SimulationDataList[16].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction18(i);
				SimulationDataList[17].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction19(i);
				SimulationDataList[18].Data.Add(new KeyValuePair<long, float>(i, value));
                value = SimulationHelper.SimulationFunction20(i);
				SimulationDataList[19].Data.Add(new KeyValuePair<long, float>(i, value));

                windValue = (float)SimulationHelper.SimulateWind(i);
				SimulationDataList[20].Data.Add(new KeyValuePair<long, float>(i, windValue));
                sunValue = (float)SimulationHelper.SimulateSun(i);
				SimulationDataList[21].Data.Add(new KeyValuePair<long, float>(i, sunValue));
            }
        }

    }
}