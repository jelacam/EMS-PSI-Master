//-----------------------------------------------------------------------
// <copyright file="IdentifiedObjectTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
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
        /// Unit test for constructor with parameters
        /// </summary>
        /// <param name="globalId">globalId for the constructor</param>
        [Test]
        [TestCase(1623)]
        public void Constructor(long globalId)
        {
            IdentifiedObject io = new IdentifiedObject(globalId);
            Assert.IsNotNull(io);
        }

        /// <summary>
        /// Unit test for IdentifiedObject GlobalId setter
        /// </summary>
        /// <param name="globalId">globalId property being set and asserted</param>
        [Test]
        [TestCase(10)]
        public void GlobalIdPropertySet(long globalId)
        {
            IdentifiedObject io = new IdentifiedObject(1623);
            io.GlobalId = globalId;
            Assert.AreEqual(io.GlobalId, globalId);
        }

        /// <summary>
        /// Unit test for IdentifiedObject Name setter
        /// </summary>
        /// <param name="name">name property being set and asserted</param>
        [Test]
        [TestCase("name")]
        public void NamePropertySet(string name)
        {
            IdentifiedObject io = new IdentifiedObject(1623);
            io.Name = name;
            Assert.AreEqual(io.Name, name);
            Assert.IsNotEmpty(io.Name);
            Assert.IsNotNull(io.Name);
        }

        /// <summary>
        /// Unit test for IdentifiedObject Mrid setter
        /// </summary>
        /// <param name="mrid">mrid property being set and asserted</param>
        [Test]
        [TestCase("mrid")]
        public void MridPRopertySet(string mrid)
        {
            IdentifiedObject io = new IdentifiedObject(1623);
            io.Mrid = mrid;
            Assert.AreEqual(io.Mrid, mrid);
            Assert.IsNotEmpty(io.Mrid);
            Assert.IsNotNull(io.Mrid);
        }

        /// <summary>
        /// Unit test for IdentifiedObject Equals method
        /// </summary>
        /// <param name="globalId1">first globalId parameter</param>
        /// <param name="globalId2">second globalId parameter</param>
        /// <param name="name1">first name parameter</param>
        /// <param name="name2">second name parameter</param>
        /// <param name="mrid1">first mrid parameter</param>
        /// <param name="mrid2">second mrid parameter</param>
        [Test]
        [TestCase(1623, 10, "name1", "name2", "mrid1", "mrid2")]
        public void EqualsMethod(long globalId1, long globalId2, string name1, string name2, string mrid1, string mrid2)
        {
            IdentifiedObject io1 = new IdentifiedObject(globalId1);
            IdentifiedObject io2 = new IdentifiedObject(globalId1);
            IdentifiedObject io3 = new IdentifiedObject(globalId2);
            IdentifiedObject io4 = null;
            io1.Name = name1;
            io2.Name = name1;
            io3.Name = name2;
            io1.Mrid = mrid1;
            io2.Mrid = mrid1;
            io3.Mrid = mrid2;
            bool resultT = io1.Equals(io2);
            Assert.IsTrue(resultT);
            bool resultF = io1.Equals(io3);
            Assert.IsFalse(resultF);
            resultF = io1.Equals(io4);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for IdentifiedObject GetHashCode method
        /// </summary>
        [Test]
        public void GetHashCodeMethod()
        {
            IdentifiedObject io = new IdentifiedObject(1623);
            int result = io.GetHashCode();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unit test for IdentifiedObject HasProperty method
        /// </summary>
        [Test]
        public void HasPropertyMethod()
        {
            IdentifiedObject io = new IdentifiedObject(1623);
            bool resultT = io.HasProperty(ModelCode.IDENTIFIEDOBJECT_GID);
            Assert.IsTrue(resultT);
            resultT = io.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            Assert.IsTrue(resultT);
            resultT = io.HasProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
            Assert.IsTrue(resultT);
            bool resultF = io.HasProperty(ModelCode.IDENTIFIEDOBJECT);
            Assert.IsFalse(resultF);
        }

        /// <summary>
        /// Unit test for IdentifiedObject GetProperty method
        /// </summary>
        [Test]
        public void GetPropertyMethod()
        {
            IdentifiedObject io = new IdentifiedObject(1623);
            io.GetProperty(ModelCode.IDENTIFIEDOBJECT_GID);
            io.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
            io.GetProperty(ModelCode.IDENTIFIEDOBJECT_NAME);
        }

        /// <summary>
        /// Unit test for IdentifiedObject SetProperty method
        /// </summary>
        [Test]
        public void SetPropertyMethod()
        {
            IdentifiedObject io = new IdentifiedObject(1623);
            io.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
            io.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_NAME));
        }
    }
}