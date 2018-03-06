using System.Collections.Generic;

namespace UISimulator
{
	public class SimulationData
	{
		private List<KeyValuePair<long, float>> data = new List<KeyValuePair<long, float>>();
		private string name;

		public SimulationData(string name)
		{
			Name = name;
		}

		public List<KeyValuePair<long, float>> Data
		{
			get
			{
				return data;
			}

			set
			{
				data = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}
	}
}
