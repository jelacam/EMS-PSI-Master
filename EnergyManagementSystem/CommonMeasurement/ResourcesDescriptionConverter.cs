//-----------------------------------------------------------------------
// <copyright file="ResourcesDescriptionConverter.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.CommonMeasurement
{
	using System;
	using EMS.Common;
	using EMS.Services.NetworkModelService.DataModel.Meas;
	using EMS.Services.NetworkModelService.DataModel.Production;
	using EMS.Services.NetworkModelService.DataModel.Wires;

	/// <summary>
	/// Class for converting resource description
	/// </summary>
	public static class ResourcesDescriptionConverter
	{
		/// <summary>
		/// Converts ResourceDescription to Analog
		/// </summary>
		/// <param name="rd">ResourceDescription entity</param>
		/// <returns>instance of Analog</returns>
		public static Analog ConvertToAnalog(ResourceDescription rd)
		{
			Analog analog = null;
			analog = new Analog(rd.Id);
			Property property = null;

			property = rd.GetProperty(ModelCode.ANALOG_MAXVALUE);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.ANALOG_MINVALUE);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.ANALOG_NORMALVALUE);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.ANALOG_SIGNALDIRECTION);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.MEASUREMENT_UNITSYMBOL);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			//property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_GID);
			//if (property != null)
			//{
			//	analog.SetProperty(property);
			//	property = null;
			//}

			property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
			if (property != null)
			{
				analog.SetProperty(property);
				property = null;
			}

			return analog;
		}

		/// <summary>
		/// Converts ResourceDescription to SynchronousMachine
		/// </summary>
		/// <param name="rd">ResourceDescription entity</param>
		/// <returns>instance of SynchronousMachine</returns>
		public static SynchronousMachine ConvertToSynchronousMachine(ResourceDescription rd)
		{
			SynchronousMachine sm = null;
			sm = new SynchronousMachine(rd.Id);
			Property property = null;

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_ACTIVE);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_FUEL);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_LOADPCT);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MAXCOSPHI);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MAXQ);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MINCOSPHI);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MINQ);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.ROTATINGMACHINE_RATEDS);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			//property = rd.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
			//if (property != null)
			//{
			//	sm.SetProperty(property);
			//	property = null;
			//}

			property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
			if (property != null)
			{
				sm.SetProperty(property);
				property = null;
			}

			return sm;
		}

		/// <summary>
		/// Converts ResourceDescription to EMSFuel
		/// </summary>
		/// <param name="rd">ResourceDescription entity</param>
		/// <returns>instance of EMSFuel</returns>
		public static EMSFuel ConvertToEMSFuel(ResourceDescription rd)
		{
			EMSFuel emsf = null;
			emsf = new EMSFuel(rd.Id);
			Property property = null;

			property = rd.GetProperty(ModelCode.EMSFUEL_FUELTYPE);
			if (property != null)
			{
				emsf.SetProperty(property);
				property = null;
			}

			//property = rd.GetProperty(ModelCode.EMSFUEL_SYNCHRONOUSMACHINES);
			//if (property != null)
			//{
			//	emsf.SetProperty(property);
			//	property = null;
			//}

			property = rd.GetProperty(ModelCode.EMSFUEL_UNITPRICE);
			if (property != null)
			{
				emsf.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
			if (property != null)
			{
				emsf.SetProperty(property);
				property = null;
			}

			property = rd.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
			if (property != null)
			{
				emsf.SetProperty(property);
				property = null;
			}

			return emsf;
		}
	}
}