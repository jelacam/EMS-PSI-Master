//-----------------------------------------------------------------------
// <copyright file="ModelDefines.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Common
{
    using System;

    /// <summary>
    /// Enumeration for EMS type
    /// </summary>
    public enum EMSType : short
    {
        /// <summary>
        /// mask type
        /// </summary>
        MASK_TYPE = unchecked((short)0xFFFF),

        /// <summary>
        /// Analog EMS type
        /// </summary>
        ANALOG = 0x0001,

        /// <summary>
        /// EnergyConsumer EMS type
        /// </summary>
        ENERGYCONSUMER = 0x0002,

        /// <summary>
        /// SynchronousMachine EMS type
        /// </summary>
        SYNCHRONOUSMACHINE = 0x0003,

        /// <summary>
        /// EMSFuel EMS type
        /// </summary>
        EMSFUEL = 0x0004,
    }

    /// <summary>
    /// Enumeration for model code
    /// </summary>
    [Flags]
    public enum ModelCode : long
    {
        /// <summary>
        /// ModelCode for IDENTIFIEDOBJECT
        /// </summary>
        IDENTIFIEDOBJECT = 0x1000000000000000,

        /// <summary>
        /// ModelCode for IDENTIFIEDOBJECT_GID
        /// </summary>
        IDENTIFIEDOBJECT_GID = 0x1000000000000104,

        /// <summary>
        /// ModelCode for IDENTIFIEDOBJECT_MRID
        /// </summary>
        IDENTIFIEDOBJECT_MRID = 0x1000000000000207,

        /// <summary>
        /// ModelCode for IDENTIFIEDOBJECT_NAME
        /// </summary>
        IDENTIFIEDOBJECT_NAME = 0x1000000000000307,

        /// <summary>
        /// ModelCode for MEASUREMENT
        /// </summary>
        MEASUREMENT = 0x1100000000000000,

        /// <summary>
        /// ModelCode for MEASUREMENT_MEASUREMENTTYPE
        /// </summary>
        MEASUREMENT_MEASUREMENTTYPE = 0x1100000000000107,

        /// <summary>
        /// ModelCode for MEASUREMENT_UNITSYMBOL
        /// </summary>
        MEASUREMENT_UNITSYMBOL = 0x110000000000020a,

        /// <summary>
        /// ModelCode for MEASUREMENT_POWERSYSTEMRESOURCE
        /// </summary>
        MEASUREMENT_POWERSYSTEMRESOURCE = 0x1100000000000309,

        /// <summary>
        /// ModelCode for POWERSYSTEMRESOURCE
        /// </summary>
        POWERSYSTEMRESOURCE = 0x1200000000000000,

        /// <summary>
        /// ModelCode for POWERSYSTEMRESOURCE_MEASUREMENTS
        /// </summary>
        POWERSYSTEMRESOURCE_MEASUREMENTS = 0x1200000000000119,

		/// <summary>
		/// ModelCode for EMSFUEL
		/// </summary>
		EMSFUEL = 0x1300000000040000,

		/// <summary>
		/// ModelCode for EMSFUEL_FUELTYPE
		/// </summary>
		EMSFUEL_FUELTYPE = 0x130000000004010a,

		/// <summary>
		/// ModelCode for EMSFUEL_UNITPRICE
		/// </summary>
		EMSFUEL_UNITPRICE = 0x1300000000040205,

		/// <summary>
		/// ModelCode for EMSFUEL_SYNCHRONOUSMACHINES
		/// </summary>
		EMSFUEL_SYNCHRONOUSMACHINES = 0x1300000000040319,

		/// <summary>
		/// ModelCode for ANALOG
		/// </summary>
		ANALOG = 0x1110000000010000,

        /// <summary>
        /// ModelCode for ANALOG_MAXVALUE
        /// </summary>
        ANALOG_MAXVALUE = 0x1110000000010105,

        /// <summary>
        /// ModelCode for ANALOG_MINVALUE
        /// </summary>
        ANALOG_MINVALUE = 0x1110000000010205,

        /// <summary>
        /// ModelCode for ANALOG_NORMALVALUE
        /// </summary>
        ANALOG_NORMALVALUE = 0x1110000000010305,

        /// <summary>
        /// ModelCode for ANALOG_SIGNALDIRECTION
        /// </summary>
        ANALOG_SIGNALDIRECTION = 0x111000000001040a,

        /// <summary>
        /// ModelCode for EQUIPMENT
        /// </summary>
        EQUIPMENT = 0x1210000000000000,

        /// <summary>
        /// ModelCode for CONDUCTINGEQUIPMENT
        /// </summary>
        CONDUCTINGEQUIPMENT = 0x1211000000000000,

        /// <summary>
        /// ModelCode for ENERGYCONSUMER
        /// </summary>
        ENERGYCONSUMER = 0x1211100000020000,

        /// <summary>
        /// ModelCode for ENERGYCONSUMER_PFIXED
        /// </summary>
        ENERGYCONSUMER_PFIXED = 0x1211100000020105,

        /// <summary>
        /// ModelCode for ENERGYCONSUMER_PFIXEDPCT
        /// </summary>
        ENERGYCONSUMER_PFIXEDPCT = 0x1211100000020205,

        /// <summary>
        /// ModelCode for ENERGYCONSUMER_QFIXED
        /// </summary>
        ENERGYCONSUMER_QFIXED = 0x1211100000020305,

        /// <summary>
        /// ModelCode for ENERGYCONSUMER_QFIXEDPCT
        /// </summary>
        ENERGYCONSUMER_QFIXEDPCT = 0x1211100000020405,

        /// <summary>
        /// ModelCode for REGULATINGCONDEQ
        /// </summary>
        REGULATINGCONDEQ = 0x1211200000000000,

        /// <summary>
        /// ModelCode for ROTATINGMACHINE
        /// </summary>
        ROTATINGMACHINE = 0x1211210000000000,

        /// <summary>
        /// ModelCode for ROTATINGMACHINE_RATEDS
        /// </summary>
        ROTATINGMACHINE_RATEDS = 0x1211210000000105,

        /// <summary>
        /// ModelCode for SYNCHRONOUSMACHINE
        /// </summary>
        SYNCHRONOUSMACHINE = 0x1211211000030000,

        /// <summary>
        /// ModelCode for SYNCHRONOUSMACHINE_MAXQ
        /// </summary>
        SYNCHRONOUSMACHINE_MAXQ = 0x1211211000030105,

        /// <summary>
        /// ModelCode for SYNCHRONOUSMACHINE_MINQ
        /// </summary>
        SYNCHRONOUSMACHINE_MINQ = 0x1211211000030205,

        /// <summary>
        /// ModelCode for SYNCHRONOUSMACHINE_OPERATINGMODE
        /// </summary>
        SYNCHRONOUSMACHINE_OPERATINGMODE = 0x121121100003030a,

        /// <summary>
        /// ModelCode for SYNCHRONOUSMACHINE_FUEL
        /// </summary>
        SYNCHRONOUSMACHINE_FUEL = 0x1211211000030409,

		/// <summary>
		/// ModelCode for SYNCHRONOUSMACHINE_ACTIVE
		/// </summary>
		SYNCHRONOUSMACHINE_ACTIVE = 0x1211211000030501,

		/// <summary>
		/// ModelCode for SYNCHRONOUSMACHINE_LOADPCT
		/// </summary>
		SYNCHRONOUSMACHINE_LOADPCT = 0x1211211000030605,

		/// <summary>
		/// ModelCode for SYNCHRONOUSMACHINE_MAXCOSPHI
		/// </summary>
		SYNCHRONOUSMACHINE_MAXCOSPHI = 0x1211211000030705,

		/// <summary>
		/// ModelCode for SYNCHRONOUSMACHINE_MINCOSPHI
		/// </summary>
		SYNCHRONOUSMACHINE_MINCOSPHI = 0x1211211000030805
	}

    /// <summary>
    /// Enumeration for model code mask
    /// </summary>
    [Flags]
    public enum ModelCodeMask : long
    {
        MASK_GID_TYPE = 0x0000ffff00000000,

        MASK_TYPE = 0x00000000ffff0000,
        MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
        MASK_ATTRIBUTE_TYPE = 0x00000000000000ff,

        MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
        MASK_FIRSTNBL = unchecked((long)0xf000000000000000),
        MASK_DELFROMNBL8 = unchecked((long)0xfffffff000000000),
    }
}