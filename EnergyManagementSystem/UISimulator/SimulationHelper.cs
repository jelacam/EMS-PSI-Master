using System;

namespace UISimulator
{
	public static class SimulationHelper
	{

		public static double SimulationFunction1(int x)
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

		public static float SimulationFunction2(int x)
		{
			double retVal = 0;
			retVal = 200 + 20 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

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

		public static float SimulationFunction3(int x)
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

		public static float SimulationFunction4(int x)
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

		public static float SimulationFunction5(int x)
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

		public static float SimulationFunction6(int x)
		{
			double retVal = 0;
			retVal = 600 + 60 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

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

		public static float SimulationFunction7(int x)
		{
			double retVal = 0;
			retVal = 700 + 70 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

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

		public static float SimulationFunction8(int x)
		{
			double retVal = 0;
			retVal = 800 + 80 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

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

		public static float SimulationFunction9(int x)
		{
			double retVal = 0;
			retVal = 900 + 90 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

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

		public static float SimulationFunction10(int x)
		{
			double retVal = 0;
			retVal = 1000 + 100 * (-9.924232 * x + 3.095262 * Math.Pow(x, 2) - 0.3209949 * Math.Pow(x, 3) + 0.01411989 * Math.Pow(x, 4) - 0.0002267766 * Math.Pow(x, 5));

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

		public static float SimulationFunction11(int x)
		{
			float retVal = 0;

			if (x >= 0 && x <= 8)
			{
				retVal = 1100 + 100 * (1 / 10 * x);
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

		public static float SimulationFunction12(int x)
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

		public static float SimulationFunction13(int x)
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

		public static float SimulationFunction14(int x)
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

		public static float SimulationFunction15(int x)
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

		public static float SimulationFunction16(int x)
		{
			float retVal = 0;

			if (x >= 0 && x <= 8)
			{
				retVal = 1600 + 4;
			}
			else if (x > 8 && x <= 10)
			{
				retVal = 1200 - 4 * x / 3 + 14 + 2 / 3;
			}
			else if (x > 10 && x <= 17)
			{
				retVal = 1200 + 1 + 1 / 3 / 9;
			}
			else if (x > 17 && x <= 19)
			{
				retVal = 1200 - 21 - 1 / 3 + 4 * x / 3;
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

		public static float SimulationFunction17(int x)
		{
			float retVal = 0;

			if (x >= 0 && x <= 8)
			{
				retVal = 1700 + 4;
			}
			else if (x > 8 && x <= 10)
			{
				retVal = 1100 - (float)1.3 * x + 14 + 2 / 3;
			}
			else if (x > 10 && x <= 17)
			{
				retVal = 1100 + 1 + 1 / 3 / 9;
			}
			else if (x > 17 && x <= 19)
			{
				retVal = 1100 - 21 - 1 / 3 + (float)1.3 * x;
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

		public static float SimulationFunction18(int x)
		{
			float retVal = 0;

			if (x >= 0 && x <= 8)
			{
				retVal = 1800 + 4;
			}
			else if (x > 8 && x <= 10)
			{
				retVal = 1300 - (float)1.3 * x + 14 + 2 / 3;
			}
			else if (x > 10 && x <= 17)
			{
				retVal = 1300 + 1 + 1 / 3 / 9;
			}
			else if (x > 17 && x <= 19)
			{
				retVal = 1300 - 21 - 1 / 3 + (float)1.3 * x;
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

		public static float SimulationFunction19(int x)
		{
			float retVal = 0;

			if (x >= 0 && x <= 8)
			{
				retVal = 1900 + 4;
			}
			else if (x > 8 && x <= 10)
			{
				retVal = 1500 - (float)1.3 * x + 14 + 2 / 3;
			}
			else if (x > 10 && x <= 17)
			{
				retVal = 1500 + 1 + 1 / 3 / 9;
			}
			else if (x > 17 && x <= 19)
			{
				retVal = 1500 - 21 - 1 / 3 + (float)1.3 * x;
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

		public static float SimulationFunction20(int x)
		{
			float retVal = 0;

			if (x >= 0 && x <= 8)
			{
				retVal = 2000 + 4;
			}
			else if (x > 8 && x <= 10)
			{
				retVal = 1300 - (float)1.3 * x + 14 + 2 / 3;
			}
			else if (x > 10 && x <= 17)
			{
				retVal = 1300 + 1 + 1 / 3 / 9;
			}
			else if (x > 17 && x <= 19)
			{
				retVal = 1300 - 21 - 1 / 3 + (float)1.3 * x;
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

		public static double SimulateWind(int x)
		{
			return (float)Math.Sin(x / 10f) * 13.5f + 13.5f;
		}

		public static double SimulateSun(int x)
		{
			return (float)Math.Sin(x / 10f) * 50f + 50f;
		}
	}
}
