//-----------------------------------------------------------------------
// <copyright file="ConductingEquipmentTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing ConductingEquipment
    /// </summary>
    [TestFixture]
    public class ConductingEquipmentTest
    {
        /// <summary>
        /// Instance of ConductingEquipment
        /// </summary>
        private ConductingEquipment ce1;

        /// <summary>
        /// Instance of ConductingEquipment
        /// </summary>
        private ConductingEquipment ce2;

        /// <summary>
        /// Instance of ConductingEquipment
        /// </summary>
        private ConductingEquipment ce3;

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
            this.ce1 = new ConductingEquipment(this.globalId1);
            this.ce2 = new ConductingEquipment(this.globalId1);
            this.ce3 = new ConductingEquipment(this.globalId2);
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "ConductingEquipmentConstructor")]
        public void Constructor()
        {
            ConductingEquipment ce = new ConductingEquipment(this.globalId1);
            Assert.IsNotNull(ce);
        }

        /// <summary>
        /// Unit test for ConductingEquipment Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "ConductingEquipmentEqualsMethod")]
        public void EqualsMethod()
        {
            this.resultT = this.ce1.Equals(this.ce2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.ce1.Equals(this.ce3);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for ConductingEquipment GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "ConductingEquipmentGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.ce1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for ConductingEquipment HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "ConductingEquipmentHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.ce1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.ce1.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(this.resultT);
            this.resultF = this.ce1.HasProperty(ModelCode.CONDUCTINGEQUIPMENT);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for ConductingEquipment GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "ConductingEquipmentGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.ce1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.ce1.Mrid);
            this.ce1.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(this.ce1.Measurements);
        }

        /// <summary>
        /// Unit test for ConductingEquipment SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "ConductingEquipmentSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.ce1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.ce1.Mrid);
        }
    }
}