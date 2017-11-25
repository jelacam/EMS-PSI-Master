//-----------------------------------------------------------------------
// <copyright file="SynchronousMachineTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Wires;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing SynchronousMachine
    /// </summary>
    [TestFixture]
    public class SynchronousMachineTest
    {
        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            SynchronousMachine sm = new SynchronousMachine(globalId);
            Assert.IsNotNull(sm);
        }

        /// <summary>
        /// Unit test for SynchronousMachine FuelType setter
        /// </summary>
        /// <param name="fuelType">fuelType property being set and asserted</param>
        [Test]
        [TestCase(EmsFuelType.coal)]
        public void FuelTypePropertySet(EmsFuelType fuelType)
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            sm.FuelType = fuelType;
            Assert.AreEqual(sm.FuelType, fuelType);
            Assert.IsNotNull(sm.FuelType);
        }

        /// <summary>
        /// Unit test for SynchronousMachine MaxQ setter
        /// </summary>
        /// <param name="maxQ">maxQ property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void MaxQPropertySet(float maxQ)
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            sm.MaxQ = maxQ;
            Assert.AreEqual(sm.MaxQ, maxQ);
        }

        /// <summary>
        /// Unit test for SynchronousMachine MinQ setter
        /// </summary>
        /// <param name="minQ">minQ property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void MinQPropertySet(float minQ)
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            sm.MinQ = minQ;
            Assert.AreEqual(sm.MinQ, minQ);
        }

        /// <summary>
        /// Unit test for SynchronousMachine OperatingMode setter
        /// </summary>
        /// <param name="operatingMode">operatingMode property being set and asserted</param>
        [Test]
        [TestCase(SynchronousMachineOperatingMode.generator)]
        public void OperatingModePropertySet(SynchronousMachineOperatingMode operatingMode)
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            sm.OperatingMode = operatingMode;
            Assert.AreEqual(sm.OperatingMode, operatingMode);
            Assert.IsNotNull(sm.OperatingMode);
        }

        /// <summary>
        /// Unit test for SynchronousMachine Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            SynchronousMachine sm1 = new SynchronousMachine(globalId1);
            SynchronousMachine sm2 = new SynchronousMachine(globalId1);
            SynchronousMachine sm3 = null;
            SynchronousMachine sm4 = new SynchronousMachine(globalId1);
            SynchronousMachine sm5 = new SynchronousMachine(globalId2);
            sm1.FuelType = EmsFuelType.coal;
            sm1.MaxQ = 10;
            sm1.MinQ = 1;
            sm1.OperatingMode = SynchronousMachineOperatingMode.generator;
            sm2.FuelType = EmsFuelType.coal;
            sm2.MaxQ = 10;
            sm2.MinQ = 1;
            sm2.OperatingMode = SynchronousMachineOperatingMode.generator;
            sm4.FuelType = EmsFuelType.hydro;
            sm4.MaxQ = 100;
            sm4.MinQ = 10;
            sm4.OperatingMode = SynchronousMachineOperatingMode.condenser;
            sm5.FuelType = EmsFuelType.coal;
            sm5.MaxQ = 10;
            sm5.MinQ = 1;
            sm5.OperatingMode = SynchronousMachineOperatingMode.generator;
            bool resultT = sm1.Equals(sm2);
            Assert.IsTrue(resultT);
            bool resultF = sm1.Equals(sm3);
            Assert.IsFalse(resultF);
            resultF = sm1.Equals(sm4);
            Assert.IsFalse(resultF);
            resultF = sm1.Equals(sm5);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for SynchronousMachine GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            int result = sm.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for SynchronousMachine HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            bool resultT = sm.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = sm.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(resultT);
            resultT = sm.HasProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsTrue(resultT);
            resultT = sm.HasProperty(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE);
            Assert.IsTrue(resultT);
            resultT = sm.HasProperty(ModelCode.SYNCHRONOUSMACHINE_MAXQ);
            Assert.IsTrue(resultT);
            resultT = sm.HasProperty(ModelCode.SYNCHRONOUSMACHINE_MINQ);
            Assert.IsTrue(resultT);
            resultT = sm.HasProperty(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE);
            Assert.IsTrue(resultT);
            bool resultF = sm.HasProperty(ModelCode.SYNCHRONOUSMACHINE);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for SynchronousMachine GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            sm.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(sm.Mrid);
            sm.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(sm.Measurements);
            sm.GetProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsNotNull(sm.RatedS);
            sm.GetProperty(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE);
            Assert.IsNotNull(sm.FuelType);
            sm.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MAXQ);
            Assert.IsNotNull(sm.MaxQ);
            sm.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MINQ);
            Assert.IsNotNull(sm.MinQ);
            sm.GetProperty(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE);
            Assert.IsNotNull(sm.OperatingMode);
        }

        /// <summary>
        /// Unit test for SynchronousMachine SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            SynchronousMachine sm = new SynchronousMachine(1623);
            sm.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(sm.Mrid);
            sm.SetProperty(new Property(ModelCode.ROTATINGMACHINE_RATEDS));
            Assert.IsNotNull(sm.RatedS);
            sm.SetProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_FUELTYPE));
            Assert.IsNotNull(sm.FuelType);
            sm.SetProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MAXQ));
            Assert.IsNotNull(sm.MaxQ);
            sm.SetProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MINQ));
            Assert.IsNotNull(sm.MinQ);
            sm.SetProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE));
            Assert.IsNotNull(sm.OperatingMode);
        }
    }
}