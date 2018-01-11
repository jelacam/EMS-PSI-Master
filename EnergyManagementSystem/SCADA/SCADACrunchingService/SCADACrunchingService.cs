//-----------------------------------------------------------------------
// <copyright file="SCADACrunchingService.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACrunchingService
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using EMS.Common;

    /// <summary>
    /// SCADACrunchingService represents SCADA Crunching component
    /// </summary>
    public class SCADACrunchingService : IDisposable
    {
        /// <summary>
        /// Instance of SCADA Crunching logic
        /// </summary>
        private SCADACrunching scadaCR = null;

        /// <summary>
        /// ServiceHost list
        /// </summary>
        private List<ServiceHost> hosts = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SCADACrunchingService"/> class
        /// Creates new SCADACrunching instance and initialize hosts
        /// </summary>
        public SCADACrunchingService()
        {
            this.scadaCR = new SCADACrunching();
            this.InitializeHosts();
        }

        /// <summary>
        /// Starting hosts
        /// </summary>
        public void Start()
        {
            this.StartHosts();
        }

        /// <summary>
        /// Closing hosts
        /// </summary>
        public void CloseHosts()
        {
            if (this.hosts == null || this.hosts.Count == 0)
            {
                throw new Exception("SCADA Crunching Services can not be closed because it is not initialized.");
            }

            foreach (ServiceHost host in this.hosts)
            {
                host.Close();
            }

            string message = "The SCADA Crunching Service is closed.";
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            Console.WriteLine("\n\n{0}", message);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            this.CloseHosts();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///  Integrity Update 
        /// </summary>
        /// <returns></returns>
        public bool IntegrityUpdate()
        {
            return scadaCR.InitiateIntegrityUpdate();
        }

        /// <summary>
        /// Initialize service hosts
        /// </summary>
        private void InitializeHosts()
        {
            this.hosts = new List<ServiceHost>();
            this.hosts.Add(new ServiceHost(typeof(SCADACrunching)));
        }

        /// <summary>
        /// Starting hosts
        /// </summary>
        private void StartHosts()
        {
            if (this.hosts == null || this.hosts.Count == 0)
            {
                throw new Exception("SCADA Crunching Services can not be opend because it is not initialized.");
            }

            string message = string.Empty;
            foreach (ServiceHost host in this.hosts)
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

            message = "The SCADA Crunching Service is started.";
            Console.WriteLine("\n{0}", message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
        }
    }
}
