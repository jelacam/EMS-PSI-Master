//-----------------------------------------------------------------------
// <copyright file="AnalogLocationTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------



namespace SCADACommandingTest
{
    using EMS.Services.SCADACommandingService;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing CMDAnalogLocation
    /// </summary>
    [TestFixture]
    public class CMDAnalogLocationTest
    {
        /// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
        [TestCase(TestName = "CMDAnalogLocationConstructor")]
        public void Constructor()
        {
            CMDAnalogLocation al = new CMDAnalogLocation();
            Assert.IsNotNull(al);
        }
    }
}
