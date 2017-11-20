//-----------------------------------------------------------------------
// <copyright file="Analog.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Meas
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// Analog class
    /// </summary>
    public class Analog : Measurement
    {
        public Analog(long globalId) : base(globalId)
        {

        }
    }
}
