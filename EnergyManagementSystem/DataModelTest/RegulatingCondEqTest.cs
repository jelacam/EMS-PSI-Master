//-----------------------------------------------------------------------
// <copyright file="RegulatingCondEqTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;
    using EMS.Services.NetworkModelService.DataModel.Meas;
    using EMS.Services.NetworkModelService.DataModel.Wires;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing RegulatingCondEq
    /// </summary>
    [TestFixture]
    public class RegulatingCondEqTest
    {
        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            RegulatingCondEq rce = new RegulatingCondEq(globalId);
            Assert.IsNotNull(rce);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            RegulatingCondEq rce1 = new RegulatingCondEq(globalId1);
            RegulatingCondEq rce2 = new RegulatingCondEq(globalId1);
            RegulatingCondEq rce3 = new RegulatingCondEq(globalId2);
            bool resultT = rce1.Equals(rce2);
            Assert.IsTrue(resultT);
            bool resultF = rce1.Equals(rce3);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            RegulatingCondEq rce = new RegulatingCondEq(1623);
            int result = rce.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            RegulatingCondEq rce = new RegulatingCondEq(1623);
            bool resultT = rce.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = rce.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(resultT);
            bool resultF = rce.HasProperty(ModelCode.REGULATINGCONDEQ);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            RegulatingCondEq rce = new RegulatingCondEq(1623);
            rce.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(rce.Mrid);
            rce.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(rce.Measurements);
        }

        /// <summary>
        /// Unit test for RegulatingCondEq SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            RegulatingCondEq rce = new RegulatingCondEq(1623);
            rce.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(rce.Mrid);
        }
    }
}