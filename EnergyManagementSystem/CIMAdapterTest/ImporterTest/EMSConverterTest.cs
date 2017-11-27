using EMS.CIMAdapter;
using EMS.CIMAdapter.Importer;
using EMS.Common;
using EMS.Services.NetworkModelService.DataModel.Core;
using EMS.Services.NetworkModelService.DataModel.Meas;
using EMS.Services.NetworkModelService.DataModel.Wires;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIMAdapterTest.ImporterTest
{
	[TestFixture]
	public class EMSConverterTest
	{
		#region Declarations

		ImportHelper importHelper;
		TransformAndLoadReport report;

		ResourceDescription rdIdentifiedObject;
		ResourceDescription rdPowerSystemResource;
		ResourceDescription rdEquipment;
		ResourceDescription rdConductingEquipment;
		ResourceDescription rdEnergyConsumer;
		ResourceDescription rdRegulatingCondEq;
		ResourceDescription rdRotatingMachine;
		ResourceDescription rdSynchronousMachine;
		ResourceDescription rdMeasurement;
		ResourceDescription rdAnalog;

		EMS.IdentifiedObject cimIdentifiedObject;
		EMS.PowerSystemResource cimPowerSystemResource;
		EMS.Equipment cimEquipment;
		EMS.ConductingEquipment cimConductingEquipment;
		EMS.EnergyConsumer cimEnergyConsumer;
		EMS.RegulatingCondEq cimRegulatingCondEq;
		EMS.RotatingMachine cimRotatingMachine;
		EMS.SynchronousMachine cimSynchronousMachine;
		EMS.Measurement cimMeasurement;
		EMS.Analog cimAnalog;

		#endregion Declarations

		#region Setup

		[OneTimeSetUp]
		public void SetupTest()
		{
			importHelper = new ImportHelper();
			report = new TransformAndLoadReport();

			rdIdentifiedObject = new ResourceDescription();
			rdPowerSystemResource = new ResourceDescription();
			rdEquipment = new ResourceDescription();
			rdConductingEquipment = new ResourceDescription();
			rdEnergyConsumer = new ResourceDescription();
			rdRegulatingCondEq = new ResourceDescription();
			rdRotatingMachine = new ResourceDescription();
			rdSynchronousMachine = new ResourceDescription();
			rdMeasurement = new ResourceDescription();
			rdAnalog = new ResourceDescription();

			cimIdentifiedObject = new EMS.IdentifiedObject();
			cimPowerSystemResource = new EMS.PowerSystemResource();
			cimEquipment = new EMS.Equipment();
			cimConductingEquipment = new EMS.ConductingEquipment();
			cimEnergyConsumer = new EMS.EnergyConsumer();
			cimRegulatingCondEq = new EMS.RegulatingCondEq();
			cimRotatingMachine = new EMS.RotatingMachine();
			cimSynchronousMachine = new EMS.SynchronousMachine();
			cimMeasurement = new EMS.Measurement();
			cimAnalog = new EMS.Analog();
		}

		#endregion Setup

		#region Tests

		[Test]
		[TestCase("1000", "TestIdentifiedObject", TestName = "PopulateIdentifiedObjectPropertiesTest")]
		public void PopulateIdentifiedObjectPropertiesTest(string mRid, string name)
		{
			cimIdentifiedObject.MRID = mRid;
			cimIdentifiedObject.Name = name;

			EMSConverter.PopulateIdentifiedObjectProperties(cimIdentifiedObject, rdIdentifiedObject);
			Assert.AreEqual(rdIdentifiedObject.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdIdentifiedObject.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
		}

		[Test]
		[TestCase("1001", "TestPowerSystemResource", TestName = "PopulatePowerSystemResourcePropertiesTest")]
		public void PopulatePowerSystemResourcePropertiesTest(string mRid, string name)
		{
			cimPowerSystemResource.MRID = mRid;
			cimPowerSystemResource.Name = name;

			EMSConverter.PopulatePowerSystemResourceProperties(cimPowerSystemResource, rdPowerSystemResource);
			Assert.AreEqual(rdPowerSystemResource.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdPowerSystemResource.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
		}

		[Test]
		[TestCase("1002", "TestEquipment", TestName = "PopulateEquipmentPropertiesTest")]
		public void PopulateEquipmentPropertiessTest(string mRid, string name)
		{
			cimEquipment.MRID = mRid;
			cimEquipment.Name = name;

			EMSConverter.PopulateEquipmentProperties(cimEquipment, rdEquipment);
			Assert.AreEqual(rdEquipment.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdEquipment.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
		}

		[Test]
		[TestCase("1003", "TestConductingEquipment", TestName = "PopulateConductingEquipmenttPropertiesTest")]
		public void PopulateConductingEquipmentPropertiesTest(string mRid, string name)
		{
			cimConductingEquipment.MRID = mRid;
			cimConductingEquipment.Name = name;

			EMSConverter.PopulateConductingEquipmentProperties(cimConductingEquipment, rdConductingEquipment);
			Assert.AreEqual(rdConductingEquipment.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdConductingEquipment.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
		}

		[Test]
		[TestCase("1004", "TestEnergyConsumer", 10, 10, 10, 10, TestName = "PopulateEnergyConsumerPropertiesTest")]
		public void PopulateEnergyConsumerPropertiesTest(string mRid, string name, float pFixed, float pFixedPct, float qFixed, float qFixedPct)
		{
			cimEnergyConsumer.MRID = mRid;
			cimEnergyConsumer.Name = name;
			cimEnergyConsumer.Pfixed = pFixed;
			cimEnergyConsumer.PfixedPct = pFixedPct;
			cimEnergyConsumer.Qfixed = qFixed;
			cimEnergyConsumer.QfixedPct = qFixedPct;

			EMSConverter.PopulateEnergyConsumerProperties(cimEnergyConsumer, rdEnergyConsumer, importHelper, report);
			Assert.AreEqual(rdEnergyConsumer.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdEnergyConsumer.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
			Assert.AreEqual(rdEnergyConsumer.GetProperty(ModelCode.ENERGYCONSUMER_PFIXED).AsFloat(), pFixed);
			Assert.AreEqual(rdEnergyConsumer.GetProperty(ModelCode.ENERGYCONSUMER_PFIXEDPCT).AsFloat(), pFixedPct);
			Assert.AreEqual(rdEnergyConsumer.GetProperty(ModelCode.ENERGYCONSUMER_QFIXED).AsFloat(), qFixed);
			Assert.AreEqual(rdEnergyConsumer.GetProperty(ModelCode.ENERGYCONSUMER_QFIXEDPCT).AsFloat(), qFixedPct);
		}

		[Test]
		[TestCase("1004", "TestRegulatingCondEq", TestName = "PopulateRegulatingCondEqPropertiesTest")]
		public void PopulateRegulatingCondEqPropertiesTest(string mRid, string name)
		{
			cimRegulatingCondEq.MRID = mRid;
			cimRegulatingCondEq.Name = name;

			EMSConverter.PopulateRegulatingCondEqProperties(cimRegulatingCondEq, rdRegulatingCondEq, importHelper, report);
			Assert.AreEqual(rdRegulatingCondEq.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdRegulatingCondEq.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
		}

		[Test]
		[TestCase("1005", "TestRegulatingCondEq", 10, TestName = "PopulateRotatingMachinePropertiesTest")]
		public void PopulateRotatingMachinePropertiesTest(string mRid, string name, float ratedS)
		{
			cimRotatingMachine.MRID = mRid;
			cimRotatingMachine.Name = name;
			cimRotatingMachine.RatedS = ratedS;

			EMSConverter.PopulateRotatingMachineProperties(cimRotatingMachine, rdRotatingMachine, importHelper, report);
			Assert.AreEqual(rdRotatingMachine.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdRotatingMachine.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
			Assert.AreEqual(rdRotatingMachine.GetProperty(ModelCode.ROTATINGMACHINE_RATEDS).AsFloat(), ratedS);
		}

		[Test]
		[TestCase("1006", "TestSynchronousMachine", 10, EMS.EmsFuelType.coal, EMS.SynchronousMachineOperatingMode.generator,
			10, 10, TestName = "PopulateSynchronousMachineePropertiesTest")]
		public void PopulateSynchronousMachinePropertiesTest(string mRid, string name, float ratedS, EMS.EmsFuelType fuelType,
			EMS.SynchronousMachineOperatingMode operatingMode, float maxQ, float minQ)
		{
			cimSynchronousMachine.MRID = mRid;
			cimSynchronousMachine.Name = name;
			cimSynchronousMachine.RatedS = ratedS;
			cimSynchronousMachine.FuelType = fuelType;
			cimSynchronousMachine.OperatingMode = operatingMode;
			cimSynchronousMachine.MaxQ = maxQ;
			cimSynchronousMachine.MinQ = minQ;

			EMSConverter.PopulateSynchronousMachineProperties(cimSynchronousMachine, rdSynchronousMachine, importHelper, report);
			Assert.AreEqual(rdSynchronousMachine.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdSynchronousMachine.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
			Assert.AreEqual(rdSynchronousMachine.GetProperty(ModelCode.ROTATINGMACHINE_RATEDS).AsFloat(), ratedS);
			Assert.AreEqual(rdSynchronousMachine.GetProperty(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE).AsEnum(), 0);
			Assert.AreEqual(rdSynchronousMachine.GetProperty(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE).AsEnum(), 0);
			Assert.AreEqual(rdSynchronousMachine.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MAXQ).AsFloat(), maxQ);
			Assert.AreEqual(rdSynchronousMachine.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MINQ).AsFloat(), minQ);
		}

		[Test]
		[TestCase("1007", "TestMeasurement", "Napon", EMS.UnitSymbol.VA, TestName = "PopulateMeasurementPropertiesTest")]
		public void PopulateMeasurementPropertiesTest(string mRid, string name, string measurementType, EMS.UnitSymbol unitSymbol)
		{
			cimMeasurement.MRID = mRid;
			cimMeasurement.Name = name;
			cimMeasurement.MeasurementType = measurementType;
			cimMeasurement.UnitSymbol = unitSymbol;
			
			EMSConverter.PopulateMeasurementProperties(cimMeasurement, rdMeasurement, importHelper, report);
			Assert.AreEqual(rdMeasurement.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdMeasurement.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
			Assert.AreEqual(rdMeasurement.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE).ToString(), measurementType);
			Assert.AreEqual(rdMeasurement.GetProperty(ModelCode.MEASUREMENT_UNITSYMBOL).AsEnum(), 0);
		}

		[Test]
		[TestCase("1008", "TestAnalog",10,10,10,EMS.SignalDirection.Read, TestName = "PopulateAnalogPropertiesTest")]
		public void PopulateAnalogPropertiesTest(string mRid, string name, float maxValue, float minValue, float normalValue, EMS.SignalDirection signalDirection)
		{
			cimAnalog.MRID = mRid;
			cimAnalog.Name = name;
			cimAnalog.MaxValue = maxValue;
			cimAnalog.MinValue = minValue;
			cimAnalog.NormalValue = normalValue;
			cimAnalog.SignalDirection = signalDirection;

			EMSConverter.PopulateAnalogProperties(cimAnalog, rdAnalog, importHelper, report);
			Assert.AreEqual(rdAnalog.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdAnalog.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
			Assert.AreEqual(rdAnalog.GetProperty(ModelCode.ANALOG_MAXVALUE).AsFloat(), maxValue);
			Assert.AreEqual(rdAnalog.GetProperty(ModelCode.ANALOG_MINVALUE).AsFloat(), minValue);
			Assert.AreEqual(rdAnalog.GetProperty(ModelCode.ANALOG_NORMALVALUE).AsFloat(), normalValue);
			Assert.AreEqual(rdAnalog.GetProperty(ModelCode.ANALOG_SIGNALDIRECTION).AsEnum(), 0);
		}

		#endregion Tests
	}
}
