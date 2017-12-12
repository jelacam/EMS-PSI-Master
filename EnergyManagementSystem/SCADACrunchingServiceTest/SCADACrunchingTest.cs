//-----------------------------------------------------------------------
// <copyright file="SCADACrunchingTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACrunchingServiceTest
{
	using EMS.Services.SCADACrunchingService;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing SCADACrunching
	/// </summary>
	[TestFixture]
	public class SCADACrunchingTest
	{
		/// <summary>
		/// Instance of SCADACrunching
		/// </summary>
		private SCADACrunching scadaCR;

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
			this.scadaCR = new SCADACrunching();
		}

		/// <summary>
		/// Unit test for SCADACrunching SendValues method
		/// </summary>
		[Test]
		[TestCase(TestName = "SCADACrunchingSendValuesMethod")]
		public void SendValuesMethod()
		{
			byte[] val = new byte[3];
			val[0] = 1; // ReadCoils
			val[1] = 1; // arrayLength
			val[2] = 0;
			this.resultT = this.scadaCR.SendValues(val);
			Assert.IsTrue(this.resultT);
		}

		/// <summary>
		/// Unit test for SCADACrunching Test method
		/// </summary>
		[Test]
		[TestCase(TestName = "SCADACrunchingTestMethod")]
		public void TestMethod()
		{
			this.scadaCR.Test();
		}
	}
}