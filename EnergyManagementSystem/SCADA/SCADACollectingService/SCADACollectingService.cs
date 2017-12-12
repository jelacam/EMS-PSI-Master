//-----------------------------------------------------------------------
// <copyright file="SCADACollectingService.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACollectingService
{
	using System;
	using System.Collections.Generic;
	using System.ServiceModel;
	using Common;

	/// <summary>
	/// SCADACollectingService represents SCADA Collecting component
	/// </summary>
	public class SCADACollectingService : IDisposable
	{
		/// <summary>
		/// Instance of SCADA Collecting logic
		/// </summary>
		private SCADACollecting scadaCL = null;

		/// <summary>
		/// ServiceHost list
		/// </summary>
		private List<ServiceHost> hosts = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="SCADACollectingService"/> class
		/// Creates new SCADACollecting instance and initialize hosts
		/// </summary>
		public SCADACollectingService()
		{
			this.scadaCL = new SCADACollecting();
			this.InitializeHosts();
		}

		/// <summary>
		/// Starting hosts
		/// </summary>
		public void Start()
		{
			this.StartHosts();
			this.scadaCL.GetDataFromSimulator();
		}

		/// <summary>
		/// Dispose method
		/// </summary>
		public void Dispose()
		{
			this.CloseHosts();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Initialize service hosts
		/// </summary>
		private void InitializeHosts()
		{
			this.hosts = new List<ServiceHost>();
			this.hosts.Add(new ServiceHost(typeof(SCADACollecting)));
		}

		/// <summary>
		/// Starting hosts
		/// </summary>
		private void StartHosts()
		{
			if (this.hosts == null || this.hosts.Count == 0)
			{
				throw new Exception("SCADA Collecting Services can not be opened because it is not initialized.");
			}

			string message = string.Empty;
			foreach (ServiceHost host in this.hosts)
			{
				try
				{
					host.Open();

					message = string.Format("The WCF service {0} is ready.", host.Description.Name);
					Console.WriteLine(message);
					CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

					foreach (Uri uri in host.BaseAddresses)
					{
						Console.WriteLine(uri);
						CommonTrace.WriteTrace(CommonTrace.TraceInfo, uri.ToString());
					}

					Console.WriteLine("\n");
				}
				catch (CommunicationException ce)
				{
					Console.WriteLine(ce.Message);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}

			message = string.Format("Trace level: {0}", CommonTrace.TraceLevel);
			Console.WriteLine(message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			message = "The SCADA Collecting Service is started.";
			Console.WriteLine("\n{0}", message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
		}

		/// <summary>
		/// Closing hosts
		/// </summary>
		private void CloseHosts()
		{
			if (this.hosts == null || this.hosts.Count == 0)
			{
				throw new Exception("SCADA Collecting Services can not be closed because it is not initialized.");
			}

			foreach (ServiceHost host in this.hosts)
			{
				host.Close();
			}

			string message = "The SCADA Collecting Service is closed.";
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
			Console.WriteLine("\n\n{0}", message);
		}
	}
}
