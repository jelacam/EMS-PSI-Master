//-----------------------------------------------------------------------
// <copyright file="EquipmentTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing Equipment
    /// </summary>
    [TestFixture]
    public class EquipmentTest
    {
        /// <summary>
        /// Instance of Equipment
        /// </summary>
        private Equipment e1;

        /// <summary>
        /// Instance of Equipment
        /// </summary>
        private Equipment e2;

        /// <summary>
        /// Instance of Equipment
        /// </summary>
        private Equipment e3;

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
            this.e1 = new Equipment(this.globalId1);
            this.e2 = new Equipment(this.globalId1);
            this.e3 = new Equipment(this.globalId2);
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "EquipmentConstructor")]
        public void Constructor()
        {
            Equipment e = new Equipment(this.globalId1);
            Assert.IsNotNull(e);
        }

        /// <summary>
        /// Unit test for Equipment Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "EquipmentEqualsMethod")]
        public void EqualsMethod()
        {
            this.resultT = this.e1.Equals(this.e2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.e1.Equals(this.e3);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for Equipment GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "EquipmentGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.e1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for Equipment HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "EquipmentHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.e1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.e1.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(this.resultT);
            this.resultF = this.e1.HasProperty(ModelCode.EQUIPMENT);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for Equipment GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "EquipmentGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.e1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.e1.Mrid);
            this.e1.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(this.e1.Measurements);
        }

        /// <summary>
        /// Unit test for Equipment SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "EquipmentSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.e1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.e1.Mrid);
        }
    }
}