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
		/// List of MeasurementUnit
		/// </summary>
		private List<MeasurementUnit> measurementsEnergyCons;

        /// <summary>
		/// List of MeasurementUnit
		/// </summary>
		private List<MeasurementUnit> measurementGenerators;

        /// <summary>
        /// Container for true result
        /// </summary>
        private bool resultT;

		/// <summary>
		/// Container for false result
		/// </summary>
		private bool resultF;

		/// <summary>
		/// Container for wind speed
		/// </summary>
		private float windSpeed;

		/// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
		public void SetupTest()
		{
			this.windSpeed = 15;

			this.ce = new CalculationEngine();
			var mu1 = new MeasurementUnit();
			mu1.Gid = 1;
			mu1.CurrentValue = 1;
			var mu2 = new MeasurementUnit();
			mu2.Gid = 2;
			mu2.CurrentValue = 2;
			var mu3 = new MeasurementUnit();
			mu3.Gid = 3;
			mu3.CurrentValue = 3;

			this.measurementsEnergyCons = new List<MeasurementUnit>();
			this.measurementsEnergyCons.Add(mu1);
			this.measurementsEnergyCons.Add(mu2);
			this.measurementsEnergyCons.Add(mu3);

            var muGen1 = new MeasurementUnit();
            var muGen2 = new MeasurementUnit();
            var muGen3 = new MeasurementUnit();

            muGen1.Gid = 4;
            muGen1.CurrentValue = 1;
            muGen2.Gid = 5;
            muGen2.CurrentValue = 2;
            muGen3.Gid = 6;
            muGen3.CurrentValue = 3;

            this.measurementGenerators = new List<MeasurementUnit>();
            this.measurementGenerators.Add(muGen1);
            this.measurementGenerators.Add(muGen2);
            this.measurementGenerators.Add(muGen3);
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
        [Ignore("izmenjeno za publish")]
        public void OptimizeMethod()
		{
			this.resultT = this.ce.Optimize(this.measurementsEnergyCons,this.measurementGenerators, this.windSpeed);
			Assert.IsTrue(this.resultT);
			this.resultF = this.ce.Optimize(new List<MeasurementUnit>(),new List<MeasurementUnit>(), this.windSpeed);
			Assert.IsFalse(this.resultF);
			this.resultF = this.ce.Optimize(null,null, this.windSpeed);
			Assert.IsFalse(this.resultF);
		}
	}
}