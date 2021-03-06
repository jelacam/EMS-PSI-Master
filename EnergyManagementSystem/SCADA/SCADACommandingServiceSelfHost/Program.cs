﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SCADACommandingServiceSelfHost
{
    using EMS.Common;
    using EMS.Services.SCADACommandingService;
    using System;

    /// <summary>
    /// Class for Main method
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            ConsoleOptions.SetWindowOptions(ConsoleColor.DarkYellow, 0, 1);
            Console.Title = "SCADA Commanding Service";

            try
            {
                string message = "Starting SCADA Commanding Service ...";
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                Console.WriteLine("\n{0}\n", message);

                using (SCADACommandingService scadaCMD = new SCADACommandingService())
                {
                    scadaCMD.Start();

                    try
                    {
                        bool integrityResult = scadaCMD.IntegrityUpdate();
                        if (integrityResult)
                        {
                            message = "Integrity Update finished successfully.";
                            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                            Console.WriteLine(message);
                        }
                    }
                    catch (Exception e)
                    {
                        message = "Integrity Update failed. " + e.Message;
                        CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                        Console.WriteLine(message);
                    }

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