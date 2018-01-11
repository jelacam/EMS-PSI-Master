//-----------------------------------------------------------------------
// <copyright file="SCADACommandingService.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.Services.SCADACommandingService
{
    using EMS.Common;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    /// <summary>
    /// SCADACommandingService represents SCADA Commanding component
    /// </summary>
    public class SCADACommandingService : IDisposable
    {
        /// <summary>
        /// Instance of SCADA Commanding logic
        /// </summary>
        private SCADACommanding scadaCMD = null;

        /// <summary>
        /// ServiceHost list
        /// </summary>
        private List<ServiceHost> hosts = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SCADACommandingService"/> class
        /// Creates new SCADACommanding instance and initialize hosts
        /// </summary>
        public SCADACommandingService()
        {
            scadaCMD = new SCADACommanding();
            InitializeHosts();
        }

        /// <summary>
        /// Starting hosts
        /// </summary>
        public void Start()
        {
            StartHosts();
        }

        public bool IntegrityUpdate()
        {
            return scadaCMD.InitiateIntegrityUpdate();
        }

        /// <summary>
        /// Initialize service hosts
        /// </summary>
        private void InitializeHosts()
        {
            hosts = new List<ServiceHost>();
            hosts.Add(new ServiceHost(typeof(SCADACommanding)));
        }

        /// <summary>
        /// Starting hosts
        /// </summary>
        private void StartHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("SCADA Commanding Services can not be opend because it is not initialized.");
            }

            string message = string.Empty;
            foreach (ServiceHost host in hosts)
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

            message = "The SCADA Commanding Service is started.";
            Console.WriteLine("\n{0}", message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            CloseHosts();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Closing hosts
        /// </summary>
        public void CloseHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("SCADA Commanding Services can not be closed because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }

            string message = "The SCADA Commanding Service is closed.";
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            Console.WriteLine("\n\n{0}", message);
        }

        /// <summary>
        /// Test write data in simulator
        /// </summary>
        public void TestWrite()
        {
            this.scadaCMD.TestWrite();
        }
    }
}