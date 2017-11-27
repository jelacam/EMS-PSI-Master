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
        /// Instance of Analog
        /// </summary>
        private Analog a1;

        /// <summary>
        /// Instance of Analog
        /// </summary>
        private Analog a2;

        /// <summary>
        /// Instance of Analog
        /// </summary>
        private Analog a3;

        /// <summary>
        /// Instance of Analog
        /// </summary>
        private Analog a4;

        /// <summary>
        /// Instance of Analog
        /// </summary>
        private Analog a5;

        /// <summary>
        /// Container for maxValue
        /// </summary>
        private float maxValue1;

        /// <summary>
        /// Container for maxValue
        /// </summary>
        private float maxValue2;

        /// <summary>
        /// Container for minValue
        /// </summary>
        private float minValue1;

        /// <summary>
        /// Container for minValue
        /// </summary>
        private float minValue2;

        /// <summary>
        /// Container for normalValue
        /// </summary>
        private float normalValue1;

        /// <summary>
        /// Container for normalValue
        /// </summary>
        private float normalValue2;

        /// <summary>
        /// Container for signalDirection
        /// </summary>
        private SignalDirection signalDirection1;

        /// <summary>
        /// Container for signalDirection
        /// </summary>
        private SignalDirection signalDirection2;

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
            this.maxValue1 = 1000;
            this.maxValue2 = 100;
            this.minValue1 = 10;
            this.minValue2 = 1;
            this.normalValue1 = 500;
            this.normalValue2 = 50;
            this.signalDirection1 = SignalDirection.Read;
            this.signalDirection2 = SignalDirection.Write;
            this.a1 = new Analog(this.globalId1);
            this.a2 = new Analog(this.globalId1);
            this.a2.MaxValue = this.maxValue1;
            this.a2.MinValue = this.minValue1;
            this.a2.NormalValue = this.normalValue1;
            this.a2.SignalDirection = this.signalDirection1;
            this.a3 = new Analog(this.globalId1);
            this.a3.MaxValue = this.maxValue2;
            this.a3.MinValue = this.minValue2;
            this.a3.NormalValue = this.normalValue2;
            this.a3.SignalDirection = this.signalDirection2;
            this.a4 = new Analog(this.globalId2);
            this.a4.MaxValue = this.maxValue1;
            this.a4.MinValue = this.minValue1;
            this.a4.NormalValue = this.normalValue1;
            this.a4.SignalDirection = this.signalDirection1;
            this.a5 = null;
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogConstructor")]
        public void Constructor()
        {
            Analog a = new Analog(this.globalId1);
            Assert.IsNotNull(a);
        }

        /// <summary>
        /// Unit test for Analog MaxValue setter
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogMaxValueProperty")]
        public void MaxValueProperty()
        {
            this.a1.MaxValue = this.maxValue1;
            Assert.AreEqual(this.a1.MaxValue, this.maxValue1);
        }

        /// <summary>
        /// Unit test for Analog MinValue setter
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogMinValueProperty")]
        public void MinValueProperty()
        {
            this.a1.MinValue = this.minValue1;
            Assert.AreEqual(this.a1.MinValue, this.minValue1);
        }

        /// <summary>
        /// Unit test for Analog NormalValue setter
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogNormalValueProperty")]
        public void NormalValueProperty()
        {
            this.a1.NormalValue = this.normalValue1;
            Assert.AreEqual(this.a1.NormalValue, this.normalValue1);
        }

        /// <summary>
        /// Unit test for Analog SignalDirection setter
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogSignalDirectionProperty")]
        public void SignalDirectionProperty()
        {
            this.a1.SignalDirection = this.signalDirection1;
            Assert.AreEqual(this.a1.SignalDirection, this.signalDirection1);
            Assert.IsNotNull(this.a1.SignalDirection);
        }

        /// <summary>
        /// Unit test for Analog Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogEqualsMethod")]
        public void EqualsMethod()
        {
            this.a1.MaxValue = this.maxValue1;
            this.a1.MinValue = this.minValue1;
            this.a1.NormalValue = this.normalValue1;
            this.a1.SignalDirection = this.signalDirection1;
            this.resultT = this.a1.Equals(this.a2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.a1.Equals(this.a3);
            Assert.IsFalse(this.resultF);
            this.resultF = this.a1.Equals(this.a4);
            Assert.IsFalse(this.resultF);
            this.resultF = this.a1.Equals(this.a5);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for Analog GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.a1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for Analog HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.a1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.a1.HasProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsTrue(this.resultT);
            this.resultT = this.a1.HasProperty(ModelCode.ANALOG_MAXVALUE);
            Assert.IsTrue(this.resultT);
            this.resultT = this.a1.HasProperty(ModelCode.ANALOG_MINVALUE);
            Assert.IsTrue(this.resultT);
            this.resultT = this.a1.HasProperty(ModelCode.ANALOG_NORMALVALUE);
            Assert.IsTrue(this.resultT);
            this.resultT = this.a1.HasProperty(ModelCode.ANALOG_SIGNALDIRECTION);
            Assert.IsTrue(this.resultT);
            this.resultF = this.a1.HasProperty(ModelCode.ANALOG);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for Analog GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.a1.GetProperty(ModelCode.ANALOG_MAXVALUE);
            Assert.IsNotNull(this.a1.MaxValue);
            this.a1.GetProperty(ModelCode.ANALOG_MINVALUE);
            Assert.IsNotNull(this.a1.MinValue);
            this.a1.GetProperty(ModelCode.ANALOG_NORMALVALUE);
            Assert.IsNotNull(this.a1.NormalValue);
            this.a1.GetProperty(ModelCode.ANALOG_SIGNALDIRECTION);
            Assert.IsNotNull(this.a1.SignalDirection);
            this.a1.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsNotNull(this.a1.MeasurementType);
            this.a1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.a1.Mrid);
        }

        /// <summary>
        /// Unit test for Analog SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "AnalogSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.a1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.a1.Mrid);
            this.a1.SetProperty(new Property(ModelCode.MEASUREMENT_MEASUREMENTTYPE));
            Assert.IsNotNull(this.a1.MeasurementType);
            this.a1.SetProperty(new Property(ModelCode.ANALOG_MAXVALUE));
            Assert.IsNotNull(this.a1.MaxValue);
            this.a1.SetProperty(new Property(ModelCode.ANALOG_MINVALUE));
            Assert.IsNotNull(this.a1.MinValue);
            this.a1.SetProperty(new Property(ModelCode.ANALOG_NORMALVALUE));
            Assert.IsNotNull(this.a1.NormalValue);
            this.a1.SetProperty(new Property(ModelCode.ANALOG_SIGNALDIRECTION));
            Assert.IsNotNull(this.a1.SignalDirection);
        }
    }
}