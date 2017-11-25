//-----------------------------------------------------------------------
// <copyright file="ConductingEquipmentTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;
    using EMS.Services.NetworkModelService.DataModel.Meas;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing ConductingEquipment
    /// </summary>
    [TestFixture]
    public class ConductingEquipmentTest
    {
        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            ConductingEquipment ce = new ConductingEquipment(globalId);
            Assert.IsNotNull(ce);
        }

        /// <summary>
        /// Unit test for ConductingEquipment Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            ConductingEquipment ce1 = new ConductingEquipment(globalId1);
            ConductingEquipment ce2 = new ConductingEquipment(globalId1);
            ConductingEquipment ce3 = new ConductingEquipment(globalId2);
            bool resultT = ce1.Equals(ce2);
            Assert.IsTrue(resultT);
            bool resultF = ce1.Equals(ce3);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for ConductingEquipment GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            ConductingEquipment ce = new ConductingEquipment(1623);
            int result = ce.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for ConductingEquipment HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            ConductingEquipment ce = new ConductingEquipment(1623);
            bool resultT = ce.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = ce.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(resultT);
            bool resultF = ce.HasProperty(ModelCode.CONDUCTINGEQUIPMENT);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for ConductingEquipment GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            ConductingEquipment ce = new ConductingEquipment(1623);
            ce.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(ce.Mrid);
            ce.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(ce.Measurements);
        }

        /// <summary>
        /// Unit test for ConductingEquipment SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            ConductingEquipment ce = new ConductingEquipment(1623);
            ce.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(ce.Mrid);
        }
    }
}
