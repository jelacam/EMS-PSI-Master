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
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            RotatingMachine rm = new RotatingMachine(globalId);
            Assert.IsNotNull(rm);
        }

        /// <summary>
        /// Unit test for RotatingMachine RatedS setter
        /// </summary>
        /// <param name="ratedS">ratedS property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void RatedSPropertySet(float ratedS)
        {
            RotatingMachine rm = new RotatingMachine(1623);
            rm.RatedS = ratedS;
            Assert.AreEqual(rm.RatedS, ratedS);
        }

        /// <summary>
        /// Unit test for RotatingMachine Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            RotatingMachine rm1 = new RotatingMachine(globalId1);
            RotatingMachine rm2 = new RotatingMachine(globalId1);
            RotatingMachine rm3 = null;
            RotatingMachine rm4 = new RotatingMachine(globalId1);
            RotatingMachine rm5 = new RotatingMachine(globalId2);
            rm1.RatedS = 10;
            rm2.RatedS = 10;
            rm4.RatedS = 100;
            rm5.RatedS = 10;
            bool resultT = rm1.Equals(rm2);
            Assert.IsTrue(resultT);
            bool resultF = rm1.Equals(rm3);
            Assert.IsFalse(resultF);
            resultF = rm1.Equals(rm4);
            Assert.IsFalse(resultF);
            resultF = rm1.Equals(rm5);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for RotatingMachine GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            RotatingMachine rm = new RotatingMachine(1623);
            int result = rm.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for RotatingMachine HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            RotatingMachine rm = new RotatingMachine(1623);
            bool resultT = rm.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = rm.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(resultT);
            resultT = rm.HasProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsTrue(resultT);
            bool resultF = rm.HasProperty(ModelCode.ROTATINGMACHINE);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for RotatingMachine GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            RotatingMachine rm = new RotatingMachine(1623);
            rm.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(rm.Mrid);
            rm.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(rm.Measurements);
            rm.GetProperty(ModelCode.ROTATINGMACHINE_RATEDS);
            Assert.IsNotNull(rm.RatedS);
        }

        /// <summary>
        /// Unit test for RotatingMachine SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            RotatingMachine rm = new RotatingMachine(1623);
            rm.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(rm.Mrid);
            rm.SetProperty(new Property(ModelCode.ROTATINGMACHINE_RATEDS));
            Assert.IsNotNull(rm.RatedS);
        }
    }
}