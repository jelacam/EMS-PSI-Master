//-----------------------------------------------------------------------
// <copyright file="EnumDescs.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// EnumDescs class
    /// </summary>
    public class EnumDescs
    {
        /// <summary>
        /// Stores property2enumType
        /// </summary>
        private Dictionary<ModelCode, Type> property2enumType = new Dictionary<ModelCode, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDescs"/> class.
        /// </summary>
        public EnumDescs()
        {
            this.property2enumType.Add(ModelCode.MEASUREMENT_UNITSYMBOL, typeof(UnitSymbol));
            this.property2enumType.Add(ModelCode.ANALOG_SIGNALDIRECTION, typeof(SignalDirection));
            this.property2enumType.Add(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE, typeof(EmsFuelType));
            this.property2enumType.Add(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE, typeof(SynchronousMachineOperatingMode));
        }

        public List<string> GetEnumList(ModelCode propertyId)
        {
            List<string> enumList = new List<string>();

            if (this.property2enumType.ContainsKey(propertyId))
            {
                Type type = this.property2enumType[propertyId];

                for (int i = 0; i < Enum.GetValues(type).Length; i++)
                {
                    enumList.Add(Enum.GetValues(type).GetValue(i).ToString());
                }
            }
            else
            {
                throw new Exception(string.Format("Failed to get enum list. Property ({0}) is not of enum type.", propertyId));
            }

            return enumList;
        }

        public List<string> GetEnumList(Type enumType)
        {
            List<string> enumList = new List<string>();

            try
            {
                for (int i = 0; i < Enum.GetValues(enumType).Length; i++)
                {
                    enumList.Add(Enum.GetValues(enumType).GetValue(i).ToString());
                }

                return enumList;
            }
            catch
            {
                throw new Exception(string.Format("Failed to get enum list. Type ({0}) is not of enum type.", enumType));
            }
        }

        public Type GetEnumTypeForPropertyId(ModelCode propertyId)
        {
            if (this.property2enumType.ContainsKey(propertyId))
            {
                return this.property2enumType[propertyId];
            }
            else
            {
                throw new Exception(string.Format("Property ({0}) is not of enum type.", propertyId));
            }
        }

        public Type GetEnumTypeForPropertyId(ModelCode propertyId, bool throwsException)
        {
            if (this.property2enumType.ContainsKey(propertyId))
            {
                return this.property2enumType[propertyId];
            }
            else if (throwsException)
            {
                throw new Exception(string.Format("Property ({0}) is not of enum type.", propertyId));
            }
            else
            {
                return null;
            }
        }

        public short GetEnumValueFromString(ModelCode propertyId, string value)
        {
            Type type = this.GetEnumTypeForPropertyId(propertyId);

            if (Enum.GetUnderlyingType(type) == typeof(short))
            {
                return (short)Enum.Parse(type, value);
            }
            else if (Enum.GetUnderlyingType(type) == typeof(uint))
            {
                return (short)((uint)Enum.Parse(type, value));
            }
            else if (Enum.GetUnderlyingType(type) == typeof(byte))
            {
                return (short)((byte)Enum.Parse(type, value));
            }
            else if (Enum.GetUnderlyingType(type) == typeof(sbyte))
            {
                return (short)((sbyte)Enum.Parse(type, value));
            }
            else
            {
                throw new Exception(string.Format("Failed to get enum value from string ({0}). Invalid underlying type.", value));
            }
        }

        public string GetStringFromEnum(ModelCode propertyId, short enumValue)
        {
            if (this.property2enumType.ContainsKey(propertyId))
            {
                string retVal = Enum.GetName(this.GetEnumTypeForPropertyId(propertyId), enumValue);
                if (retVal != null)
                {
                    return retVal;
                }
                else
                {
                    return enumValue.ToString();
                }
            }
            else
            {
                return enumValue.ToString();
            }
        }
    }
}