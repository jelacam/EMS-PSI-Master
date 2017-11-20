//-----------------------------------------------------------------------
// <copyright file="SynchronousMachine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// SynchronousMachine class
    /// </summary>
    public class SynchronousMachine : RotatingMachine
    {
        public SynchronousMachine(long globalId) : base(globalId)
        {

        }
    }
}
