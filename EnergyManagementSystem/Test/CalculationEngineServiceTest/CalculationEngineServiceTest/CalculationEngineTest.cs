//-----------------------------------------------------------------------
// <copyright file="CalculationEngineTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace CalculationEngineServiceTest
{
	using System.Collections.Generic;
	using NUnit.Framework;
	using EMS.Services.CalculationEngineService;
	using EMS.CommonMeasurement;

	/// <summary>
	/// Class for unit testing CalculationEngine
	/// </summary>
	[TestFixture]
	public class CalculationEngineTest
	{
		/// <summary>
		/// Instance of CalculationEngine
		/// </summary>
		private CalculationEngine ce;

		/// <summary>
		/// Instance of MeasurementUnit
		/// </summary>
		private MeasurementUnit mu1;

		/// <summary>
		/// Instance of MeasurementUnit
		/// </summary>
		private MeasurementUnit mu2;

		/// <summary>
		/// Instance of MeasurementUnit
		/// </summary>
		private MeasurementUnit mu3;

		/// <summary>
		/// List of MeasurementUnit
		/// </summary>
		private List<MeasurementUnit> measurements;

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
			this.ce = new CalculationEngine();
			this.mu1 = new MeasurementUnit();
			this.mu1.Gid = 1;
			this.mu1.CurrentValue = 1;
			this.mu2 = new MeasurementUnit();
			this.mu2.Gid = 2;
			this.mu2.CurrentValue = 2;
			this.mu3 = new MeasurementUnit();
			this.mu3.Gid = 3;
			this.mu3.CurrentValue = 3;
			this.measurements = new List<MeasurementUnit>();
			this.measurements.Add(this.mu1);
			this.measurements.Add(this.mu2);
			this.measurements.Add(this.mu3);
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "CalculationEngineConstructor")]
		public void Constructor()
		{
			CalculationEngine calce = new CalculationEngine();
			Assert.IsNotNull(calce);
		}

		/// <summary>
		/// Unit test for CalculationEngine Optimize method
		/// </summary>
		[Test]
		[TestCase(TestName = "CalculationEngineOptimizeMethod")]
		public void OptimizeMethod()
		{
			this.resultT = this.ce.Optimize(this.measurements);
			Assert.IsTrue(this.resultT);
			this.resultF = this.ce.Optimize(new List<MeasurementUnit>());
			Assert.IsFalse(this.resultF);
			this.resultF = this.ce.Optimize(null);
			Assert.IsFalse(this.resultF);
		}
	}
}