//-----------------------------------------------------------------------
// <copyright file="IdentifiedObjectTest.cs" company="EMS-Team">
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
    /// Class for unit testing IdentifiedObject
    /// </summary>
    [TestFixture]
    public class IdentifiedObjectTest
    {
        /// <summary>
        /// Instance of IdentifiedObject
        /// </summary>
        private IdentifiedObject io1;

        /// <summary>
        /// Instance of IdentifiedObject
        /// </summary>
        private IdentifiedObject io2;

        /// <summary>
        /// Instance of IdentifiedObject
        /// </summary>
        private IdentifiedObject io3;

        /// <summary>
        /// Instance of IdentifiedObject
        /// </summary>
        private IdentifiedObject io4;

        /// <summary>
        /// Container for globalId
        /// </summary>
        private long globalId1;

        /// <summary>
        /// Container for globalId
        /// </summary>
        private long globalId2;

        /// <summary>
        /// Container for name
        /// </summary>
        private string name1;

        /// <summary>
        /// Container for name
        /// </summary>
        private string name2;

        /// <summary>
        /// Container for mrid
        /// </summary>
        private string mrid1;

        /// <summary>
        /// Container for mrid
        /// </summary>
        private string mrid2;

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
            this.name1 = "name1";
            this.name2 = "name2";
            this.mrid1 = "mrid1";
            this.mrid2 = "mrid2";
            this.io1 = new IdentifiedObject(this.globalId1);
            this.io2 = new IdentifiedObject(this.globalId1);
            this.io2.Name = this.name1;
            this.io2.Mrid = this.mrid1;
            this.io3 = new IdentifiedObject(this.globalId2);
            this.io3.Name = this.name2;
            this.io3.Mrid = this.mrid2;
            this.io4 = null;
        }

        /// <summary>
        /// Unit test for constructor with parameters
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectConstructor")]
        public void Constructor()
        {
            IdentifiedObject io = new IdentifiedObject(this.globalId1);
            Assert.IsNotNull(io);
        }

        /// <summary>
        /// Unit test for IdentifiedObject GlobalId setter
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectGlobalIdProperty")]
        public void GlobalIdProperty()
        {
            IdentifiedObject io = new IdentifiedObject(this.globalId1);
            Assert.AreEqual(io.GlobalId, this.globalId1);
        }

        /// <summary>
        /// Unit test for IdentifiedObject Name setter
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectNameProperty")]
        public void NameProperty()
        {
            this.io1.Name = this.name1;
            Assert.AreEqual(this.io1.Name, this.name1);
            Assert.IsNotEmpty(this.io1.Name);
            Assert.IsNotNull(this.io1.Name);
        }

        /// <summary>
        /// Unit test for IdentifiedObject Mrid setter
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectMridProperty")]
        public void MridPRoperty()
        {
            this.io1.Mrid = this.mrid1;
            Assert.AreEqual(this.io1.Mrid, this.mrid1);
            Assert.IsNotEmpty(this.io1.Mrid);
            Assert.IsNotNull(this.io1.Mrid);
        }

        /// <summary>
        /// Unit test for IdentifiedObject Equals method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectEqualsMethod")]
        public void EqualsMethod()
        {
            this.io1.Name = this.name1;
            this.io1.Mrid = this.mrid1;
            this.resultT = this.io1.Equals(this.io2);
            Assert.IsTrue(this.resultT);
            this.resultF = this.io1.Equals(this.io3);
            Assert.IsFalse(this.resultF);
            this.resultF = this.io1.Equals(this.io4);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for IdentifiedObject GetHashCode method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectGetHashCodeMethod")]
        public void GetHashCodeMethod()
        {
            int result = this.io1.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for IdentifiedObject HasProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectHasPropertyMethod")]
        public void HasPropertyMethod()
        {
            this.resultT = this.io1.HasProperty(ModelCode.IDENTIFIEDOBJECT_GID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.io1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(this.resultT);
            this.resultT = this.io1.HasProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
            Assert.IsTrue(this.resultT);
            this.resultF = this.io1.HasProperty(ModelCode.IDENTIFIEDOBJECT);
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for IdentifiedObject GetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectGetPropertyMethod")]
        public void GetPropertyMethod()
        {
            this.io1.GetProperty(ModelCode.IDENTIFIEDOBJECT_GID);
            this.io1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            this.io1.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
            Assert.Throws<Exception>(() => this.io1.GetProperty(ModelCode.IDENTIFIEDOBJECT));
        }

        /// <summary>
        /// Unit test for IdentifiedObject SetProperty method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectSetPropertyMethod")]
        public void SetPropertyMethod()
        {
            this.io1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            this.io1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_NAME));
            Assert.Throws<Exception>(() => this.io1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT)));
        }

        /// <summary>
        /// Unit test for IdentifiedObject ==
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectEqualityMethod")]
        public void EqualityMethod()
        {
            this.io1.Name = this.name1;
            this.io1.Mrid = this.mrid1;
            this.resultT = this.io1 == this.io2;
            Assert.IsTrue(this.resultT);
            this.resultT = this.io4 == null;
            Assert.IsTrue(this.resultT);
            this.resultF = this.io1 == this.io4;
            Assert.IsFalse(this.resultF);
            this.resultF = this.io4 == this.io1;
            Assert.IsFalse(this.resultF);
        }

        /// <summary>
        /// Unit test for IdentifiedObject !=
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectInequalityMethod")]
        public void InequalityMethod()
        {
            this.io1.Name = this.name1;
            this.io1.Mrid = this.mrid1;
            this.resultT = this.io1 != this.io3;
            Assert.IsTrue(this.resultT);
        }

        /// <summary>
        /// Unit test for IdentifiedObject AddReference method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectAddReferenceMethod")]
        public void AddReferenceMethod()
        {
            Assert.Throws<Exception>(() => this.io1.AddReference(ModelCode.IDENTIFIEDOBJECT_MRID, 1));
        }

        /// <summary>
        /// Unit test for IdentifiedObject RemoveReference method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectRemoveReferenceMethod")]
        public void RemoveReferenceMethod()
        {
            Assert.Throws<ModelException>(() => this.io1.RemoveReference(ModelCode.IDENTIFIEDOBJECT_MRID, 1));
        }

        /// <summary>
        /// Unit test for IdentifiedObject GetReferences method
        /// </summary>
        [Test]
        [TestCase(TestName = "IdentifiedObjectGetReferencesMethod")]
        public void GetReferencesMethod()
        {
            List<long> l = new List<long>();
            l.Add(1);
            l.Add(2);
            Dictionary<ModelCode, List<long>> d = new Dictionary<ModelCode, List<long>>();
            d[ModelCode.IDENTIFIEDOBJECT_MRID] = l;
            this.io1.GetReferences(d);
        }
    }
}