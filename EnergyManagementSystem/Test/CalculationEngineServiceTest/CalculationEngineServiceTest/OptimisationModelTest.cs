//-----------------------------------------------------------------------
// <copyright file="OptimisationModelTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace CalculationEngineServiceTest
{
	using System;
	using EMS.Common;
	using EMS.CommonMeasurement;
	using EMS.Services.CalculationEngineService;
	using EMS.Services.NetworkModelService.DataModel.Production;
	using EMS.Services.NetworkModelService.DataModel.Wires;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing OptimisationModel
	/// </summary>
	[TestFixture]
	public class OptimisationModelTest
	{
		/// <summary>
		/// Instance of OptimisationModel
		/// </summary>
		public OptimisationModel om1;

		/// <summary>
		/// Instance of OptimisationModel
		/// </summary>
		public OptimisationModel om2;

		public SynchronousMachine sm1;
		public SynchronousMachine sm2;

		public EMSFuel emsf11;
		public EMSFuel emsf12;


		public MeasurementUnit mu1;
		public MeasurementUnit mu2;

		public SynchronousMachineCurveModel smcm1;
		public SynchronousMachineCurveModel smcm2;

		public float windSpeed;

		public float sunlight;

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
			emsf11 = new EMSFuel(11);
			emsf11.FuelType = EmsFuelType.coal;
			emsf11.Mrid = "Fuel1";
			emsf11.Name = "Fuel1";
			emsf11.UnitPrice = 3;

			emsf12 = new EMSFuel(11);
			emsf12.FuelType = EmsFuelType.wind;
			emsf12.Mrid = "Fuel2";
			emsf12.Name = "Fuel2";
			emsf12.UnitPrice = 1;

			sm1 = new SynchronousMachine(1);
			sm1.Active = true;
			sm1.Fuel = emsf11.GlobalId;
			sm1.LoadPct = 100;
			sm1.MaxCosPhi = 0.8f;
			sm1.MaxQ = 1000;
			sm1.MinCosPhi = -0.8f;
			sm1.MinQ = 10;
			sm1.Mrid = "SyncMachine1";
			sm1.Name = "SyncMachine1";
			sm1.RatedS = 1000;

			sm2 = new SynchronousMachine(2);
			sm2.Active = false;
			sm2.Fuel = emsf12.GlobalId;
			sm2.LoadPct = 100;
			sm2.MaxCosPhi = 0.8f;
			sm2.MaxQ = 1000;
			sm2.MinCosPhi = -0.8f;
			sm2.MinQ = 10;
			sm2.Mrid = "SyncMachine2";
			sm2.Name = "SyncMachine2";
			sm2.RatedS = 1000;

			mu1 = new MeasurementUnit();
			mu1.CurrentValue = 200;
			mu1.Gid = sm1.GlobalId;
			mu1.MaxValue = sm1.MaxQ;
			mu1.MinValue = sm1.MinQ;
			mu1.TimeStamp = DateTime.Now;

			mu2 = new MeasurementUnit();
			mu2.CurrentValue = 200;
			mu2.Gid = sm2.GlobalId;
			mu2.MaxValue = sm2.MaxQ;
			mu2.MinValue = sm2.MinQ;
			mu2.TimeStamp = DateTime.Now;

			smcm1 = new SynchronousMachineCurveModel("SyncMachine1", 1, 2, 3);
			smcm2 = new SynchronousMachineCurveModel("SyncMachine2", 1, 2, 3);
			windSpeed = 15;
			sunlight = 55;

			om1 = new OptimisationModel(sm1, emsf11, mu1, windSpeed, sunlight, smcm1);
			om2 = new OptimisationModel(sm2, emsf12, mu2, windSpeed, sunlight, smcm2);
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelConstructorWithoutParameters")]
		public void Constructor1()
		{
			OptimisationModel optM = new OptimisationModel();
			Assert.IsNotNull(optM);
		}

		/// <summary>
		/// Unit test for constructor with parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelConstructorWithParameters")]
		public void Constructor2()
		{
			OptimisationModel optM = new OptimisationModel(sm1, emsf11, mu1, windSpeed, sunlight, smcm1);
			Assert.IsNotNull(optM);
		}

		/// <summary>
		/// Unit test for OptimisationModel GlobalId property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelGlobalIdProperty")]
		public void GlobalIdProperty()
		{
			Assert.IsNotNull(om1.GlobalId);
		}

		/// <summary>
		/// Unit test for OptimisationModel Price property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelPriceProperty")]
		public void PriceProperty()
		{
			Assert.IsNotNull(om1.Price);
		}

		/// <summary>
		/// Unit test for OptimisationModel MeasuredValue property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelMeasuredValueProperty")]
		public void MeasuredValueProperty()
		{
			Assert.IsNotNull(om1.MeasuredValue);
		}

		/// <summary>
		/// Unit test for OptimisationModel LinearOptimizedValue property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelLinearOptimizedValueProperty")]
		public void LinearOptimizedValueProperty()
		{
			Assert.IsNotNull(om1.LinearOptimizedValue);
		}

		/// <summary>
		/// Unit test for OptimisationModel GenericOptimizedValue property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelGenericOptimizedValueProperty")]
		public void GenericOptimizedValueProperty()
		{
			Assert.IsNotNull(om1.GenericOptimizedValue);
		}

		/// <summary>
		/// Unit test for OptimisationModel MinPower property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelMinPowerProperty")]
		public void MinPowerProperty()
		{
			Assert.IsNotNull(om1.MinPower);
		}

		/// <summary>
		/// Unit test for OptimisationModel MaxPower property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelMaxPowerProperty")]
		public void MaxPowerProperty()
		{
			Assert.IsNotNull(om1.MaxPower);
		}

		/// <summary>
		/// Unit test for OptimisationModel Managable property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelManagableProperty")]
		public void ManagableProperty()
		{
			Assert.IsNotNull(om1.Managable);
		}

		/// <summary>
		/// Unit test for OptimisationModel Renewable property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelRenewableProperty")]
		public void RenewableProperty()
		{
			Assert.IsNotNull(om1.Renewable);
			resultF = om1.Renewable;
			Assert.IsFalse(resultF);
			resultT = om2.Renewable;
			Assert.IsTrue(resultT);
		}

		/// <summary>
		/// Unit test for OptimisationModel WindPct property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelWindPctProperty")]
		public void WindPctProperty()
		{
			Assert.IsNotNull(om1.WindPct);
		}

		/// <summary>
		/// Unit test for OptimisationModel EmsFuel property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelEmsFuelProperty")]
		public void EmsFuelProperty()
		{
			Assert.IsNotNull(om1.EmsFuel);
		}

		/// <summary>
		/// Unit test for OptimisationModel Curve property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelCurveProperty")]
		public void CurveProperty()
		{
			Assert.IsNotNull(om1.Curve);
		}

		/// <summary>
		/// Unit test for OptimisationModel EmissionFactor property
		/// </summary>
		[Test]
		[TestCase(TestName = "OptimisationModelEmissionFactorProperty")]
		public void EmissionFactorProperty()
		{
			Assert.IsNotNull(om1.EmissionFactor);
		}
	}
}