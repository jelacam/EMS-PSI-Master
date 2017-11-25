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
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            EnergyConsumer ec = new EnergyConsumer(globalId);
            Assert.IsNotNull(ec);
        }

        /// <summary>
        /// Unit test for EnergyConsumer PFixed setter
        /// </summary>
        /// <param name="pFixed">pFixed property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void PFixedPropertySet(float pFixed)
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            ec.PFixed = pFixed;
            Assert.AreEqual(ec.PFixed, pFixed);
        }

        /// <summary>
        /// Unit test for EnergyConsumer PFixedPct setter
        /// </summary>
        /// <param name="pFixedPct">pFixedPct property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void PFixedPctPropertySet(float pFixedPct)
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            ec.PFixedPct = pFixedPct;
            Assert.AreEqual(ec.PFixedPct, pFixedPct);
        }

        /// <summary>
        /// Unit test for EnergyConsumer QFixed setter
        /// </summary>
        /// <param name="qFixed">qFixed property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void QFixedPropertySet(float qFixed)
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            ec.QFixed = qFixed;
            Assert.AreEqual(ec.QFixed, qFixed);
        }

        /// <summary>
        /// Unit test for EnergyConsumer QFixedPct setter
        /// </summary>
        /// <param name="qFixedPct">qFixedPct property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void QFixedPctPropertySet(float qFixedPct)
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            ec.QFixedPct = qFixedPct;
            Assert.AreEqual(ec.QFixedPct, qFixedPct);
        }

        /// <summary>
        /// Unit test for EnergyConsumer Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            EnergyConsumer ec1 = new EnergyConsumer(globalId1);
            EnergyConsumer ec2 = new EnergyConsumer(globalId1);
            EnergyConsumer ec3 = null;
            EnergyConsumer ec4 = new EnergyConsumer(globalId1);
            EnergyConsumer ec5 = new EnergyConsumer(globalId2);
            ec1.PFixed = 10;
            ec1.PFixedPct = 20;
            ec1.QFixed = 10;
            ec1.QFixedPct = 20;
            ec2.PFixed = 10;
            ec2.PFixedPct = 20;
            ec2.QFixed = 10;
            ec2.QFixedPct = 20;
            ec4.PFixed = 100;
            ec4.PFixedPct = 200;
            ec4.QFixed = 100;
            ec4.QFixedPct = 200;
            ec5.PFixed = 10;
            ec5.PFixedPct = 20;
            ec5.QFixed = 10;
            ec5.QFixedPct = 20;
            bool resultT = ec1.Equals(ec2);
            Assert.IsTrue(resultT);
            bool resultF = ec1.Equals(ec3);
            Assert.IsFalse(resultF);
            resultF = ec1.Equals(ec4);
            Assert.IsFalse(resultF);
            resultF = ec1.Equals(ec5);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for EnergyConsumer GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            int result = ec.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for EnergyConsumer HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            bool resultT = ec.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = ec.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(resultT);
            resultT = ec.HasProperty(ModelCode.ENERGYCONSUMER_PFIXED);
            Assert.IsTrue(resultT);
            resultT = ec.HasProperty(ModelCode.ENERGYCONSUMER_PFIXEDPCT);
            Assert.IsTrue(resultT);
            resultT = ec.HasProperty(ModelCode.ENERGYCONSUMER_QFIXED);
            Assert.IsTrue(resultT);
            resultT = ec.HasProperty(ModelCode.ENERGYCONSUMER_QFIXEDPCT);
            Assert.IsTrue(resultT);
            bool resultF = ec.HasProperty(ModelCode.ENERGYCONSUMER);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for EnergyConsumer GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            ec.GetProperty(ModelCode.ENERGYCONSUMER_PFIXED);
            Assert.IsNotNull(ec.PFixed);
            ec.GetProperty(ModelCode.ENERGYCONSUMER_PFIXEDPCT);
            Assert.IsNotNull(ec.PFixedPct);
            ec.GetProperty(ModelCode.ENERGYCONSUMER_QFIXED);
            Assert.IsNotNull(ec.QFixed);
            ec.GetProperty(ModelCode.ENERGYCONSUMER_QFIXEDPCT);
            Assert.IsNotNull(ec.QFixedPct);
            ec.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(ec.Measurements);
            ec.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(ec.Mrid);
        }

        /// <summary>
        /// Unit test for EnergyConsumer SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            EnergyConsumer ec = new EnergyConsumer(1623);
            ec.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(ec.Mrid);
            ec.SetProperty(new Property(ModelCode.ENERGYCONSUMER_PFIXED));
            Assert.IsNotNull(ec.PFixed);
            ec.SetProperty(new Property(ModelCode.ENERGYCONSUMER_PFIXEDPCT));
            Assert.IsNotNull(ec.PFixedPct);
            ec.SetProperty(new Property(ModelCode.ENERGYCONSUMER_QFIXED));
            Assert.IsNotNull(ec.QFixed);
            ec.SetProperty(new Property(ModelCode.ENERGYCONSUMER_QFIXEDPCT));
            Assert.IsNotNull(ec.QFixedPct);
        }
    }
}