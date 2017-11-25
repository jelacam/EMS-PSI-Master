//-----------------------------------------------------------------------
// <copyright file="AnalogTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Meas;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing Analog
    /// </summary>
    [TestFixture]
    public class AnalogTest
    {
        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            Analog a = new Analog(globalId);
            Assert.IsNotNull(a);
        }

        /// <summary>
        /// Unit test for Analog MaxValue setter
        /// </summary>
        /// <param name="maxValue">maxValue property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void MaxValuePropertySet(float maxValue)
        {
            Analog a = new Analog(1623);
            a.MaxValue = maxValue;
            Assert.AreEqual(a.MaxValue, maxValue);
        }

        /// <summary>
        /// Unit test for Analog MinValue setter
        /// </summary>
        /// <param name="minValue">minValue property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void MinValuePropertySet(float minValue)
        {
            Analog a = new Analog(1623);
            a.MinValue = minValue;
            Assert.AreEqual(a.MinValue, minValue);
        }

        /// <summary>
        /// Unit test for Analog NormalValue setter
        /// </summary>
        /// <param name="normalValue">normalValue property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void NormalValuePropertySet(float normalValue)
        {
            Analog a = new Analog(1623);
            a.NormalValue = normalValue;
            Assert.AreEqual(a.NormalValue, normalValue);
        }

        /// <summary>
        /// Unit test for Analog SignalDirection setter
        /// </summary>
        /// <param name="signalDirection">signalDirection property being set and asserted</param>
        [Test]
        [TestCase(SignalDirection.Read)]
        public void SignalDirectionPropertySet(SignalDirection signalDirection)
        {
            Analog a = new Analog(1623);
            a.SignalDirection = signalDirection;
            Assert.AreEqual(a.SignalDirection, signalDirection);
            Assert.IsNotNull(a.SignalDirection);
        }

        /// <summary>
        /// Unit test for Analog Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            Analog a1 = new Analog(globalId1);
            Analog a2 = new Analog(globalId1);
            Analog a3 = null;
            Analog a4 = new Analog(globalId1);
            Analog a5 = new Analog(globalId2);

            a1.MaxValue = 10;
            a1.MinValue = 1;
            a1.NormalValue = 5;
            a1.SignalDirection = SignalDirection.Read;
            a2.MaxValue = 10;
            a2.MinValue = 1;
            a2.NormalValue = 5;
            a2.SignalDirection = SignalDirection.Read;
            a4.MaxValue = 100;
            a4.MinValue = 10;
            a4.NormalValue = 50;
            a4.SignalDirection = SignalDirection.Write;
            a5.MaxValue = 10;
            a5.MinValue = 1;
            a5.NormalValue = 5;
            a5.SignalDirection = SignalDirection.Read;
            bool resultT = a1.Equals(a2);
            Assert.IsTrue(resultT);
            bool resultF = a1.Equals(a3);
            Assert.IsFalse(resultF);
            resultF = a1.Equals(a4);
            Assert.IsFalse(resultF);
            resultF = a1.Equals(a5);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for Analog GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            Analog a = new Analog(1623);
            int result = a.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for Analog HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            Analog a = new Analog(1623);
            bool resultT = a.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = a.HasProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsTrue(resultT);
            resultT = a.HasProperty(ModelCode.ANALOG_MAXVALUE);
            Assert.IsTrue(resultT);
            resultT = a.HasProperty(ModelCode.ANALOG_MINVALUE);
            Assert.IsTrue(resultT);
            resultT = a.HasProperty(ModelCode.ANALOG_NORMALVALUE);
            Assert.IsTrue(resultT);
            resultT = a.HasProperty(ModelCode.ANALOG_SIGNALDIRECTION);
            Assert.IsTrue(resultT);
            bool resultF = a.HasProperty(ModelCode.ANALOG);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for Analog GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            Analog a = new Analog(1623);
            a.GetProperty(ModelCode.ANALOG_MAXVALUE);
            Assert.IsNotNull(a.MaxValue);
            a.GetProperty(ModelCode.ANALOG_MINVALUE);
            Assert.IsNotNull(a.MinValue);
            a.GetProperty(ModelCode.ANALOG_NORMALVALUE);
            Assert.IsNotNull(a.NormalValue);
            a.GetProperty(ModelCode.ANALOG_SIGNALDIRECTION);
            Assert.IsNotNull(a.SignalDirection);
            a.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsNotNull(a.MeasurementType);
            a.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(a.Mrid);
        }

        /// <summary>
        /// Unit test for Analog SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            Analog a = new Analog(1623);
            a.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(a.Mrid);
            a.SetProperty(new Property(ModelCode.MEASUREMENT_MEASUREMENTTYPE));
            Assert.IsNotNull(a.MeasurementType);
            a.SetProperty(new Property(ModelCode.ANALOG_MAXVALUE));
            Assert.IsNotNull(a.MaxValue);
            a.SetProperty(new Property(ModelCode.ANALOG_MINVALUE));
            Assert.IsNotNull(a.MinValue);
            a.SetProperty(new Property(ModelCode.ANALOG_NORMALVALUE));
            Assert.IsNotNull(a.NormalValue);
            a.SetProperty(new Property(ModelCode.ANALOG_SIGNALDIRECTION));
            Assert.IsNotNull(a.SignalDirection);
        }
    }
}