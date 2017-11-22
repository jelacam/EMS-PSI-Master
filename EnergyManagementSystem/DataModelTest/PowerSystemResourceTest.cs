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
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            PowerSystemResource psr = new PowerSystemResource(globalId);
            Assert.IsNotNull(psr);
        }

        /// <summary>
        /// Unit test for PowerSystemResource Measurements setter
        /// </summary>
        [Test]
        public void MeasurementsPropertySet()
        {
            PowerSystemResource psr = new PowerSystemResource(1623);
            long m1 = 1;
            long m2 = 2;
            List<long> l = new List<long>();
            l.Add(m1);
            l.Add(m2);
            psr.Measurements = l;
            Assert.IsNotNull(psr.Measurements);
        }

        /// <summary>
        /// Unit test for PowerSystemResource Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        [Test]
        [TestCase(1623, 10)]
        public void EqualsMethod(long globalId1, long globalId2)
        {
            PowerSystemResource psr1 = new PowerSystemResource(globalId1);
            PowerSystemResource psr2 = new PowerSystemResource(globalId1);
            PowerSystemResource psr3 = new PowerSystemResource(globalId2);
            PowerSystemResource psr4 = new PowerSystemResource(globalId2);
            PowerSystemResource psr5 = null;
            long m1 = 1;
            long m2 = 2;
            List<long> l = new List<long>();
            l.Add(m1);
            l.Add(m2);
            psr1.Measurements = l;
            psr2.Measurements = l;
            psr3.Measurements = null;
            psr4.Measurements = null;
            bool resultT = psr1.Equals(psr2);
            Assert.IsTrue(resultT);
            bool resultF = psr1.Equals(psr3);
            Assert.IsFalse(resultF);
            resultT = psr3.Equals(psr4);
            Assert.IsTrue(resultT);
            resultF = psr1.Equals(psr5);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for PowerSystemResource GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            PowerSystemResource psr = new PowerSystemResource(1623);
            int result = psr.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for PowerSystemResource HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            PowerSystemResource psr = new PowerSystemResource(1623);
            bool resultT = psr.HasProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsTrue(resultT);
            bool resultF = psr.HasProperty(ModelCode.POWERSYSTEMRESOURCE);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for PowerSystemResource GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            PowerSystemResource psr = new PowerSystemResource(1623);
            psr.GetProperty(ModelCode.POWERSYSTEMRESOURCE_MEASUREMENTS);
            Assert.IsNotNull(psr.Measurements);
            psr.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsNotNull(psr.Mrid);
        }

        /// <summary>
        /// Unit test for PowerSystemResource SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            PowerSystemResource psr = new PowerSystemResource(1623);
            psr.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            Assert.IsNotNull(psr.Mrid);
        }

        /// <summary>
        /// Unit test for PowerSystemResource IsReferenced
        /// </summary>
        [Test]
        public void IsReferenced()
        {
            PowerSystemResource psr = new PowerSystemResource(1623);
            bool result = psr.IsReferenced;
            Assert.IsFalse(result);
            psr.Measurements.Add(1);
            result = psr.IsReferenced;
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Unit test for PowerSystemResource GetReferences method
        /// </summary>
        [Test]
        public void GetReferencesMethod()
        {
            PowerSystemResource psr = new PowerSystemResource(1623);
            long m1 = 1;
            long m2 = 2;
            List<long> l = new List<long>();
            l.Add(m1);
            l.Add(m2);
            psr.Measurements = l;
            Dictionary<ModelCode, List<long>> d = new Dictionary<ModelCode, List<long>>();
            psr.GetReferences(d, TypeOfReference.Both);
        }
    }
}