//-----------------------------------------------------------------------
// <copyright file="SCADACollectingTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACellectingServiceTest
{
	using EMS.Services.SCADACollectingService;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing SCADACrunching
	/// </summary>
	[TestFixture]
	public class SCADACollectingTest
    {

		/// <summary>
		/// Instance of SCADACollecting
		/// </summary>
		private SCADACollecting scadaCL;

		/// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
		public void SetupTest()
		{
			this.scadaCL = new SCADACollecting();
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "SCADACollectingConstructor")]
		public void ConstructorCL()
		{
			SCADACollecting cl = new SCADACollecting();
			Assert.IsNotNull(cl);
		}

		/// <summary>
		/// Unit test for SCADACollecting StartCollectingData method
		/// </summary>
		[Test]
		[TestCase(TestName = "SCADACollectingStartCollectingDataMethod")]
		public void StartCollectingDataMethod()
		{
			this.scadaCL.StartCollectingData();
		}

		/// <summary>
		/// Unit test for SCADACollecting GetDataFromSimulator method
		/// </summary>
		[Test]
		[TestCase(TestName = "SCADACollectingGetDataFromSimulatorMethod")]
		public void GetDataFromSimulatorMethod()
		{
			this.scadaCL.GetDataFromSimulator();
		}
	}
}
