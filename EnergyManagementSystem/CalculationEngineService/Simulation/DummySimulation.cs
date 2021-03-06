﻿using EMS.CommonMeasurement;
using System.Collections.Generic;

namespace EMS.Services.CalculationEngineService.Simulation
{
	public class DummySimulation
	{
		private List<SimulationData> simulationDataList;
		private readonly long DURATION = 24;

		public DummySimulation()
		{
			PopulateSimulationData();
		}

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

		public float GetCurrentConsumption(int hour)
		{
			float totalConsumption = 0;
			for (int i = 0; i < 20; i++)
			{
				totalConsumption += SimulationDataList[i].Data[hour].Value;
			}

			return totalConsumption;
		}

		public float GetWindSpeed(int hour)
		{
			return SimulationDataList[20].Data[hour].Value;
		}

		public float GetSunLight(int hour)
		{
			return SimulationDataList[21].Data[hour].Value;
		}

		private void PopulateSimulationData()
		{
			SimulationDataList = new List<SimulationData>();
			SimulationDataList.Add(new SimulationData("SimulationData1"));
			SimulationDataList.Add(new SimulationData("SimulationData2"));
			SimulationDataList.Add(new SimulationData("SimulationData3"));
			SimulationDataList.Add(new SimulationData("SimulationData4"));
			SimulationDataList.Add(new SimulationData("SimulationData5"));
			SimulationDataList.Add(new SimulationData("SimulationData6"));
			SimulationDataList.Add(new SimulationData("SimulationData7"));
			SimulationDataList.Add(new SimulationData("SimulationData8"));
			SimulationDataList.Add(new SimulationData("SimulationData9"));
			SimulationDataList.Add(new SimulationData("SimulationData10"));
			SimulationDataList.Add(new SimulationData("SimulationData11"));
			SimulationDataList.Add(new SimulationData("SimulationData12"));
			SimulationDataList.Add(new SimulationData("SimulationData13"));
			SimulationDataList.Add(new SimulationData("SimulationData14"));
			SimulationDataList.Add(new SimulationData("SimulationData15"));
			SimulationDataList.Add(new SimulationData("SimulationData16"));
			SimulationDataList.Add(new SimulationData("SimulationData17"));
			SimulationDataList.Add(new SimulationData("SimulationData18"));
			SimulationDataList.Add(new SimulationData("SimulationData19"));
			SimulationDataList.Add(new SimulationData("SimulationData20"));

			SimulationDataList.Add(new SimulationData("SimulationWindData"));
			SimulationDataList.Add(new SimulationData("SimulationSunData"));

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
