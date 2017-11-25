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
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            Measurement m = new Measurement(globalId);
            Assert.IsNotNull(m);
        }

        /// <summary>
        /// Unit test for Measurement MeasurementType setter
        /// </summary>
        /// <param name="measurementType">measurementType property being set and asserted</param>
        [Test]
        [TestCase("measurementType")]
        public void MeasurementTypePropertySet(string measurementType)
        {
            Measurement m = new Measurement(1623);
            m.MeasurementType = measurementType;
            Assert.AreEqual(m.MeasurementType, measurementType);
            Assert.IsNotEmpty(m.MeasurementType);
            Assert.IsNotNull(m.MeasurementType);
        }

        /// <summary>
        /// Unit test for Measurement UnitSymbol setter
        /// </summary>
        /// <param name="unitSymbol">unitSymbol property being set and asserted</param>
        [Test]
        [TestCase(UnitSymbol.A)]
        public void UnitSymbolPropertySet(UnitSymbol unitSymbol)
        {
            Measurement m = new Measurement(1623);
            m.UnitSymbol = unitSymbol;
            Assert.AreEqual(m.UnitSymbol, unitSymbol);
            Assert.IsNotNull(m.UnitSymbol);
        }

        /// <summary>
        /// Unit test for Measurement PowerSystemResource setter
        /// </summary>
        [Test]
        public void PowerSystemResourcePropertySet()
        {
            Measurement m = new Measurement(1623);
            PowerSystemResource psr = new PowerSystemResource(1);
            m.PowerSystemResource = 1;
            Assert.IsNotNull(m.PowerSystemResource);
        }

        /// <summary>
        /// Unit test for Measurement Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            Measurement m1 = new Measurement(globalId1);
            Measurement m2 = new Measurement(globalId1);
            Measurement m3 = null;
            Measurement m4 = new Measurement(globalId1);
            Measurement m5 = new Measurement(globalId2);
            PowerSystemResource psr1 = new PowerSystemResource(1);
            PowerSystemResource psr2 = new PowerSystemResource(2);
            m1.MeasurementType = "measurementType1";
            m1.UnitSymbol = UnitSymbol.A;
            m1.PowerSystemResource = 1;
            m2.MeasurementType = "measurementType1";
            m2.UnitSymbol = UnitSymbol.A;
            m2.PowerSystemResource = 1;
            m4.MeasurementType = "measurementType2";
            m4.UnitSymbol = UnitSymbol.deg;
            m4.PowerSystemResource = 2;
            m5.MeasurementType = "measurementType2";
            m5.UnitSymbol = UnitSymbol.deg;
            m5.PowerSystemResource = 2;
            bool resultT = m1.Equals(m2);
            Assert.IsTrue(resultT);
            bool resultF = m1.Equals(m3);
            Assert.IsFalse(resultF);
            resultF = m1.Equals(m4);
            Assert.IsFalse(resultF);
            resultF = m1.Equals(m5);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for Measurement GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            Measurement m = new Measurement(1623);
            int result = m.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for Measurement HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            Measurement m = new Measurement(1623);
            bool resultT = m.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = m.HasProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsTrue(resultT);
            resultT = m.HasProperty(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE);
            Assert.IsTrue(resultT);
            resultT = m.HasProperty(ModelCode.MEASUREMENT_UNITSYMBOL);
            Assert.IsTrue(resultT);
            bool resultF = m.HasProperty(ModelCode.MEASUREMENT);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for Measurement GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            Measurement m = new Measurement(1623);
            m.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTTYPE);
            Assert.IsNotNull(m.MeasurementType);
            m.GetProperty(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE);
            Assert.IsNotNull(m.PowerSystemResource);
            m.GetProperty(ModelCode.MEASUREMENT_UNITSYMBOL);
            Assert.IsNotNull(m.UnitSymbol);
            m.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(m.Mrid);
        }

        /// <summary>
        /// Unit test for Measurement SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            Measurement m = new Measurement(1623);
            m.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(m.Mrid);
            m.SetProperty(new Property(ModelCode.MEASUREMENT_MEASUREMENTTYPE));
            Assert.IsNotNull(m.MeasurementType);
            m.SetProperty(new Property(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE));
            Assert.IsNotNull(m.PowerSystemResource);
            m.SetProperty(new Property(ModelCode.MEASUREMENT_UNITSYMBOL));
            Assert.IsNotNull(m.UnitSymbol);
        }

        /// <summary>
        /// Unit test for Measurement GetReferences method
        /// </summary>
        [Test]
        public void GetReferencesPropertyMethod()
        {
            Measurement m = new Measurement(1623);
            List<long> l = new List<long>();
            l.Add(10);
            Dictionary<ModelCode, List<long>> d = new Dictionary<ModelCode, List<long>>();
            d.Add(ModelCode.POWERSYSTEMRESOURCE, l);
            m.GetReferences(d, TypeOfReference.Both);
            PowerSystemResource psr = new PowerSystemResource(10);
            m.PowerSystemResource = 10;
            m.GetReferences(d, TypeOfReference.Reference);
        }
    }
}