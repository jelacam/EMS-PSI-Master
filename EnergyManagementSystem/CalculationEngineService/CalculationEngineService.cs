//-----------------------------------------------------------------------
// <copyright file="CalculationEngineService.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
    using EMS.Common;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    /// <summary>
    /// Class for CalculationEngineService
    /// </summary>
    public class CalculationEngineService : IDisposable
    {
        /// <summary>
        /// CalculationEngine instance
        /// </summary>
        private CalculationEngine ce = null;

        /// <summary>
        /// list of ServiceHost
        /// </summary>
        private List<ServiceHost> hosts = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationEngineService" /> class
        /// </summary>
        public CalculationEngineService()
        {
            this.ce = new CalculationEngine();
            CrToCe.CalculationEngine = this.ce;
            CeToUI.CalculationEngine = this.ce;
            this.InitializeHosts();
        }

        /// <summary>
        /// Start method
        /// </summary>
        public void Start()
        {
            this.StartHosts();
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
        /// CloseHosts method
        /// </summary>
        public void CloseHosts()
        {
            if (this.hosts == null || this.hosts.Count == 0)
            {
                throw new Exception("Calculation Engine Services can not be closed because it is not initialized.");
            }

            foreach (ServiceHost host in this.hosts)
            {
                host.Close();
            }

            string message = "The Calculation Engine Service is closed.";
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            Console.WriteLine("\n\n{0}", message);
        }

        /// <summary>
        /// InitializeHosts method
        /// </summary>
        private void InitializeHosts()
        {
            this.hosts = new List<ServiceHost>();
            this.hosts.Add(new ServiceHost(typeof(CrToCe)));
            //this.hosts.Add(new ServiceHost(typeof(CeToUI)));
            //this.hosts.Add(new ServiceHost(typeof(PublisherService)));
            this.hosts.Add(new ServiceHost(typeof(CalculationEngine)));
        }

        /// <summary>
        /// StartHosts method
        /// </summary>
        private void StartHosts()
        {
            if (this.hosts == null || this.hosts.Count == 0)
            {
                throw new Exception("Calculation Engine Services can not be opend because it is not initialized.");
            }

            string message = string.Empty;
            foreach (ServiceHost host in this.hosts)
            {
                host.Open();
                message = string.Format("The WCF service {0} is ready.", host.Description.Name);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                message = "Endpoints:";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                foreach (Uri uri in host.BaseAddresses)
                {
                    Console.WriteLine(uri);
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, uri.ToString());
                }

                Console.WriteLine("\n");
            }

            message = string.Format("Connection string: {0}", Config.Instance.ConnectionString);
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            message = string.Format("Trace level: {0}", CommonTrace.TraceLevel);
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            message = "The Calculation Engine Service is started.";
            Console.WriteLine("\n{0}", message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
        }

        /// <summary>
        /// Integrity update for Calculation Engine service
        /// </summary>
        /// <returns></returns>
        public bool IntegrityUpdate()
        {
            return ce.InitiateIntegrityUpdate();
        }

        public void Populate()
        {
            ce.PopulateDatabase();
        }
    }
}