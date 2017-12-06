using SCADACollectingService;
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
				string message = "Starting Network Model Service...";
				Console.WriteLine("\n{0}\n", message);

				using (SCADACollectingService service = new SCADACollectingService("localhost",502))
				{
					service.Start();

					message = "Press <Enter> to stop the service.";
					Console.WriteLine(message);
					Console.ReadLine();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("NetworkModelService failed.");
				Console.WriteLine(ex.StackTrace);
				Console.ReadLine();
			}
		}
	}
}
