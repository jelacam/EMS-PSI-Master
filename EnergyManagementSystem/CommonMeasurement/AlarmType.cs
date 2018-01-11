//-----------------------------------------------------------------------
// <copyright file="AlarmType.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.CommonMeasurement
{
    /// <summary>
    /// Enumeration for alarm type
    /// </summary>
    public enum AlarmType
    {
        rawMin = 0,
        rawMax = 1,
        eguMin = 2,
        eguMax = 3,
        optimizedMin = 4,
        optimizedMax = 5
    }
}