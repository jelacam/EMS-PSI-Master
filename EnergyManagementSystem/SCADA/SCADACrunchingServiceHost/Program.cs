//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACrunchingServiceHost
{
	using System;
	using EMS.Common;
	using EMS.Services.SCADACrunchingService;

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
			ConsoleOptions.SetWindowOptions(ConsoleColor.DarkYellow, 0, 2);
			Console.Title = "SCADA Crunching Service";

			try
			{
                string message = "Starting SCADA Crunching Service ...";
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("\n{0}\n", message);

                using (SCADACrunchingService scadaCR = new SCADACrunchingService())
                {
                    scadaCR.Start();

                    message = "Press <Enter> to stop the service.";
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                    Console.WriteLine(message);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("SCADA Crunching Service failed.");
                Console.WriteLine(ex.StackTrace);
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, "SCADA Crunching Service failed.");
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
