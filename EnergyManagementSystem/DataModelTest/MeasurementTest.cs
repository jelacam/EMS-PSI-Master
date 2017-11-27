//-----------------------------------------------------------------------
// <copyright file="MeasurementTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using System.Collections.Generic;
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;
    using EMS.Services.NetworkModelService.DataModel.Meas;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing Measurement
    /// </summary>
    [TestFixture]
    public class MeasurementTest
    {
        /// <summary>
        /// Instance of Measurement
        /// </summary>
        private Measurement m1;

        /// <summary>
        /// Instance of Measurement
        /// </summary>
        private Measurement m2;

        /// <summary>
        /// Instance of Measurement
        /// </summary>
        private Measurement m3;

        /// <summary>
        /// Instance of Measurement
        /// </summary>
        private Measurement m4;

        /// <summary>
        /// Instance of Measurement
        /// </summary>
        private Measurement m5;

        /// <summary>
        /// Container for measurementType
        /// </summary>
        private string measurementType1;

        /// <summary>
        /// Container for measurementType
        /// </summary>
        private string measurementType2;

        /// <summary>
        /// Container for unitSymbol
        /// </summary>
        private UnitSymbol unitSymbol1;

        /// <summary>
        /// Container for unitSymbol
        /// </summary>
        private UnitSymbol unitSymbol2;

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
            this.measurementType1 = "measurementType1";
            this.measurementType2 = "measurementType2";
            this.unitSymbol1 = UnitSymbol.A;
            this.unitSymbol2 = UnitSymbol.deg;
            this.m1 = new Measurement(this.globalId1);
            this.m2 = new Measurement(this.globalId1);
            this.m2.MeasurementType = this.measurementType1;
            this.m2.UnitSymbol = this.unitSymbol1;
            this.m2.PowerSystemResource = 1;
            this.m3 = new Measurement(this.globalId1);
            this.m3.MeasurementType = this.measurementType2;
            this.m3.UnitSymbol = this.unitSymbol2;
            this.m4 = new Measurement(this.globalId2);
            this.m4.MeasurementType = this.measurementType2;
            this.m4.UnitSymbol = this.unitSymbol2;
            this.m4.PowerSystemResource = 2;
            this.m5 = null;
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementConstructor")]
        public void Constructor()
        {
            Measurement m = new Measurement(this.globalId1);
            Assert.IsNotNull(m);
        }

        /// <summary>
        /// Unit test for Measurement MeasurementType setter
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementMeasurementTypeProperty")]
        public void MeasurementTypeProperty()
        {
            this.m1.MeasurementType = this.measurementType1;
            Assert.AreEqual(this.m1.MeasurementType, this.measurementType1);
            Assert.IsNotEmpty(this.m1.MeasurementType);
            Assert.IsNotNull(this.m1.MeasurementType);
        }

        /// <summary>
        /// Unit test for Measurement UnitSymbol setter
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementUnitSymbolProperty")]
        public void UnitSymbolProperty()
        {
            this.m1.UnitSymbol = this.unitSymbol1;
            Assert.AreEqual(this.m1.UnitSymbol, this.unitSymbol1);
            Assert.IsNotNull(this.m1.UnitSymbol);
        }

        /// <summary>
        /// Unit test for Measurement PowerSystemResource setter
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementPowerSystemResourceProperty")]
        public void PowerSystemResourceProperty()
        {
            PowerSystemResource psr = new PowerSystemResource(1);
            this.m1.PowerSystemResource = 1;
            Assert.IsNotNull(this.m1.PowerSystemResource);
        }

        /// <summary>
        /// Unit test for Measurement Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementEqualsMethod")]
        public void EqualsMethod()
        {
            this.m1.MeasurementType = this.measurementType1;
            this.m1.UnitSymbol = this.unitSymbol1;
            this.m1.PowerSystemResource = 1;
            this.resultT = this.m1.Equals(this.m2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.m1.Equals(this.m3);
            Assert.IsFalse(this.resultF);
            this.resultF = this.m1.Equals(this.m4);
            Assert.IsFalse(this.resultF);
            this.resultF = this.m1.Equals(this.m5);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for Measurement GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.m1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for Measurement HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.m1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.m1.HasProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsTrue(this.resultT);
            this.resultT = this.m1.HasProperty(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE);
            Assert.IsTrue(this.resultT);
            this.resultT = this.m1.HasProperty(ModelCode.MEASUREMENT_UNITSYMBOL);
            Assert.IsTrue(this.resultT);
            this.resultF = this.m1.HasProperty(ModelCode.MEASUREMENT);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for Measurement GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.m1.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsNotNull(this.m1.MeasurementType);
            this.m1.GetProperty(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE);
            Assert.IsNotNull(this.m1.PowerSystemResource);
            this.m1.GetProperty(ModelCode.MEASUREMENT_UNITSYMBOL);
            Assert.IsNotNull(this.m1.UnitSymbol);
            this.m1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.m1.Mrid);
        }

        /// <summary>
        /// Unit test for Measurement SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.m1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.m1.Mrid);
            this.m1.SetProperty(new Property(ModelCode.MEASUREMENT_MEASUREMENTTYPE));
            Assert.IsNotNull(this.m1.MeasurementType);
            this.m1.SetProperty(new Property(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE));
            Assert.IsNotNull(this.m1.PowerSystemResource);
            this.m1.SetProperty(new Property(ModelCode.MEASUREMENT_UNITSYMBOL));
            Assert.IsNotNull(this.m1.UnitSymbol);
        }

        /// <summary>
        /// Unit test for Measurement GetReferences method
        /// </summary>
        [Test]
        [TestCase(TestName = "MeasurementGetReferencesMethod")]
        public void GetReferencesMethod()
        {
            List<long> l = new List<long>();
            l.Add(10);
            Dictionary<ModelCode, List<long>> d = new Dictionary<ModelCode, List<long>>();
            d.Add(ModelCode.POWERSYSTEMRESOURCE, l);
            this.m1 = new Measurement(this.globalId1);
            this.m1.GetReferences(d, TypeOfReference.Both);
            this.m1.PowerSystemResource = 100;
            this.m1.GetReferences(d, TypeOfReference.Reference);
            this.m1.GetReferences(d, TypeOfReference.Both);
        }
    }
}