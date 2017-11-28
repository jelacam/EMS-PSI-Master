//-----------------------------------------------------------------------
// <copyright file="EnergyConsumerTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Wires;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing EnergyConsumer
    /// </summary>
    [TestFixture]
    public class EnergyConsumerTest
    {
        /// <summary>
        /// Instance of EnergyConsumer
        /// </summary>
        private EnergyConsumer ec1;

        /// <summary>
        /// Instance of EnergyConsumer
        /// </summary>
        private EnergyConsumer ec2;

        /// <summary>
        /// Instance of EnergyConsumer
        /// </summary>
        private EnergyConsumer ec3;

        /// <summary>
        /// Instance of EnergyConsumer
        /// </summary>
        private EnergyConsumer ec4;

        /// <summary>
        /// Instance of EnergyConsumer
        /// </summary>
        private EnergyConsumer ec5;

        /// <summary>
        /// Container for pFixed
        /// </summary>
        private float pFixed1;

        /// <summary>
        /// Container for pFixed
        /// </summary>
        private float pFixed2;

        /// <summary>
        /// Container for pFixedPct
        /// </summary>
        private float pFixedPct1;

        /// <summary>
        /// Container for pFixedPct
        /// </summary>
        private float pFixedPct2;

        /// <summary>
        /// Container for qFixed
        /// </summary>
        private float qFixed1;

        /// <summary>
        /// Container for qFixed
        /// </summary>
        private float qFixed2;

        /// <summary>
        /// Container for qFixedPct
        /// </summary>
        private float qFixedPct1;

        /// <summary>
        /// Container for qFixedPct
        /// </summary>
        private float qFixedPct2;

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
            this.pFixed1 = 100;
            this.pFixed2 = 200;
            this.pFixedPct1 = 10;
            this.pFixedPct2 = 20;
            this.qFixed1 = 10;
            this.qFixed2 = 20;
            this.qFixedPct1 = 1;
            this.qFixedPct2 = 2;
            this.ec1 = new EnergyConsumer(this.globalId1);
            this.ec2 = new EnergyConsumer(this.globalId1);
            this.ec2.PFixed = this.pFixed1;
            this.ec2.PFixedPct = this.pFixedPct1;
            this.ec2.QFixed = this.qFixed1;
            this.ec2.QFixedPct = this.qFixedPct1;
            this.ec3 = new EnergyConsumer(this.globalId1);
            this.ec3.PFixed = this.pFixed2;
            this.ec3.PFixedPct = this.pFixedPct2;
            this.ec3.QFixed = this.qFixed2;
            this.ec3.QFixedPct = this.qFixedPct2;
            this.ec4 = new EnergyConsumer(this.globalId2);
            this.ec4.PFixed = this.pFixed1;
            this.ec4.PFixedPct = this.pFixedPct1;
            this.ec4.QFixed = this.qFixed1;
            this.ec4.QFixedPct = this.qFixedPct1;
            this.ec5 = null;
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerConstructor")]
        public void Constructor()
        {
            EnergyConsumer ec = new EnergyConsumer(this.globalId1);
            Assert.IsNotNull(ec);
        }

        /// <summary>
        /// Unit test for EnergyConsumer PFixed setter
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerPFixedProperty")]
        public void PFixedProperty()
        {
            this.ec1.PFixed = this.pFixed1;
            Assert.AreEqual(this.ec1.PFixed, this.pFixed1);
        }

        /// <summary>
        /// Unit test for EnergyConsumer PFixedPct setter
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerPFixedPctProperty")]
        public void PFixedPctProperty()
        {
            this.ec1.PFixedPct = this.pFixedPct1;
            Assert.AreEqual(this.ec1.PFixedPct, this.pFixedPct1);
        }

        /// <summary>
        /// Unit test for EnergyConsumer QFixed setter
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerQFixedProperty")]
        public void QFixedProperty()
        {
            this.ec1.QFixed = this.qFixed1;
            Assert.AreEqual(this.ec1.QFixed, this.qFixed1);
        }

        /// <summary>
        /// Unit test for EnergyConsumer QFixedPct setter
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerQFixedPctProperty")]
        public void QFixedPctProperty()
        {
            this.ec1.QFixedPct = this.qFixedPct1;
            Assert.AreEqual(this.ec1.QFixedPct, this.qFixedPct1);
        }

        /// <summary>
        /// Unit test for EnergyConsumer Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerEqualsMethod")]
        public void EqualsMethod()
        {
            this.ec1.PFixed = this.pFixed1;
            this.ec1.PFixedPct = this.pFixedPct1;
            this.ec1.QFixed = this.qFixed1;
            this.ec1.QFixedPct = this.qFixedPct1;
            this.resultT = this.ec1.Equals(this.ec2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.ec1.Equals(this.ec3);
            Assert.IsFalse(this.resultF);
            this.resultF = this.ec1.Equals(this.ec4);
            Assert.IsFalse(this.resultF);
            this.resultF = this.ec1.Equals(this.ec5);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for EnergyConsumer GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.ec1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for EnergyConsumer HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.ec1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.ec1.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(this.resultT);
            this.resultT = this.ec1.HasProperty(ModelCode.ENERGYCONSUMER_PFIXED);
            Assert.IsTrue(this.resultT);
            this.resultT = this.ec1.HasProperty(ModelCode.ENERGYCONSUMER_PFIXEDPCT);
            Assert.IsTrue(this.resultT);
            this.resultT = this.ec1.HasProperty(ModelCode.ENERGYCONSUMER_QFIXED);
            Assert.IsTrue(this.resultT);
            this.resultT = this.ec1.HasProperty(ModelCode.ENERGYCONSUMER_QFIXEDPCT);
            Assert.IsTrue(this.resultT);
            this.resultF = this.ec1.HasProperty(ModelCode.ENERGYCONSUMER);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for EnergyConsumer GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.ec1.GetProperty(ModelCode.ENERGYCONSUMER_PFIXED);
            Assert.IsNotNull(this.ec1.PFixed);
            this.ec1.GetProperty(ModelCode.ENERGYCONSUMER_PFIXEDPCT);
            Assert.IsNotNull(this.ec1.PFixedPct);
            this.ec1.GetProperty(ModelCode.ENERGYCONSUMER_QFIXED);
            Assert.IsNotNull(this.ec1.QFixed);
            this.ec1.GetProperty(ModelCode.ENERGYCONSUMER_QFIXEDPCT);
            Assert.IsNotNull(this.ec1.QFixedPct);
            this.ec1.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(this.ec1.Measurements);
            this.ec1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.ec1.Mrid);
        }

        /// <summary>
        /// Unit test for EnergyConsumer SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "EnergyConsumerSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.ec1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.ec1.Mrid);
            this.ec1.SetProperty(new Property(ModelCode.ENERGYCONSUMER_PFIXED));
            Assert.IsNotNull(this.ec1.PFixed);
            this.ec1.SetProperty(new Property(ModelCode.ENERGYCONSUMER_PFIXEDPCT));
            Assert.IsNotNull(this.ec1.PFixedPct);
            this.ec1.SetProperty(new Property(ModelCode.ENERGYCONSUMER_QFIXED));
            Assert.IsNotNull(this.ec1.QFixed);
            this.ec1.SetProperty(new Property(ModelCode.ENERGYCONSUMER_QFIXEDPCT));
            Assert.IsNotNull(this.ec1.QFixedPct);
        }
    }
}