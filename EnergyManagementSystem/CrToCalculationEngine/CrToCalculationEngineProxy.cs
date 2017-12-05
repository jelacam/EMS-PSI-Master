//-----------------------------------------------------------------------
// <copyright file="CrToCalculationEngineProxy.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
	using System;
	using System.ServiceModel;

	public class CrToCalculationEngineProxy : ICrToCalculationEngineContract, IDisposable
	{
		private static ICrToCalculationEngineContract proxy;
		private static ChannelFactory<ICrToCalculationEngineContract> factory;		

		public static ICrToCalculationEngineContract Instance
		{
			get
			{
				if (proxy == null)
				{
					factory = new ChannelFactory<ICrToCalculationEngineContract>("*");
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