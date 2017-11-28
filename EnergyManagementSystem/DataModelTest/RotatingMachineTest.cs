//-----------------------------------------------------------------------
// <copyright file="RotatingMachineTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Wires;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing RotatingMachine
    /// </summary>
    [TestFixture]
    public class RotatingMachineTest
    {
        /// <summary>
        /// Instance of RotatingMachine
        /// </summary>
        private RotatingMachine rm1;

        /// <summary>
        /// Instance of RotatingMachine
        /// </summary>
        private RotatingMachine rm2;

        /// <summary>
        /// Instance of RotatingMachine
        /// </summary>
        private RotatingMachine rm3;

        /// <summary>
        /// Instance of RotatingMachine
        /// </summary>
        private RotatingMachine rm4;

        /// <summary>
        /// Instance of RotatingMachine
        /// </summary>
        private RotatingMachine rm5;

        /// <summary>
        /// Container for ratedS
        /// </summary>
        private float ratedS1;

        /// <summary>
        /// Container for ratedS
        /// </summary>
        private float ratedS2;

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
            this.ratedS1 = 100;
            this.ratedS2 = 1;
            this.rm1 = new RotatingMachine(globalId1);
            this.rm2 = new RotatingMachine(globalId1);
            this.rm2.RatedS = this.ratedS1;
            this.rm3 = new RotatingMachine(globalId1);
            this.rm3.RatedS = this.ratedS2;
            this.rm4 = new RotatingMachine(globalId2);
            this.rm4.RatedS = this.ratedS1;
            this.rm5 = null;
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "RotatingMachineConstructor")]
        public void Constructor()
        {
            RotatingMachine rm = new RotatingMachine(this.globalId1);
            Assert.IsNotNull(rm);
        }

        /// <summary>
        /// Unit test for RotatingMachine RatedS setter
        /// </summary>
        [Test]
        [TestCase(TestName = "RotatingMachineRatedSProperty")]
        public void RatedSProperty()
        {
            this.rm1.RatedS = this.ratedS1;
            Assert.AreEqual(this.rm1.RatedS, this.ratedS1);
        }

        /// <summary>
        /// Unit test for RotatingMachine Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "RotatingMachineEqualsMethod")]
        public void EqualsMethod()
        {
            this.rm1.RatedS = this.ratedS1;
            this.rm2.RatedS = this.ratedS1;
            this.rm3.RatedS = this.ratedS2;
            this.rm4.RatedS = this.ratedS1;
            this.resultT = this.rm1.Equals(this.rm2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.rm1.Equals(this.rm3);
            Assert.IsFalse(this.resultF);
            this.resultF = this.rm1.Equals(this.rm4);
            Assert.IsFalse(this.resultF);
            this.resultF = this.rm1.Equals(this.rm5);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for RotatingMachine GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "RotatingMachineGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.rm1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for RotatingMachine HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "RotatingMachineHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.rm1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.rm1.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(this.resultT);
            this.resultT = this.rm1.HasProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsTrue(this.resultT);
            this.resultF = this.rm1.HasProperty(ModelCode.ROTATINGMACHINE);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for RotatingMachine GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "RotatingMachineGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.rm1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.rm1.Mrid);
            this.rm1.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(this.rm1.Measurements);
            this.rm1.GetProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsNotNull(this.rm1.RatedS);
        }

        /// <summary>
        /// Unit test for RotatingMachine SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "RotatingMachineSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.rm1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.rm1.Mrid);
            this.rm1.SetProperty(new Property(ModelCode.ROTATINGMACHINE_RATEDS));
            Assert.IsNotNull(this.rm1.RatedS);
        }
    }
}