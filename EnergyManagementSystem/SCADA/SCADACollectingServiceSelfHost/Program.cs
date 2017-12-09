using EMS.Common;
using EMS.Services.SCADACollectingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACollectingServiceSelfHost
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				string message = "Starting SCADA Collecting Service ...";
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
				Console.WriteLine("\n{0}\n", message);

				using (SCADACollectingService scadaCOL = new SCADACollectingService())
				{
					scadaCOL.Start();

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
