//-----------------------------------------------------------------------
// <copyright file="Enums.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Common
{
    /// <summary>
    /// Enumeration for signal direction
    /// </summary>
    public enum SignalDirection : short
    {
        /// <summary>
        /// Read signal direction
        /// </summary>
        Read = 0,

        /// <summary>
        /// ReadWrite signal direction
        /// </summary>
        ReadWrite = 1,

        /// <summary>
        /// Write signal direction
        /// </summary>
        Write = 2
    }

    /// <summary>
    /// Enumeration for fuel type
    /// </summary>
    public enum EmsFuelType : short
    {
        /// <summary>
        /// coal fuel type
        /// </summary>
        coal = 0,

        /// <summary>
        /// hydro fuel type
        /// </summary>
        hydro = 1,

        /// <summary>
        /// oil fuel type
        /// </summary>
        oil = 2,

        /// <summary>
        /// solar fuel type
        /// </summary>
        solar = 3,

        /// <summary>
        /// wind fuel type
        /// </summary>
        wind = 4
    }

    /// <summary>
    /// Enumeration for SynchronousMachine operating mode
    /// </summary>
    public enum SynchronousMachineOperatingMode
    {
        /// <summary>
        /// generator operating mode
        /// </summary>
        generator = 0,

        /// <summary>
        /// condenser operating mode
        /// </summary>
        condenser = 1
    }

    /// <summary>
    /// Enumeration for unit symbol
    /// </summary>
    public enum UnitSymbol : short
    {
        /// <summary>
        /// unit symbol VA
        /// </summary>
        VA = 0x00,

        /// <summary>
        /// unit symbol W
        /// </summary>
        W = 0x01,

        /// <summary>
        /// unit symbol VAr
        /// </summary>
        VAr = 0x02,

        /// <summary>
        /// unit symbol VAh
        /// </summary>
        VAh = 0x03,

        /// <summary>
        /// unit symbol Wh
        /// </summary>
        Wh = 0x04,

        /// <summary>
        /// unit symbol VArh
        /// </summary>
        VArh = 0x05,

        /// <summary>
        /// unit symbol V
        /// </summary>
        V = 0x06,

        /// <summary>
        /// unit symbol ohm
        /// </summary>
        ohm = 0x07,

        /// <summary>
        /// unit symbol A
        /// </summary>
        A = 0x08,

        /// <summary>
        /// unit symbol F
        /// </summary>
        F = 0x09,

        /// <summary>
        /// unit symbol H
        /// </summary>
        H = 0x0A,

        /// <summary>
        /// unit symbol degC
        /// </summary>
        degC = 0x0B,

        /// <summary>
        /// unit symbol s
        /// </summary>
        s = 0x0C,

        /// <summary>
        /// unit symbol min
        /// </summary>
        min = 0x0D,

        /// <summary>
        /// unit symbol h
        /// </summary>
        h = 0x0E,

        /// <summary>
        /// unit symbol deg
        /// </summary>
        deg = 0x0F,

        /// <summary>
        /// unit symbol rad
        /// </summary>
        rad = 0x10,

        /// <summary>
        /// unit symbol J
        /// </summary>
        J = 0x11,

        /// <summary>
        /// unit symbol N
        /// </summary>
        N = 0x12,

        /// <summary>
        /// unit symbol S
        /// </summary>
        S = 0x13,

        /// <summary>
        /// unit symbol none
        /// </summary>
        none = 0x14,

        /// <summary>
        /// unit symbol Hz
        /// </summary>
        Hz = 0x15,

        /// <summary>
        /// unit symbol g
        /// </summary>
        g = 0x16,

        /// <summary>
        /// unit symbol Pa
        /// </summary>
        Pa = 0x17,

        /// <summary>
        /// unit symbol m
        /// </summary>
        m = 0x18,

        /// <summary>
        /// unit symbol m2
        /// </summary>
        m2 = 0x19,

        /// <summary>
        /// unit symbol m3
        /// </summary>
        m3 = 0x1A
    }
}