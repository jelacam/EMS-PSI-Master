//-----------------------------------------------------------------------
// <copyright file="SCADACommandingTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACommandingTest
{
	using EMS.CommonMeasurement;
	using EMS.ServiceContracts;
	using EMS.Services.SCADACommandingService;
	using NSubstitute;
	using NUnit.Framework;
	using System.Collections.Generic;

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
		/// Measurement units list
		/// </summary>
		private List<MeasurementUnit> measurements;

		/// <summary>
		/// Param for unit test result
		/// </summary>
		private bool resultSendDataToSimulator;

		/// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
		public void SetupTest()
		{
			scadaCmd = new SCADACommanding();
			measurements = new List<MeasurementUnit>();
			measurements.Add(
				new MeasurementUnit
				{
					CurrentValue = 10,
					Gid = 20000
				});
			measurements.Add(
				new MeasurementUnit
				{
					CurrentValue = 20,
					Gid = 20001
				});

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

		/// <summary>
		/// Unit test for SendDataToSimulator method
		/// </summary>
		[Test]
		[TestCase(TestName = "SendDataToSimulatorTest")]
		public void SendDataToSimulatorTest()
		{
			resultSendDataToSimulator = scadaCmd.SendDataToSimulator(measurements);
			Assert.IsTrue(resultSendDataToSimulator);
		}

		/// <summary>
		/// Unit test for test write method
		/// </summary>
		[Test]
		[TestCase(TestName = "TestWriteMethod")]
		public void TestWriteMethod()
		{
			scadaCmd.TestWrite();
		}
	}
}
