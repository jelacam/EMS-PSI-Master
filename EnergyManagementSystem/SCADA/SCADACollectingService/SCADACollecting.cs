//-----------------------------------------------------------------------
// <copyright file="SCADACollecting.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.Services.SCADACollectingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EMS.ServiceContracts;

    /// <summary>
    /// SCADACollecting component logic
    /// </summary>
    public class SCADACollecting : IScadaCLContract
    {
        /// <summary>
        /// Method for getting data values from simulator
        /// </summary>
        /// <returns> true if values are successfully returned </returns>
        public bool GetDataFromSimulator()
        {
            throw new NotImplementedException();
        }
    }
}
