//-----------------------------------------------------------------------
// <copyright file="LinearOptimizationTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace CalculationEngineServiceTest
{
	using System.Collections.Generic;
	using EMS.Common;
	using EMS.CommonMeasurement;
	using EMS.Services.CalculationEngineService;
	using EMS.Services.CalculationEngineService.LinearAlgorithm;
	using EMS.Services.NetworkModelService.DataModel.Production;
	using EMS.Services.NetworkModelService.DataModel.Wires;
	using NUnit.Framework;	

	/// <summary>
	/// Class for unit testing LinearOptimization
	/// </summary>
	[TestFixture]
	public class LinearOptimizationTest
	{
		/// <summary>
		/// Instance of LinearOptimization
		/// </summary>
		public LinearOptimization lo;

		/// <summary>
		/// Instance of LinearOptimization
		/// </summary>
		public LinearOptimization lino;

		/// <summary>
		/// Container for min production
		/// </summary>
		private float minProduction;

		/// <summary>
		/// Container for max production
		/// </summary>
		private float maxProduction;

		/// <summary>
		/// Container for optimized linear
		/// </summary>
		private float optimizedLinear;

		/// <summary>
		/// Container for wind optimized linear
		/// </summary>
		private float windOptimizedLinear;

		/// <summary>
		/// Container for wind optimized pct linear
		/// </summary>
		private float windOptimizedPctLinear;

		/// <summary>
		/// Container for profit
		/// </summary>
		private float profit;

		/// <summary>
		/// Container for co2 emission non renewable
		/// </summary>
		private float co2EmissionNonRenewable;

		/// <summary>
		/// Container for co2 emmission renewable
		/// </summary>
		private float co2EmmissionRenewable;

		/// <summary>
		/// Container for total cost with renewable
		/// </summary>
		private float totalCostWithRenewable;

		/// <summary>
		/// Container for total cost non renewable
		/// </summary>
		private float totalCostNonRenewable;

		private Dictionary<long, OptimisationModel> optModelMap;
		private float consumption;
		private float windSpeed;
		private float sunlight;

		/// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
		public void SetupTest()
		{
			optimizedLinear = 50;
			windOptimizedLinear = 20;
			windOptimizedPctLinear = 10;
			profit = 50;
			co2EmissionNonRenewable = 2;
			co2EmmissionRenewable = 12;
			totalCostWithRenewable = 100;
			totalCostNonRenewable = 150;
			minProduction = 0;
			maxProduction = 0;

			windSpeed = 20;
			sunlight = 20;
			consumption = 375;

			#region fuels
			EMSFuel f1 = new EMSFuel(11);
			f1.FuelType = EmsFuelType.coal;
			f1.UnitPrice = 10;
			EMSFuel f2 = new EMSFuel(12);
			f2.FuelType = EmsFuelType.hydro;
			f2.UnitPrice = 15;
			EMSFuel f3 = new EMSFuel(13);
			f3.FuelType = EmsFuelType.wind;
			f3.UnitPrice = 1;
			EMSFuel f4 = new EMSFuel(14);
			f4.FuelType = EmsFuelType.solar;
			f4.UnitPrice = 1;
			EMSFuel f5 = new EMSFuel(15);
			f5.FuelType = EmsFuelType.oil;
			f5.UnitPrice = 20;
			#endregion

			#region synchronous machines
			SynchronousMachine sm1 = new SynchronousMachine(1);
			sm1.Active = true;
			sm1.Fuel = 11;
			sm1.MaxQ = 100;
			sm1.MinQ = 10;
			sm1.Mrid = "sm1";
			minProduction += sm1.MinQ;
			maxProduction += sm1.MaxQ;
			SynchronousMachine sm2 = new SynchronousMachine(2);
			sm2.Active = true;
			sm2.Fuel = 12;
			sm2.MaxQ = 100;
			sm2.MinQ = 10;
			sm2.Mrid = "sm2";
			minProduction += sm2.MinQ;
			maxProduction += sm2.MaxQ;
			SynchronousMachine sm3 = new SynchronousMachine(3);
			sm3.Active = true;
			sm3.Fuel = 13;
			sm3.MaxQ = 100;
			sm3.MinQ = 10;
			sm3.Mrid = "sm3";
			minProduction += sm3.MinQ;
			maxProduction += sm3.MaxQ;
			SynchronousMachine sm4 = new SynchronousMachine(4);
			sm4.Active = true;
			sm4.Fuel = 14;
			sm4.MaxQ = 100;
			sm4.MinQ = 10;
			sm4.Mrid = "sm4";
			minProduction += sm4.MinQ;
			maxProduction += sm4.MaxQ;
			SynchronousMachine sm5 = new SynchronousMachine(5);
			sm5.Active = true;
			sm5.Fuel = 15;
			sm5.MaxQ = 100;
			sm5.MinQ = 10;
			sm5.Mrid = "sm5";
			minProduction += sm5.MinQ;
			maxProduction += sm5.MaxQ;
			#endregion

			#region measurement units
			MeasurementUnit mu1 = new MeasurementUnit();
			mu1.CurrentValue = 50;
			mu1.Gid = sm1.GlobalId;
			mu1.MaxValue = sm1.MaxQ;
			mu1.MinValue = sm1.MinQ;
			mu1.OptimizationType = OptimizationType.Linear;
			MeasurementUnit mu2 = new MeasurementUnit();
			mu2.CurrentValue = 50;
			mu2.Gid = sm2.GlobalId;
			mu2.MaxValue = sm2.MaxQ;
			mu2.MinValue = sm2.MinQ;
			mu2.OptimizationType = OptimizationType.Linear;
			MeasurementUnit mu3 = new MeasurementUnit();
			mu3.CurrentValue = 50;
			mu3.Gid = sm3.GlobalId;
			mu3.MaxValue = sm3.MaxQ;
			mu3.MinValue = sm3.MinQ;
			mu3.OptimizationType = OptimizationType.Linear;
			MeasurementUnit mu4 = new MeasurementUnit();
			mu4.CurrentValue = 50;
			mu4.Gid = sm4.GlobalId;
			mu4.MaxValue = sm4.MaxQ;
			mu4.MinValue = sm4.MinQ;
			mu4.OptimizationType = OptimizationType.Linear;
			MeasurementUnit mu5 = new MeasurementUnit();
			mu5.CurrentValue = 50;
			mu5.Gid = sm5.GlobalId;
			mu5.MaxValue = sm5.MaxQ;
			mu5.MinValue = sm5.MinQ;
			mu5.OptimizationType = OptimizationType.Linear;
			#endregion

			#region curve models
			SynchronousMachineCurveModel smcm1 = new SynchronousMachineCurveModel();
			smcm1.MRId = sm1.Mrid;
			smcm1.A = 1;
			smcm1.B = 2;
			smcm1.C = 3;
			SynchronousMachineCurveModel smcm2 = new SynchronousMachineCurveModel();
			smcm2.MRId = sm2.Mrid;
			smcm2.A = 1;
			smcm2.B = 2;
			smcm2.C = 3;
			SynchronousMachineCurveModel smcm3 = new SynchronousMachineCurveModel();
			smcm3.MRId = sm3.Mrid;
			smcm3.A = 1;
			smcm3.B = 2;
			smcm3.C = 3;
			SynchronousMachineCurveModel smcm4 = new SynchronousMachineCurveModel();
			smcm4.MRId = sm4.Mrid;
			smcm4.A = 1;
			smcm4.B = 2;
			smcm4.C = 3;
			SynchronousMachineCurveModel smcm5 = new SynchronousMachineCurveModel();
			smcm5.MRId = sm5.Mrid;
			smcm5.A = 1;
			smcm5.B = 2;
			smcm5.C = 3;
			#endregion

			#region optimisation model
			optModelMap = new Dictionary<long, OptimisationModel>();

			OptimisationModel om1 = new OptimisationModel(sm1, f1, mu1, windSpeed, sunlight, smcm1);
			optModelMap.Add(om1.GlobalId, om1);
			OptimisationModel om2 = new OptimisationModel(sm2, f2, mu2, windSpeed, sunlight, smcm2);
			optModelMap.Add(om2.GlobalId, om2);
			OptimisationModel om3 = new OptimisationModel(sm3, f3, mu3, windSpeed, sunlight, smcm3);
			optModelMap.Add(om3.GlobalId, om3);
			OptimisationModel om4 = new OptimisationModel(sm4, f4, mu4, windSpeed, sunlight, smcm4);
			optModelMap.Add(om4.GlobalId, om4);
			OptimisationModel om5 = new OptimisationModel(sm5, f5, mu5, windSpeed, sunlight, smcm5);
			optModelMap.Add(om5.GlobalId, om5);
			#endregion

			lo = new LinearOptimization(minProduction, maxProduction);
			lino = new LinearOptimization(minProduction, maxProduction);
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationConstructor")]
		public void Constructor()
		{
			LinearOptimization linopt = new LinearOptimization(minProduction, maxProduction);
			Assert.IsNotNull(linopt);
		}

		/// <summary>
		/// Unit test for LinearOptimization OptimizedLinear property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationOptimizedLinearProperty")]
		public void OptimizedLinearProperty()
		{
			lo.OptimizedLinear = optimizedLinear;
			Assert.AreEqual(lo.OptimizedLinear, optimizedLinear);
		}

		/// <summary>
		/// Unit test for LinearOptimization WindOptimizedLinear property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationWindOptimizedLinearProperty")]
		public void WindOptimizedLinearProperty()
		{
			lo.WindOptimizedLinear = windOptimizedLinear;
			Assert.AreEqual(lo.WindOptimizedLinear, windOptimizedLinear);
		}

		/// <summary>
		/// Unit test for LinearOptimization WindOptimizedPctLinear property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationWindOptimizedPctLinearProperty")]
		public void WindOptimizedPctLinearProperty()
		{
			lo.WindOptimizedPctLinear = windOptimizedPctLinear;
			Assert.AreEqual(lo.WindOptimizedPctLinear, windOptimizedPctLinear);
		}

		/// <summary>
		/// Unit test for LinearOptimization Profit property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationProfitProperty")]
		public void ProfitProperty()
		{
			lo.Profit = profit;
			Assert.AreEqual(lo.Profit, profit);
		}

		/// <summary>
		/// Unit test for LinearOptimization CO2EmissionNonRenewable property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationCO2EmissionNonRenewableProperty")]
		public void CO2EmissionNonRenewableProperty()
		{
			lo.CO2EmissionNonRenewable = co2EmissionNonRenewable;
			Assert.AreEqual(lo.CO2EmissionNonRenewable, co2EmissionNonRenewable);
		}

		/// <summary>
		/// Unit test for LinearOptimization CO2EmmissionRenewable property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationCO2EmmissionRenewableProperty")]
		public void CO2EmmissionRenewableProperty()
		{
			lo.CO2EmmissionRenewable = co2EmmissionRenewable;
			Assert.AreEqual(lo.CO2EmmissionRenewable, co2EmmissionRenewable);
		}

		/// <summary>
		/// Unit test for LinearOptimization TotalCostWithRenewable property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationTotalCostWithRenewableProperty")]
		public void TotalCostWithRenewableProperty()
		{
			lo.TotalCostWithRenewable = totalCostWithRenewable;
			Assert.AreEqual(lo.TotalCostWithRenewable, totalCostWithRenewable);
		}

		/// <summary>
		/// Unit test for LinearOptimization TotalCostNonRenewable property
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationTotalCostNonRenewableProperty")]
		public void TotalCostNonRenewableProperty()
		{
			lo.TotalCostNonRenewable = totalCostNonRenewable;
			Assert.AreEqual(lo.TotalCostNonRenewable, totalCostNonRenewable);
		}

		/// <summary>
		/// Unit test for LinearOptimization Start method
		/// </summary>
		[Test]
		[TestCase(TestName = "LinearOptimizationStartMethod")]
		public void StartMethod()
		{			
			optModelMap = lino.Start(optModelMap, consumption);
		}
	}
}