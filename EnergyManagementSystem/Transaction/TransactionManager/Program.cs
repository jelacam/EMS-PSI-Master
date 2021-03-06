﻿using EMS.Common;
using EMS.Services.NetworkModelService.DataModel.Meas;
using EMS.Services.TransactionManagerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleOptions.SetWindowOptions(ConsoleColor.DarkMagenta, 1, 0);
            Console.Title = "Transaction Manager Service";

            try
            {
                string message = "Starting Transaction Manager Service ...";
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("\n{0}\n", message);

                using (TransactionManagerService tmService = new TransactionManagerService())
                {
                    tmService.Start();

                    Console.WriteLine("Analog test");

                    //tmService.ScadaCRPrepare(new Delta());
                    //tmService.ScadaCMDPrepare(new Delta());
                    //tmService.NMSPrepare(new Delta());

                    message = "Press <Enter> to stop the service.";
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                    Console.WriteLine(message);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Transaction Manager Service failed.");
                Console.WriteLine(ex.StackTrace);
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Transaction Manager Service failed.");
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);

                Console.ReadLine();
            }
        }
    }
}