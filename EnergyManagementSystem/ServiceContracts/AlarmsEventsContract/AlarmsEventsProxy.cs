//-----------------------------------------------------------------------
// <copyright file="AlarmsEventsProxy.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
	using System;
	using System.ServiceModel;
	using CommonMeasurement;

	/// <summary>
	/// Class for IAlarmsEventsContract and IDisposable implementation
	/// </summary>
	public class AlarmsEventsProxy : IAlarmsEventsContract, IDisposable
	{
		/// <summary>
		/// proxy object
		/// </summary>
		private static IAlarmsEventsContract proxy;

		/// <summary>
		/// ChannelFactory object
		/// </summary>
		private static ChannelFactory<IAlarmsEventsContract> factory;

		/// <summary>
		/// Gets or sets instance of IAlarmsEventsContract
		/// </summary>
		public static IAlarmsEventsContract Instance
		{
			get
			{
				if (proxy == null)
				{
					factory = new ChannelFactory<IAlarmsEventsContract>("*");
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
				Console.WriteLine("AE proxy exception: {0}", e.Message);
			}
		}

		/// <summary>
		/// Test method
		/// </summary>
		public void Test()
		{
			proxy.Test();
		}

		/// <summary>
		/// Adds new alarm
		/// </summary>
		/// <param name="alarm">alarm to add</param>
		public void AddAlarm(AlarmHelper alarm)
		{
			proxy.AddAlarm(alarm);
		}
	}
}