using EMS.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UIClient.Converters
{
	public class ModelCodeToStringConverter : IValueConverter
	{
		public static Dictionary<ModelCode, string> modelCodeStringDictionary = new Dictionary<ModelCode, string>()
		{
			{ModelCode.ANALOG,"Analog" },
			{ModelCode.ANALOG_MAXVALUE,"Analog_MaxValue" },
			{ModelCode.ANALOG_MINVALUE,"Analog_MinValue" },
			{ModelCode.ANALOG_NORMALVALUE,"Analog_NormalValue" },
			{ModelCode.ANALOG_SIGNALDIRECTION,"Analog_SignalDirection" },
			{ModelCode.CONDUCTINGEQUIPMENT,"ConductionEquipment" },
			{ModelCode.ENERGYCONSUMER,"EnergyConsumer" },
			{ModelCode.ENERGYCONSUMER_PFIXED,"EnergyConsumer_PFixed" },
			{ModelCode.ENERGYCONSUMER_PFIXEDPCT,"EnergyConsumer_PFixedPct" },
			{ModelCode.ENERGYCONSUMER_QFIXED,"EnergyConsumer_QFixed" },
			{ModelCode.ENERGYCONSUMER_QFIXEDPCT,"EnergyConsumer_QFixedPct" },
			{ModelCode.EQUIPMENT,"Equipment" },
			{ModelCode.IDENTIFIEDOBJECT,"IDObject" },
			{ModelCode.IDENTIFIEDOBJECT_GID,"GlobalId" },
			{ModelCode.IDENTIFIEDOBJECT_MRID,"MRid" },
			{ModelCode.IDENTIFIEDOBJECT_NAME,"Name" },
			{ModelCode.MEASUREMENT,"Measurement" },
			{ModelCode.MEASUREMENT_MEASUREMENTTYPE,"MeasurementType" },
			{ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE,"MeasType_PowerSystemResource" },
			{ModelCode.MEASUREMENT_UNITSYMBOL,"UnitSymbol" },
			{ModelCode.POWERSYSTEMRESOURCE,"PowerSystemResource" },
			{ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS,"PowerSystemResource_Measurements" },
			{ModelCode.REGULATINGCONDEQ,"RegulatinCondEq" },
			{ModelCode.ROTATINGMACHINE,"RotatingMachine" },
			{ModelCode.ROTATINGMACHINE_RATEDS,"RotatingMachine_Rateds" },
			{ModelCode.SYNCHRONOUSMACHINE,"SyncMachine" },
			{ModelCode.SYNCHRONOUSMACHINE_MAXQ,"SyncMachine_MaxQ" },
			{ModelCode.SYNCHRONOUSMACHINE_MINQ,"SyncMachine_MinQ" },
			{ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE,"SyncMachine_OperatingMode" },
			{ModelCode.EMSFUEL,"EMSFuel" },
			{ ModelCode.EMSFUEL_FUELTYPE,"EMSFuel_FuelType" },
			{ModelCode.EMSFUEL_SYNCHRONOUSMACHINES,"EMSFuel_SynchronousMachines" },
			{ModelCode.EMSFUEL_UNITPRICE,"EMSFuel_UnitPrice" }
		};

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null && (value is ModelCode))
			{
				return "";
			}

			var modelCode = (ModelCode)value;

			if (modelCodeStringDictionary.ContainsKey(modelCode))
			{
				return modelCodeStringDictionary[modelCode];
			}

			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
