//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.AlarmsEventsService
{
	using System;
	using EMS.Common;

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
			ConsoleOptions.SetWindowOptions(ConsoleColor.Blue, 2, 0);
			Console.Title = "Alarms Events Service";

			try
			{
				string message = "Starting Alarms Events Service...";
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
				Console.WriteLine("\n{0}\n", message);

				using (AlarmsEventsService aes = new AlarmsEventsService())
				{
					aes.Start();

					message = "Press <Enter> to stop the service.";
					CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
					Console.WriteLine(message);
					Console.ReadLine();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("AlarmsEventsService failed.");
				Console.WriteLine(ex.StackTrace);
				CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, "AlarmsEventsService failed.");
				CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
				Console.ReadLine();
			}
		}
	}
}
