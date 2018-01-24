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
        RAW_MIN = 0,
        RAW_MAX = 1,
        EGU_MIN = 2,
        EGU_MAX = 3,
        OPTIMIZED_MIN = 4,
        OPTIMIZED_MAX = 5
    }

    public enum SeverityLevel
    {
        CRITICAL = 0,       // magenta
        HIGH = 1,           // red
        MAJOR = 2,          // orange
        MEDIUM = 3,         // yellow
        LOW = 4,            // green
        MINOR = 5           // cyan
    }
}