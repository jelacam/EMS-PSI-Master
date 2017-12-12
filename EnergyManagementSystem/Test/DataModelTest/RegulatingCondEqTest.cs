//-----------------------------------------------------------------------
// <copyright file="RegulatingCondEqTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Wires;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing RegulatingCondEq
    /// </summary>
    [TestFixture]
    public class RegulatingCondEqTest
    {
        /// <summary>
        /// Instance of RegulatingCondEq
        /// </summary>
        private RegulatingCondEq rce1;

        /// <summary>
        /// Instance of RegulatingCondEq
        /// </summary>
        private RegulatingCondEq rce2;

        /// <summary>
        /// Instance of RegulatingCondEq
        /// </summary>
        private RegulatingCondEq rce3;

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
            this.rce1 = new RegulatingCondEq(this.globalId1);
            this.rce2 = new RegulatingCondEq(this.globalId1);
            this.rce3 = new RegulatingCondEq(this.globalId2);
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "EquipmentConstructor")]
        public void Constructor()
        {
            RegulatingCondEq rce = new RegulatingCondEq(this.globalId1);
            Assert.IsNotNull(rce);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "RegulatingCondEqEqualsMethod")]
        public void EqualsMethod()
        {
            this.resultT = this.rce1.Equals(this.rce2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.rce1.Equals(this.rce3);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "RegulatingCondEqGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.rce1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "RegulatingCondEqHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.rce1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.rce1.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(this.resultT);
            this.resultF = this.rce1.HasProperty(ModelCode.REGULATINGCONDEQ);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "RegulatingCondEqGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.rce1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.rce1.Mrid);
            this.rce1.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(this.rce1.Measurements);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "RegulatingCondEqSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.rce1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.rce1.Mrid);
        }
    }
}