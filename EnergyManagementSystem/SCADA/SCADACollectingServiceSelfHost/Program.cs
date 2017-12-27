//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACollectingServiceSelfHost
{
	using System;
	using EMS.Common;
	using EMS.Services.SCADACollectingService;

	/// <summary>
	/// Class for Main method
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Main method
		/// </summary>
		/// <param name="args">arguments for method</param>
		private static void Main(string[] args)
		{

			ConsoleOptions.SetWindowOptions(ConsoleColor.DarkYellow, 1, 1);
			Console.Title = "SCADA Collecting Service";

			try
			{
				string message = "Starting SCADA Collecting Service ...";
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
				Console.WriteLine("\n{0}\n", message);

				using (SCADACollectingService scadaCL = new SCADACollectingService())
				{
					scadaCL.Start();

					scadaCL.StartCollectingData();

					message = "Press <Enter> to stop the service.";
					CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
					Console.WriteLine(message);
					Console.ReadLine();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("SCADA Collecting Service failed.");
				Console.WriteLine(ex.StackTrace);
				CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, "SCADA Collecting Service failed.");
				CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);

				Console.ReadLine();
			}
		}
	}
}
