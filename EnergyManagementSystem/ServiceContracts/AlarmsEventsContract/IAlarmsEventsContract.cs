//-----------------------------------------------------------------------
// <copyright file="IAlarmsEventsContract.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
    using System;
    using System.ServiceModel;
    using CommonMeasurement;

    /// <summary>
    /// Contract for AlarmsEvents
    /// </summary>
    [ServiceContract]
    public interface IAlarmsEventsContract
    {


        /// <summary>
        /// Adds new alarm
        /// </summary>
        /// <param name="alarm">alarm to add</param>
        [OperationContract]
        void AddAlarm(AlarmHelper alarm);
    }
}
