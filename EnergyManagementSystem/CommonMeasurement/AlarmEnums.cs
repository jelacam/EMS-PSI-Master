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
    public enum AlarmType:int
    {
        NONE = 0,
        RAW_MIN = 1,
        RAW_MAX = 2,
        EGU_MIN = 3,
        EGU_MAX = 4,
        OPTIMIZED_MIN = 5,
        OPTIMIZED_MAX = 6,
        FLATLINE = 7
    }

    public enum SeverityLevel:int
    {
        NONE = 0,
        MINOR = 1,       // cyan
        LOW = 2,           // green
        MEDIUM = 3,          // yellow
        MAJOR = 4,         // orange
        HIGH = 5,            // red
        CRITICAL = 6           // magenta
    }

    public enum State:int
    {
        Active = 0,
        Cleared = 1,
    }

    public enum AckState:int
    {
        Acknowledged = 0,
        Unacknowledged = 1
    }

    public enum PublishingStatus:int
    {
        INSERT = 0,
        UPDATE = 1
    }

    public enum PersistentState:int
    {
        Persistent = 0,
        Nonpersistent = 1,
    }

    public enum InhibitState:int
    {
        Inhibit = 0,
        Noninhibit = 1
    }
}