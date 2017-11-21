namespace EMS.CIMAdapter.Importer
{
    using System;
    using EMS.Common;

    public static class EMSConverter
    {

        #region Populate ResourceDescription

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

        public static void PopulatePowerSystemResourceProperties(EMS.PowerSystemResource cimPowerSystemResource, ResourceDescription rd)
        {
            if ((cimPowerSystemResource != null) && (rd != null))
            {
                EMSConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
            }
        }

        public static void PopulateEquipmentProperties(EMS.Equipment cimEquipment, ResourceDescription rd)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                EMSConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd);
            }
        }

        public static void PopulateConductingEquipmentProperties(EMS.ConductingEquipment cimConductingEquipment, ResourceDescription rd)
        {
            if ((cimConductingEquipment != null) && (rd != null))
            {
                EMSConverter.PopulateEquipmentProperties(cimConductingEquipment, rd);
            }
        }

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

        public static void PopulateRegulatingCondEqProperties(RegulatingCondEq cimRegulatingCondEq, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegulatingCondEq != null) && (rd != null))
            {
                EMSConverter.PopulateConductingEquipmentProperties(cimRegulatingCondEq, rd);
            }
        }

        public static void PopulateRotatingMachineProperties(RotatingMachine cimRotatingMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
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

        public static void PopulateSynchronousMachineProperties(EMS.SynchronousMachine cimSynchronousMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSynchronousMachine != null) && (rd != null))
            {
                EMSConverter.PopulateRotatingMachineProperties(cimSynchronousMachine, rd, importHelper, report);

                if (cimSynchronousMachine.FuelTypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE, (short)GetEMSFuelType(cimSynchronousMachine.FuelType)));
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
            }
        }

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
