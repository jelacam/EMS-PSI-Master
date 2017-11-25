//-----------------------------------------------------------------------
// <copyright file="EquipmentTest.cs" company="EMS-Team">
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
    /// Class for unit testing Equipment
    /// </summary>
    [TestFixture]
    public class EquipmentTest
    {
        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            Equipment e = new Equipment(globalId);
            Assert.IsNotNull(e);
        }

        /// <summary>
        /// Unit test for Equipment Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            Equipment e1 = new Equipment(globalId1);
            Equipment e2 = new Equipment(globalId1);
            Equipment e3 = new Equipment(globalId2);
            bool resultT = e1.Equals(e2);
            Assert.IsTrue(resultT);
            bool resultF = e1.Equals(e3);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for Equipment GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            Equipment e = new Equipment(1623);
            int result = e.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for Equipment HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            Equipment e = new Equipment(1623);
            bool resultT = e.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = e.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(resultT);
            bool resultF = e.HasProperty(ModelCode.EQUIPMENT);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for Equipment GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            Equipment e = new Equipment(1623);
            e.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(e.Mrid);
            e.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(e.Measurements);
        }

        /// <summary>
        /// Unit test for Equipment SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            Equipment e = new Equipment(1623);
            e.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(e.Mrid);
        }
    }
}
