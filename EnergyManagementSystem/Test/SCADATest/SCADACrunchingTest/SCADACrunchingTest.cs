//-----------------------------------------------------------------------
// <copyright file="SCADACrunchingTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACrunchingServiceTest
{
	using EMS.CommonMeasurement;
	using EMS.ServiceContracts;
	using EMS.Services.SCADACrunchingService;
	using NSubstitute;
	using NUnit.Framework;
	using System.Collections.Generic;

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
			CalculationEngineProxy.Instance = Substitute.For<ICalculationEngineContract>();
			CalculationEngineProxy.Instance.OptimisationAlgorithm(new List<MeasurementUnit>(), new List<MeasurementUnit>()).ReturnsForAnyArgs(true);
			this.scadaCR = new SCADACrunching();
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "SCADACrunchingConstructor")]
		public void Constructor()
		{
			SCADACrunching cr = new SCADACrunching();
			Assert.IsNotNull(cr);
		}

		/// <summary>
		/// Unit test for SCADACrunching SendValues method
		/// </summary>
		[Test]
		[TestCase(TestName = "SCADACrunchingSendValuesMethod")]
		public void SendValuesMethod()
		{
			int length = 100;
			byte[] val = new byte[length];
			for (int i = 0; i < length; i++)
			{
				val[i] = 0x00;
			}
			val[0] = 3; // ReadHoldingRegisters
			val[1] = 20; // arrayLength
			val[2] = 1;
			val[3] = 2;
			val[4] = 3;
			val[5] = 4;
			val[6] = 5;
			val[7] = 6;
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