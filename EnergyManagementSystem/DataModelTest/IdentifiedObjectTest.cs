//-----------------------------------------------------------------------
// <copyright file="IdentifiedObjectTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
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
    }
}
