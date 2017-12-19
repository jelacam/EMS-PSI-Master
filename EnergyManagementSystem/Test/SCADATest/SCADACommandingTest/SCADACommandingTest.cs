//-----------------------------------------------------------------------
// <copyright file="SCADACommandingTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACommandingTest
{
    using EMS.Services.SCADACommandingService;
    using NUnit.Framework;

    /// <summary>
    /// Class for unit testing SCADACommanding
    /// </summary>
    /// 
    [TestFixture]
    public class SCADACommandingTest
    {
        /// <summary>
        /// Instance of SCADACommanding
        /// </summary>
        private SCADACommanding scadaCmd;

        /// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
        public void SetupTest()
        {
            scadaCmd = new SCADACommanding();
        }

        /// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
        [TestCase(TestName = "SCADACommandingConstructor")]
        public void ConstructorCommanding()
        {
            SCADACommanding cmd = new SCADACommanding();
            Assert.IsNotNull(cmd);
        }
    }
}
