namespace EMS.CIMAdapter.Importer
{
    using System;
    using EMS.Common;

/// <summary>
/// EMSConverter
/// </summary>
    public static class EMSConverter
    {

        #region Populate ResourceDescription

/// <summary>
/// Method populates IdentifiedObject properties
/// </summary>
/// <param name="cimIdentifiedObject">IdentifiedObject object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
        public static void PopulateIdentifiedObjectProperties(EMS.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDENTIFIEDOBJECT_NAME, cimIdentifiedObject.Name));
                }
            }
        }

/// <summary>
/// Method populates PowerSystemResource properties
/// </summary>
/// <param name="cimPowerSystemResource">PowerSystemResource object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
        public static void PopulatePowerSystemResourceProperties(EMS.PowerSystemResource cimPowerSystemResource, ResourceDescription rd)
        {
            if ((cimPowerSystemResource != null) && (rd != null))
            {
                EMSConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
            }
        }

/// <summary>
/// Method populates Equipment properties
/// </summary>
/// <param name="cimEquipment">Equipment object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
        public static void PopulateEquipmentProperties(EMS.Equipment cimEquipment, ResourceDescription rd)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                EMSConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd);
            }
        }

/// <summary>
/// Method populates ConductingEquipment properties
/// </summary>
/// <param name="cimConductingEquipment">ConductingEquipment object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
        public static void PopulateConductingEquipmentProperties(EMS.ConductingEquipment cimConductingEquipment, ResourceDescription rd)
        {
            if ((cimConductingEquipment != null) && (rd != null))
            {
                EMSConverter.PopulateEquipmentProperties(cimConductingEquipment, rd);
            }
        }

/// <summary>
/// Method populates EnergyConsumer properties
/// </summary>
/// <param name="cimEnergyConsumer">EnergyConsumer object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
/// <param name="importHelper">Import</param>
/// <param name="report"></param>
        public static void PopulateEnergyConsumerProperties(EMS.EnergyConsumer cimEnergyConsumer, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimEnergyConsumer != null) && (rd != null))
            {
                EMSConverter.PopulateConductingEquipmentProperties(cimEnergyConsumer, rd);

                if (cimEnergyConsumer.PfixedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYCONSUMER_PFIXED, cimEnergyConsumer.Pfixed));
                }

                if (cimEnergyConsumer.PfixedPctHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYCONSUMER_PFIXEDPCT, cimEnergyConsumer.PfixedPct));
                }

                if (cimEnergyConsumer.QfixedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYCONSUMER_QFIXED, cimEnergyConsumer.Qfixed));
                }

                if (cimEnergyConsumer.QfixedPctHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYCONSUMER_QFIXEDPCT, cimEnergyConsumer.QfixedPct));
                }
            }
        }

/// <summary>
/// Method populates RegulatingCondEq properties
/// </summary>
/// <param name="cimRegulatingCondEq">RegulatingCondEq object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
/// <param name="importHelper"></param>
/// <param name="report"></param>
        public static void PopulateRegulatingCondEqProperties(EMS.RegulatingCondEq cimRegulatingCondEq, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegulatingCondEq != null) && (rd != null))
            {
                EMSConverter.PopulateConductingEquipmentProperties(cimRegulatingCondEq, rd);
            }
        }

        /// <summary>
        /// Method populates EMSFuel properties
        /// </summary>
        /// <param name="emsFuel">EMSFuel object with values from CIM</param>
        /// <param name="rd">ResourcesDescription object from importer</param>
        /// <param name="importHelper"></param>
        /// <param name="report"></param>
        public static void PopulateEmsFuelProperties(EMSFuel emsFuel, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((emsFuel != null) && (rd != null))
            {
                EMSConverter.PopulateIdentifiedObjectProperties(emsFuel, rd);

                if (emsFuel.FuelTypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EMSFUEL_FUELTYPE, (short)emsFuel.FuelType));
                }

                if (emsFuel.UnitPriceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EMSFUEL_UNITPRICE, emsFuel.UnitPrice));
                }

            }
        }

        /// <summary>
        /// Method populates RotatingMachine properties
        /// </summary>
        /// <param name="cimRotatingMachine">RotatingMachine object with values from CIM</param>
        /// <param name="rd">ResourceDescription object from importer</param>
        /// <param name="importHelper"></param>
        /// <param name="report"></param>
        public static void PopulateRotatingMachineProperties(EMS.RotatingMachine cimRotatingMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRotatingMachine != null) && (rd != null))
            {
                EMSConverter.PopulateRegulatingCondEqProperties(cimRotatingMachine, rd, importHelper, report);

                if (cimRotatingMachine.RatedSHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ROTATINGMACHINE_RATEDS, cimRotatingMachine.RatedS));
                }
            }
        }

/// <summary>
/// Method populates SynchronousMachine properties
/// </summary>
/// <param name="cimSynchronousMachine">SynchronousMachine object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
/// <param name="importHelper"></param>
/// <param name="report"></param>
        public static void PopulateSynchronousMachineProperties(EMS.SynchronousMachine cimSynchronousMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSynchronousMachine != null) && (rd != null))
            {
                EMSConverter.PopulateRotatingMachineProperties(cimSynchronousMachine, rd, importHelper, report);

                if (cimSynchronousMachine.ActiveHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_ACTIVE, cimSynchronousMachine.Active));
                }

                if (cimSynchronousMachine.LoadPctHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_LOADPCT, cimSynchronousMachine.LoadPct));
                }

                if (cimSynchronousMachine.MaxCosPhiHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MAXCOSPHI, cimSynchronousMachine.MaxCosPhi));
                }

                if (cimSynchronousMachine.MinCosPhiHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MINCOSPHI, cimSynchronousMachine.MinCosPhi));
                }

                if (cimSynchronousMachine.MaxQHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MAXQ, cimSynchronousMachine.MaxQ));
                }

                if (cimSynchronousMachine.MinQHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MINQ, cimSynchronousMachine.MinQ));
                }

                if (cimSynchronousMachine.OperatingModeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE, (short)GetSynchronousMachineOperatingMode(cimSynchronousMachine.OperatingMode)));
                }

                if (cimSynchronousMachine.FuelHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSynchronousMachine.Fuel.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSynchronousMachine.GetType().ToString()).Append(" rdfID = \"").Append(cimSynchronousMachine.ID);
                        report.Report.Append("\" - Failed to set reference to PowerSystemResource: rdfID \"").Append(cimSynchronousMachine.Fuel.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_FUEL, gid));
                }
            }
        }

/// <summary>
/// Method populates Measurement properties
/// </summary>
/// <param name="cimMeasurement">Measurement object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
/// <param name="importHelper"></param>
/// <param name="report"></param>
        public static void PopulateMeasurementProperties(EMS.Measurement cimMeasurement, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimMeasurement != null) && (rd != null))
            {
                EMSConverter.PopulateIdentifiedObjectProperties(cimMeasurement, rd);

                if (cimMeasurement.MeasurementTypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_MEASUREMENTTYPE, cimMeasurement.MeasurementType));
                }

                if (cimMeasurement.UnitSymbolHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_UNITSYMBOL, (short)GetUnitSymbol(cimMeasurement.UnitSymbol)));
                }

                if (cimMeasurement.PowerSystemResourceHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimMeasurement.PowerSystemResource.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimMeasurement.GetType().ToString()).Append(" rdfID = \"").Append(cimMeasurement.ID);
                        report.Report.Append("\" - Failed to set reference to PowerSystemResource: rdfID \"").Append(cimMeasurement.PowerSystemResource.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE, gid));
                }
            }
        }

/// <summary>
/// Method populates Analog properties
/// </summary>
/// <param name="cimAnalog">Analog object with values from CIM</param>
/// <param name="rd">ResourceDescription object from importer</param>
/// <param name="importHelper"></param>
/// <param name="report"></param>
        public static void PopulateAnalogProperties(Analog cimAnalog, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimAnalog != null) && (rd != null))
            {
                EMSConverter.PopulateMeasurementProperties(cimAnalog, rd, importHelper, report);

                if (cimAnalog.MaxValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ANALOG_MAXVALUE, cimAnalog.MaxValue));
                }

                if (cimAnalog.MinValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ANALOG_MINVALUE, cimAnalog.MinValue));
                }

                if (cimAnalog.NormalValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ANALOG_NORMALVALUE, cimAnalog.NormalValue));
                }

                if (cimAnalog.SignalDirectionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ANALOG_SIGNALDIRECTION, (short)GetSignalDirection(cimAnalog.SignalDirection)));
                }
            }
        }

        #endregion Populate ResourceDescription


        #region Enums convert

/// <summary>
/// Method returns FuelType enum values for values defined in CIM
/// </summary>
/// <param name="fuelType">EMSFuelType enum value</param>
/// <returns>Concrete value from model</returns>
        public static EmsFuelType GetEMSFuelType(EMS.EmsFuelType fuelType)
        {
            switch (fuelType)
            {
                case EMS.EmsFuelType.coal:
                    return EmsFuelType.coal;
                case EMS.EmsFuelType.hydro:
                    return EmsFuelType.hydro;
                case EMS.EmsFuelType.oil:
                    return EmsFuelType.oli;
                case EMS.EmsFuelType.solar:
                    return EmsFuelType.solar;
                case EMS.EmsFuelType.wind:
                    return EmsFuelType.wind;
                default: return EmsFuelType.coal;
            }
        }

/// <summary>
/// Method returns SignalDirection enum values for values defined in CIM
/// </summary>
/// <param name="signalDirection">SignalDirection enum value</param>
/// <returns>Concrete value from model</returns>
        public static SignalDirection GetSignalDirection(EMS.SignalDirection signalDirection)
        {
            switch (signalDirection)
            {
                case EMS.SignalDirection.Read:
                    return SignalDirection.Read;
                case EMS.SignalDirection.ReadWrite:
                    return SignalDirection.ReadWrite;
                case EMS.SignalDirection.Write:
                    return SignalDirection.Write;
                default: return SignalDirection.Read;
            }
        }

/// <summary>
/// Method returns SynchronousMachineOperatingMode enum values for values defined in CIM
/// </summary>
/// <param name="syncMode">SynchronousMachineOperatingMode enum value</param>
/// <returns>Concrete value from model</returns>
        public static SynchronousMachineOperatingMode GetSynchronousMachineOperatingMode(EMS.SynchronousMachineOperatingMode syncMode)
        {
            switch (syncMode)
            {
                case EMS.SynchronousMachineOperatingMode.condenser:
                    return SynchronousMachineOperatingMode.condenser;
                case EMS.SynchronousMachineOperatingMode.generator:
                    return SynchronousMachineOperatingMode.generator;
                default: return SynchronousMachineOperatingMode.generator;
            }
        }

/// <summary>
/// Method returns UnitSymbol enum values for values defined in CIM
/// </summary>
/// <param name="unitSymbol">UnitSymbol enum value</param>
/// <returns>Concrete value from model</returns>
        public static UnitSymbol GetUnitSymbol(EMS.UnitSymbol unitSymbol)
        {
            switch (unitSymbol)
            {
                case EMS.UnitSymbol.A:
                    return UnitSymbol.A;
                case EMS.UnitSymbol.deg:
                    return UnitSymbol.deg;
                case EMS.UnitSymbol.degC:
                    return UnitSymbol.degC;
                case EMS.UnitSymbol.F:
                    return UnitSymbol.F;
                case EMS.UnitSymbol.g:
                    return UnitSymbol.g;
                case EMS.UnitSymbol.h:
                    return UnitSymbol.h;
                case EMS.UnitSymbol.H:
                    return UnitSymbol.H;
                case EMS.UnitSymbol.Hz:
                    return UnitSymbol.Hz;
                case EMS.UnitSymbol.J:
                    return UnitSymbol.J;
                case EMS.UnitSymbol.m:
                    return UnitSymbol.m;
                case EMS.UnitSymbol.m2:
                    return UnitSymbol.m2;
                case EMS.UnitSymbol.m3:
                    return UnitSymbol.m3;
                case EMS.UnitSymbol.min:
                    return UnitSymbol.min;
                case EMS.UnitSymbol.N:
                    return UnitSymbol.N;
                case EMS.UnitSymbol.none:
                    return UnitSymbol.none;
                case EMS.UnitSymbol.ohm:
                    return UnitSymbol.ohm;
                case EMS.UnitSymbol.Pa:
                    return UnitSymbol.Pa;
                case EMS.UnitSymbol.rad:
                    return UnitSymbol.rad;
                case EMS.UnitSymbol.s:
                    return UnitSymbol.s;
                case EMS.UnitSymbol.S:
                    return UnitSymbol.S;
                case EMS.UnitSymbol.V:
                    return UnitSymbol.V;
                case EMS.UnitSymbol.VA:
                    return UnitSymbol.VA;
                case EMS.UnitSymbol.VAh:
                    return UnitSymbol.VAh;
                case EMS.UnitSymbol.VAr:
                    return UnitSymbol.VAr;
                case EMS.UnitSymbol.VArh:
                    return UnitSymbol.VArh;
                case EMS.UnitSymbol.W:
                    return UnitSymbol.W;
                case EMS.UnitSymbol.Wh:
                    return UnitSymbol.Wh;

                default:
                    return UnitSymbol.m;
            }
        }

        #endregion Enums convert

    }
}
