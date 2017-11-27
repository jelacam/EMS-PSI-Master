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

		ImportHelper helper;
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
			helper = new ImportHelper();
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
		public void PopulatePowerSystemResourcePropertiesTest(string mRid,string name)
		{
			cimPowerSystemResource.MRID = mRid;
			cimPowerSystemResource.Name = name;

			EMSConverter.PopulatePowerSystemResourceProperties(cimPowerSystemResource, rdPowerSystemResource);
			Assert.AreEqual(rdPowerSystemResource.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID).ToString(), mRid);
			Assert.AreEqual(rdPowerSystemResource.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME).ToString(), name);
		}

		#endregion Tests
	}
}
