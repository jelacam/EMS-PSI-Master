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
        NONE = 0,
        RAW_MIN = 1,
        RAW_MAX = 2,
        EGU_MIN = 3,
        EGU_MAX = 4,
        OPTIMIZED_MIN = 5,
        OPTIMIZED_MAX = 6,
    }

    public enum SeverityLevel
    {
        NONE = 0,
        MINOR = 1,       // cyan
        LOW = 2,           // green
        MEDIUM = 3,          // yellow
        MAJOR = 4,         // orange
        HIGH = 5,            // red
        CRITICAL = 6           // magenta
    }
}