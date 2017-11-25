using EMS.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelTest
{
	[TestFixture]
	public class ModelCodeTest
	{
		[Test]
		public void TypeFromModelCodeTest()
		{
			//ANALOG
			Assert.AreEqual(EMSType.ANALOG, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ANALOG));
			Assert.AreEqual(EMSType.ANALOG, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ANALOG_MAXVALUE));
			Assert.AreEqual(EMSType.ANALOG, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ANALOG_MINVALUE));
			Assert.AreEqual(EMSType.ANALOG, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ANALOG_NORMALVALUE));
			Assert.AreEqual(EMSType.ANALOG, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ANALOG_SIGNALDIRECTION));

			//ENERGYCONSUMER
			Assert.AreEqual(EMSType.ENERGYCONSUMER, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ENERGYCONSUMER));
			Assert.AreEqual(EMSType.ENERGYCONSUMER, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ENERGYCONSUMER_PFIXED));
			Assert.AreEqual(EMSType.ENERGYCONSUMER, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ENERGYCONSUMER_PFIXEDPCT));
			Assert.AreEqual(EMSType.ENERGYCONSUMER, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ENERGYCONSUMER_QFIXED));
			Assert.AreEqual(EMSType.ENERGYCONSUMER, ModelCodeHelper.GetTypeFromModelCode(ModelCode.ENERGYCONSUMER_QFIXEDPCT));

			//SYNCHRONOUSMACHINE
			Assert.AreEqual(EMSType.SYNCHRONOUSMACHINE, ModelCodeHelper.GetTypeFromModelCode(ModelCode.SYNCHRONOUSMACHINE));
			Assert.AreEqual(EMSType.SYNCHRONOUSMACHINE, ModelCodeHelper.GetTypeFromModelCode(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE));
			Assert.AreEqual(EMSType.SYNCHRONOUSMACHINE, ModelCodeHelper.GetTypeFromModelCode(ModelCode.SYNCHRONOUSMACHINE_MAXQ));
			Assert.AreEqual(EMSType.SYNCHRONOUSMACHINE, ModelCodeHelper.GetTypeFromModelCode(ModelCode.SYNCHRONOUSMACHINE_MINQ));
			Assert.AreEqual(EMSType.SYNCHRONOUSMACHINE, ModelCodeHelper.GetTypeFromModelCode(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE));
		}

		[Test]
		public void UniqueModelCodeTest()
		{
			IEnumerable<long> modelCodeArray = Enum.GetValues(typeof(ModelCode)).Cast<long>().Select(x => (long)x);
			string[] modelCodeNames = Enum.GetNames(typeof(ModelCode));

			// check for duplicate
			Assert.AreEqual(modelCodeArray.Count(), modelCodeArray.Distinct().Count());
			Assert.AreEqual(modelCodeNames.Length, modelCodeNames.Distinct().Count());
		}

		[Test]
		public void InheritanceByteForAttributeModelCodeTest()
		{
			long inhIdObj = ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.IDENTIFIEDOBJECT);
			Assert.AreEqual(inhIdObj, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.IDENTIFIEDOBJECT_GID));
			Assert.AreEqual(inhIdObj, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.IDENTIFIEDOBJECT_MRID));
			Assert.AreEqual(inhIdObj, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.IDENTIFIEDOBJECT_NAME));

			long inhMeasurement = ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.MEASUREMENT);
			Assert.AreEqual(inhMeasurement, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.MEASUREMENT_MEASUREMENTTYPE));
			Assert.AreEqual(inhMeasurement, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE));
			Assert.AreEqual(inhMeasurement, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.MEASUREMENT_UNITSYMBOL));

			long inhPwrSrc = ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.POWERSYSTEMRESOURCE);
			Assert.AreEqual(inhPwrSrc, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS));

			long inhRotMachine = ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ROTATINGMACHINE);
			Assert.AreEqual(inhRotMachine, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ROTATINGMACHINE_RATEDS));

			//Concrete
			long inheritanceAnalog = ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ANALOG);
			Assert.AreEqual(inheritanceAnalog, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ANALOG_MAXVALUE));
			Assert.AreEqual(inheritanceAnalog, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ANALOG_MAXVALUE));
			Assert.AreEqual(inheritanceAnalog, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ANALOG_MINVALUE));
			Assert.AreEqual(inheritanceAnalog, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ANALOG_NORMALVALUE));
			Assert.AreEqual(inheritanceAnalog, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ANALOG_SIGNALDIRECTION));

			long inhEnergyConsumer = ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ENERGYCONSUMER);
			Assert.AreEqual(inhEnergyConsumer, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ENERGYCONSUMER_PFIXED));
			Assert.AreEqual(inhEnergyConsumer, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ENERGYCONSUMER_PFIXEDPCT));
			Assert.AreEqual(inhEnergyConsumer, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ENERGYCONSUMER_QFIXED));
			Assert.AreEqual(inhEnergyConsumer, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.ENERGYCONSUMER_QFIXEDPCT));

			long inhSyncMachine = ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.SYNCHRONOUSMACHINE);
			Assert.AreEqual(inhSyncMachine, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE));
			Assert.AreEqual(inhSyncMachine, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.SYNCHRONOUSMACHINE_MAXQ));
			Assert.AreEqual(inhSyncMachine, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.SYNCHRONOUSMACHINE_MINQ));
			Assert.AreEqual(inhSyncMachine, ModelCodeHelper.GetInheritanceOnlyFromModelCode(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE));
		}

		[Test]
		public void CheckInheritance()
		{
			//IDENTIFIEDOBJECT parent
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.ANALOG));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.CONDUCTINGEQUIPMENT));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.ENERGYCONSUMER));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.EQUIPMENT));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.MEASUREMENT));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.POWERSYSTEMRESOURCE));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.REGULATINGCONDEQ));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.ROTATINGMACHINE));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.IDENTIFIEDOBJECT, ModelCode.SYNCHRONOUSMACHINE));

			//POWERSYSTEMRESOURCE parent
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.POWERSYSTEMRESOURCE, ModelCode.EQUIPMENT));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.POWERSYSTEMRESOURCE, ModelCode.CONDUCTINGEQUIPMENT));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.POWERSYSTEMRESOURCE, ModelCode.ENERGYCONSUMER));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.POWERSYSTEMRESOURCE, ModelCode.REGULATINGCONDEQ));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.POWERSYSTEMRESOURCE, ModelCode.ROTATINGMACHINE));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.POWERSYSTEMRESOURCE, ModelCode.SYNCHRONOUSMACHINE));

			//EQUIPMENT parent
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.EQUIPMENT, ModelCode.CONDUCTINGEQUIPMENT));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.EQUIPMENT, ModelCode.ENERGYCONSUMER));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.EQUIPMENT, ModelCode.REGULATINGCONDEQ));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.EQUIPMENT, ModelCode.ROTATINGMACHINE));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.EQUIPMENT, ModelCode.SYNCHRONOUSMACHINE));

			//CONDUCTINGEQUIPMENT parent
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.CONDUCTINGEQUIPMENT, ModelCode.ENERGYCONSUMER));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.CONDUCTINGEQUIPMENT, ModelCode.REGULATINGCONDEQ));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.CONDUCTINGEQUIPMENT, ModelCode.ROTATINGMACHINE));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.CONDUCTINGEQUIPMENT, ModelCode.SYNCHRONOUSMACHINE));

			//REGULATINGCONDEQ parent
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.REGULATINGCONDEQ, ModelCode.ROTATINGMACHINE));
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.REGULATINGCONDEQ, ModelCode.SYNCHRONOUSMACHINE));

			//ROTATINGMACHINE parent
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.ROTATINGMACHINE, ModelCode.SYNCHRONOUSMACHINE));

			//MEASUREMENT parent
			Assert.IsTrue(ModelCodeHelper.IsInheritanceByModelCode(ModelCode.MEASUREMENT, ModelCode.ANALOG));

		}
	}
}
