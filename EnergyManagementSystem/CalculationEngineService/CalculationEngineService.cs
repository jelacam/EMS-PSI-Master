//-----------------------------------------------------------------------
// <copyright file="CalculationEngineService.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
	using System;
	using System.Collections.Generic;
	using System.ServiceModel;
	using EMS.Common;

	/// <summary>
	/// Class for CalculationEngineService
	/// </summary>
	public class CalculationEngineService : IDisposable
	{
		private CalculationEngine ce = null;
		private List<ServiceHost> hosts = null;

		public CalculationEngineService()
		{
			ce = new CalculationEngine();
			//GenericDataAccess.NetworkModel = nm;
			//ResourceIterator.NetworkModel = nm;
			InitializeHosts();
		}

		public void Start()
		{
			StartHosts();
		}

		public void Dispose()
		{
			CloseHosts();
			GC.SuppressFinalize(this);
		}

		private void InitializeHosts()
		{
			hosts = new List<ServiceHost>();
			//hosts.Add(new ServiceHost(typeof(GenericDataAccess)));
		}

		private void StartHosts()
		{
			if (hosts == null || hosts.Count == 0)
			{
				throw new Exception("Calculation Engine Services can not be opend because it is not initialized.");
			}

			string message = string.Empty;
			foreach (ServiceHost host in hosts)
			{
				host.Open();

				message = string.Format("The WCF service {0} is ready.", host.Description.Name);
				Console.WriteLine(message);
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

				message = "Endpoints:";
				Console.WriteLine(message);
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

				foreach (Uri uri in host.BaseAddresses)
				{
					Console.WriteLine(uri);
					CommonTrace.WriteTrace(CommonTrace.TraceInfo, uri.ToString());
				}

				Console.WriteLine("\n");
			}

			message = string.Format("Connection string: {0}", Config.Instance.ConnectionString);
			Console.WriteLine(message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			message = string.Format("Trace level: {0}", CommonTrace.TraceLevel);
			Console.WriteLine(message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);


			message = "The Calculation Engine Service is started.";
			Console.WriteLine("\n{0}", message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
		}

		public void CloseHosts()
		{
			if (hosts == null || hosts.Count == 0)
			{
				throw new Exception("Calculation Engine Services can not be closed because it is not initialized.");
			}

			foreach (ServiceHost host in hosts)
			{
				host.Close();
			}

			string message = "The Calculation Engine Service is closed.";
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
			Console.WriteLine("\n\n{0}", message);
		}
	}
}