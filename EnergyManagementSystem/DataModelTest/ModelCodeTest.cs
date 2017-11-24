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

	}
}
