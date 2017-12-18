﻿using EMS.Common;
using EMS.Services.SCADACommandingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADACommandingServiceSelfHost
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                string message = "Starting SCADA Commanding Service ...";
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("\n{0}\n", message);

                using (SCADACommandingService scadaCMD = new SCADACommandingService())
                {
                    scadaCMD.Start();

                    scadaCMD.TestWrite();

                    message = "Press <Enter> to stop the service.";
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                    Console.WriteLine(message);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("SCADA Commanding Service failed.");
                Console.WriteLine(ex.StackTrace);
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, "SCADA Commanding Service failed.");
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);

                Console.ReadLine();
            }
        }


    }
}
