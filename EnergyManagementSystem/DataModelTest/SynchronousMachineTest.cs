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
        /// Instance of SynchronousMachine
        /// </summary>
        private SynchronousMachine sm1;

        /// <summary>
        /// Instance of SynchronousMachine
        /// </summary>
        private SynchronousMachine sm2;

        /// <summary>
        /// Instance of SynchronousMachine
        /// </summary>
        private SynchronousMachine sm3;

        /// <summary>
        /// Instance of SynchronousMachine
        /// </summary>
        private SynchronousMachine sm4;

        /// <summary>
        /// Instance of SynchronousMachine
        /// </summary>
        private SynchronousMachine sm5;

        /// <summary>
        /// Container for maxQ
        /// </summary>
        private float maxQ1;

        /// <summary>
        /// Container for maxQ
        /// </summary>
        private float maxQ2;

        /// <summary>
        /// Container for minQ
        /// </summary>
        private float minQ1;

        /// <summary>
        /// Container for minQ
        /// </summary>
        private float minQ2;

        /// <summary>
        /// Container for fuelType
        /// </summary>
        private EmsFuelType fuelType1;

        /// <summary>
        /// Container for fuelType
        /// </summary>
        private EmsFuelType fuelType2;

        /// <summary>
        /// Container for operatingMode
        /// </summary>
        private SynchronousMachineOperatingMode operatingMode1;

        /// <summary>
        /// Container for operatingMode
        /// </summary>
        private SynchronousMachineOperatingMode operatingMode2;

        /// <summary>
        /// Container for globalId
        /// </summary>
        private long globalId1;

        /// <summary>
        /// Container for globalId
        /// </summary>
        private long globalId2;

        /// <summary>
        /// Container for true result
        /// </summary>
        private bool resultT;

        /// <summary>
        /// Container for false result
        /// </summary>
        private bool resultF;

        /// <summary>
        /// SetUp method
        /// </summary>
        [OneTimeSetUp]
        public void SetupTest()
        {
            this.globalId1 = 1623;
            this.globalId2 = 10;
            this.maxQ1 = 100;
            this.maxQ2 = 10;
            this.minQ1 = 10;
            this.minQ2 = 1;
            this.fuelType1 = EmsFuelType.coal;
            this.fuelType2 = EmsFuelType.hydro;
            this.operatingMode1 = SynchronousMachineOperatingMode.condenser;
            this.operatingMode2 = SynchronousMachineOperatingMode.generator;
            this.sm1 = new SynchronousMachine(this.globalId1);
            this.sm2 = new SynchronousMachine(this.globalId1);
            this.sm2.MaxQ = this.maxQ1;
            this.sm2.MinQ = this.minQ1;
            //this.sm2.FuelType = this.fuelType1;
            this.sm2.OperatingMode = this.operatingMode1;
            this.sm3 = new SynchronousMachine(this.globalId1);
            this.sm3.MaxQ = this.maxQ2;
            this.sm3.MinQ = this.minQ2;
            //this.sm3.FuelType = this.fuelType2;
            this.sm3.OperatingMode = this.operatingMode2;
            this.sm4 = new SynchronousMachine(this.globalId2);
            this.sm4.MaxQ = this.maxQ1;
            this.sm4.MinQ = this.minQ1;
            //this.sm4.FuelType = this.fuelType1;
            this.sm4.OperatingMode = this.operatingMode1;
            this.sm5 = null;
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineConstructor")]
        public void Constructor()
        {
            SynchronousMachine sm = new SynchronousMachine(this.globalId1);
            Assert.IsNotNull(sm);
        }

        /// <summary>
        /// Unit test for SynchronousMachine FuelType setter
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineFuelTypeProperty")]
        public void FuelTypeProperty()
        {
            //this.sm1.FuelType = this.fuelType1;
            //Assert.AreEqual(this.sm1.FuelType, this.fuelType1);
            //Assert.IsNotNull(this.sm1.FuelType);
        }

        /// <summary>
        /// Unit test for SynchronousMachine MaxQ setter
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineMaxQProperty")]
        public void MaxQProperty()
        {
            this.sm1.MaxQ = this.maxQ1;
            Assert.AreEqual(this.sm1.MaxQ, this.maxQ1);
        }

        /// <summary>
        /// Unit test for SynchronousMachine MinQ setter
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineMinQProperty")]
        public void MinQProperty()
        {
            this.sm1.MinQ = this.minQ1;
            Assert.AreEqual(this.sm1.MinQ, this.minQ1);
        }

        /// <summary>
        /// Unit test for SynchronousMachine OperatingMode setter
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineOperatingModeProperty")]
        public void OperatingModeProperty()
        {
            this.sm1.OperatingMode = this.operatingMode1;
            Assert.AreEqual(this.sm1.OperatingMode, this.operatingMode1);
            Assert.IsNotNull(this.sm1.OperatingMode);
        }

        /// <summary>
        /// Unit test for SynchronousMachine Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineEqualsMethod")]
        public void EqualsMethod()
        {
            //this.sm1.FuelType = this.fuelType1;
            this.sm1.MaxQ = this.maxQ1;
            this.sm1.MinQ = this.minQ1;
            this.sm1.OperatingMode = this.operatingMode1;
            this.resultT = this.sm1.Equals(this.sm2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.sm1.Equals(this.sm3);
            Assert.IsFalse(this.resultF);
            this.resultF = this.sm1.Equals(this.sm4);
            Assert.IsFalse(this.resultF);
            this.resultF = this.sm1.Equals(this.sm5);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for SynchronousMachine GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.sm1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for SynchronousMachine HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.sm1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.sm1.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(this.resultT);
            this.resultT = this.sm1.HasProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsTrue(this.resultT);           
            this.resultT = this.sm1.HasProperty(ModelCode.SYNCHRONOUSMACHINE_MAXQ);
            Assert.IsTrue(this.resultT);
            this.resultT = this.sm1.HasProperty(ModelCode.SYNCHRONOUSMACHINE_MINQ);
            Assert.IsTrue(this.resultT);
            this.resultT = this.sm1.HasProperty(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE);
            Assert.IsTrue(this.resultT);
            this.resultF = this.sm1.HasProperty(ModelCode.SYNCHRONOUSMACHINE);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for SynchronousMachine GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.sm1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.sm1.Mrid);
            this.sm1.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(this.sm1.Measurements);
            this.sm1.GetProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsNotNull(this.sm1.RatedS);           
            this.sm1.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MAXQ);
            Assert.IsNotNull(this.sm1.MaxQ);
            this.sm1.GetProperty(ModelCode.SYNCHRONOUSMACHINE_MINQ);
            Assert.IsNotNull(this.sm1.MinQ);
            this.sm1.GetProperty(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE);
            Assert.IsNotNull(this.sm1.OperatingMode);
        }

        /// <summary>
        /// Unit test for SynchronousMachine SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "SynchronousMachineSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.sm1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.sm1.Mrid);
            this.sm1.SetProperty(new Property(ModelCode.ROTATINGMACHINE_RATEDS));
            Assert.IsNotNull(this.sm1.RatedS);
            this.sm1.SetProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MAXQ));
            Assert.IsNotNull(this.sm1.MaxQ);
            this.sm1.SetProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_MINQ));
            Assert.IsNotNull(this.sm1.MinQ);
            this.sm1.SetProperty(new Property(ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE));
            Assert.IsNotNull(this.sm1.OperatingMode);
        }
    }
}