//-----------------------------------------------------------------------
// <copyright file="CalculationEngineProxy.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
	using System;
	using System.ServiceModel;

	public class CalculationEngineProxy : ICalculationEngineContract, IDisposable
	{
		private static ICalculationEngineContract proxy;
		private static ChannelFactory<ICalculationEngineContract> factory;		

		public static ICalculationEngineContract Instance
		{
			get
			{
				if (proxy == null)
				{
					factory = new ChannelFactory<ICalculationEngineContract>("*");
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
				//Console.WriteLine("GDA proxy exception: {0}", e.Message);
			}
		}
	}
}