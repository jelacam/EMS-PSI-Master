using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.ServiceContracts
{
	public class CalculationEngineUIProxy : ICalculationEngineUIContract, IDisposable
	{
		/// <summary>
		/// proxy object
		/// </summary>
		private static ICalculationEngineUIContract proxy;

		/// <summary>
		/// ChannelFactory object
		/// </summary>
		private static ChannelFactory<ICalculationEngineUIContract> factory;

		/// <summary>
		/// Gets or sets instance of ICalculationEngineContract
		/// </summary>
		public static ICalculationEngineUIContract Instance
		{
			get
			{
				if (proxy == null)
				{
					factory = new ChannelFactory<ICalculationEngineUIContract>("*");
					proxy = factory.CreateChannel();
					IContextChannel cc = proxy as IContextChannel;
				}

				return proxy;
			}

			set
			{
				if (proxy == null)
				{
					proxy = value;
				}
			}
		}

		/// <summary>
		/// Dispose method
		/// </summary>
		public void Dispose()
		{
			try
			{
				if (factory != null)
				{
					factory = null;
				}
			}
			catch (CommunicationException ce)
			{
				Console.WriteLine("Communication exception: {0}", ce.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine("CE proxy exception: {0}", e.Message);
			}
		}

		public List<Tuple<double, DateTime>> GetHistoryMeasurements(long gid, DateTime startTime, DateTime endTime)
		{
			return proxy.GetHistoryMeasurements(gid, startTime, endTime);
		}

        public List<Tuple<double,DateTime>> GetTotalProduction(DateTime startTime, DateTime endTime)
        {
            return proxy.GetTotalProduction(startTime, endTime);
        }
	}
}
