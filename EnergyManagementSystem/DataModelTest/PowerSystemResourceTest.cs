//-----------------------------------------------------------------------
// <copyright file="PowerSystemResourceTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing PowerSystemResource
    /// </summary>
    [TestFixture]
    public class PowerSystemResourceTest
    {
        /// <summary>
        /// Instance of PowerSystemResource
        /// </summary>
        private PowerSystemResource psr1;

        /// <summary>
        /// Instance of PowerSystemResource
        /// </summary>
        private PowerSystemResource psr2;

        /// <summary>
        /// Instance of PowerSystemResource
        /// </summary>
        private PowerSystemResource psr3;

        /// <summary>
        /// Instance of PowerSystemResource
        /// </summary>
        private PowerSystemResource psr4;

        /// <summary>
        /// Instance of PowerSystemResource
        /// </summary>
        private PowerSystemResource psr5;

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
            this.psr1 = new PowerSystemResource(this.globalId1);
            this.psr2 = new PowerSystemResource(this.globalId1);
            this.psr3 = new PowerSystemResource(this.globalId2);
            this.psr4 = new PowerSystemResource(this.globalId2);
            this.psr5 = null;
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceConstructor")]
        public void Constructor()
        {
            PowerSystemResource psr = new PowerSystemResource(this.globalId1);
            Assert.IsNotNull(psr);
        }

        /// <summary>
        /// Unit test for PowerSystemResource Measurements setter
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceMeasurementsProperty")]
        public void MeasurementsProperty()
        {
            long m1 = 1;
            long m2 = 2;
            List<long> l = new List<long>();
            l.Add(m1);
            l.Add(m2);
            this.psr1.Measurements = l;
            Assert.IsNotNull(this.psr1.Measurements);
        }

        /// <summary>
        /// Unit test for PowerSystemResource Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceEqualsMethod")]
        public void EqualsMethod()
        {
            long m1 = 1;
            long m2 = 2;
            List<long> l = new List<long>();
            l.Add(m1);
            l.Add(m2);
            this.psr1.Measurements = l;
            this.psr2.Measurements = l;
            this.psr3.Measurements = null;
            this.psr4.Measurements = null;
            this.resultT = this.psr1.Equals(this.psr2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.psr1.Equals(this.psr3);
            Assert.IsFalse(this.resultF);
            this.resultT = this.psr3.Equals(this.psr4);
            Assert.IsTrue(this.resultT);
            this.resultF = this.psr1.Equals(this.psr5);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for PowerSystemResource GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.psr1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for PowerSystemResource HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.psr1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.psr1.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(this.resultT);
            this.resultF = this.psr1.HasProperty(ModelCode.POWERSYSTEMRESOURCE);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for PowerSystemResource GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.psr1.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(this.psr1.Measurements);
            this.psr1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(this.psr1.Mrid);
        }

        /// <summary>
        /// Unit test for PowerSystemResource SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.psr1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(this.psr1.Mrid);
        }

        /// <summary>
        /// Unit test for PowerSystemResource IsReferenced
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceIsReferencedMethod")]
        public void IsReferencedMethod()
        {
            this.psr1.Measurements.RemoveRange(0, this.psr1.Measurements.Count);
            this.resultF = this.psr1.IsReferenced;
            Assert.IsFalse(this.resultF);
            this.psr1.Measurements.Add(1);
            this.resultT = this.psr1.IsReferenced;
            Assert.IsTrue(this.resultT);
        }

        /// <summary>
        /// Unit test for PowerSystemResource GetReferences method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceGetReferencesMethod")]
        public void GetReferencesMethod()
        {
            long m1 = 1;
            long m2 = 2;
            List<long> l = new List<long>();
            l.Add(m1);
            l.Add(m2);
            this.psr1.Measurements = l;
            Dictionary<ModelCode, List<long>> d = new Dictionary<ModelCode, List<long>>();
            this.psr1.GetReferences(d, TypeOfReference.Target);
            this.psr1.GetReferences(d, TypeOfReference.Both);
        }

        /// <summary>
        /// Unit test for PowerSystemResource AddReference method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceAddReferenceMethod")]
        public void AddReferenceMethod()
        {
            this.psr1.AddReference(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE, 1);
            Assert.IsNotNull(this.psr1.Measurements);
            Assert.Throws<Exception>(() => this.psr1.AddReference(ModelCode.POWERSYSTEMRESOURCE, 1));
        }

        /// <summary>
        /// Unit test for PowerSystemResource RemoveReference method
        /// </summary>
        [Test]
        [TestCase(TestName = "PowerSystemResourceRemoveReferenceMethod")]
        public void RemoveReferenceMethod()
        {
            this.psr1.RemoveReference(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE, 1);
            this.psr1.AddReference(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE, 2);
            this.psr1.RemoveReference(ModelCode.MEASUREMENT_POWERSYSTEMRESOURCE, 2);
            Assert.Throws<ModelException>(() => this.psr1.RemoveReference(ModelCode.POWERSYSTEMRESOURCE, 1));
        }
    }
}