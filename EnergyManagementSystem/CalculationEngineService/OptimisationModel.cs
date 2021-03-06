﻿//-----------------------------------------------------------------------
// <copyright file="OptimisationModel.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
	using System;
	using Common;
	using CommonMeasurement;
	using NetworkModelService.DataModel.Production;
	using NetworkModelService.DataModel.Wires;

	/// <summary>
	/// class for OptimisationModel
	/// </summary>
	public class OptimisationModel
	{
		/// <summary>
		/// Gets or sets globalId for OptimisationModel
		/// </summary>
		public long GlobalId { get; set; }

		/// <summary>
		/// Gets or sets price for OptimisationModel
		/// </summary>
		public float Price { get; set; }

		/// <summary>
		/// Gets or sets measuredValue for OptimisationModel
		/// </summary>
		public float MeasuredValue { get; set; }

		/// <summary>
		/// Gets or sets linearOptimizedValue for OptimisationModel
		/// </summary>
		public float LinearOptimizedValue { get; set; }

		/// <summary>
		/// Gets or sets genericOptimizedValue for OptimisationModel
		/// </summary>
		public float GenericOptimizedValue { get; set; }

		/// <summary>
		/// Gets or sets minPower for OptimisationModel
		/// </summary>
		public float MinPower { get; set; }

		/// <summary>
		/// Gets or sets maxPower for OptimisationModel
		/// </summary>
		public float MaxPower { get; set; }

		/// <summary>
		/// Gets or sets managable for OptimisationModel
		/// </summary>
		public bool Managable { get; set; }

		/// <summary>
		/// Gets or sets renewable for OptimisationModel
		/// </summary>
		public bool Renewable { get; set; }

		/// <summary>
		/// Gets or sets windPct for OptimisationModel
		/// </summary>
		public float WindPct { get; set; }

		/// <summary>
		/// Gets emsFuel for OptimisationModel
		/// </summary>
		public EMSFuel EmsFuel { get; private set; }

		/// <summary>
		/// Gets data about curve
		/// </summary>
		public SynchronousMachineCurveModel Curve { get; private set; }

		/// <summary>
		/// Gets emission factor for fuel
		/// </summary>
		public float EmissionFactor { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OptimisationModel" /> class
		/// </summary>
		public OptimisationModel()
		{
			GlobalId = 0;
			Price = 0;
			MeasuredValue = 0;
			LinearOptimizedValue = 0;
			GenericOptimizedValue = 0;
			MinPower = 0;
			MaxPower = 0;
			Managable = true;
			Renewable = false;
			WindPct = 1;
			Curve = new SynchronousMachineCurveModel();
			EmsFuel = null;
			EmissionFactor = 1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OptimisationModel" /> class
		/// </summary>
		/// <param name="sm">generator</param>
		/// <param name="emsf">fuel</param>
		/// <param name="mu">measured value</param>
		/// <param name="windSpeed">speed of wind</param>
		/// <param name="sunlight">sunlight percent</param>
		/// <param name="smcm">generator curve</param>
		public OptimisationModel(SynchronousMachine sm, EMSFuel emsf, MeasurementUnit mu, float windSpeed, float sunlight, SynchronousMachineCurveModel smcm)
		{
			GlobalId = sm.GlobalId;
			MeasuredValue = mu.CurrentValue;
			LinearOptimizedValue = 0; // izracunati
			GenericOptimizedValue = 0; // izracunati			
			EmsFuel = emsf;
			Curve = smcm;
			EmissionFactor = ChooseEmissionFactor(emsf.FuelType);

			WindPct = emsf.FuelType.Equals(EmsFuelType.wind) ? CalculateWindPct(windSpeed) : 100;
			Managable = ((emsf.FuelType.Equals(EmsFuelType.wind) && WindPct == 0) || (emsf.FuelType.Equals(EmsFuelType.solar) && (0 >= sunlight || sunlight > 100))) ? false : true; //true=optimizuj
			Renewable = (emsf.FuelType.Equals(EmsFuelType.wind) || emsf.FuelType.Equals(EmsFuelType.solar)) ? true : false;

			/*if (!Managable)
			{
				Price = 0;
			}
			else if (Renewable)
			{
				Price = 1;
			}
			else
			{
				Price = CalculatePrice(MeasuredValue);
			}*/
			Price = 0;

			MinPower = ((!Managable) || (Renewable && WindPct == 0) || (Renewable && (sunlight <= 0 || sunlight > 100))) ? 0 : sm.MinQ;

			if ((!Managable) || (emsf.FuelType.Equals(EmsFuelType.wind) && WindPct == 0) || (emsf.FuelType.Equals(EmsFuelType.solar) && (sunlight <= 0 || sunlight > 100)))
			{
				MaxPower = 0;
			}
			else if (emsf.FuelType.Equals(EmsFuelType.wind) && WindPct > 0)
			{
				MaxPower = (sm.MaxQ - sm.MinQ) / 100 * WindPct + sm.MinQ;
			}
			else if (emsf.FuelType.Equals(EmsFuelType.solar) && 0 < sunlight && sunlight <= 100)
			{
				MaxPower = (sm.MaxQ - sm.MinQ) / 100 * sunlight + sm.MinQ;
			}
			else
			{
				MaxPower = sm.MaxQ;
			}
		}

		/// <summary>
		/// Calculates emission factor for diferent fuel
		/// </summary>
		/// <param name="fuelType">fuel type</param>
		/// <returns>emission factor</returns>
		public float ChooseEmissionFactor(EmsFuelType fuelType)
		{
			float retVal = 0;
			switch (fuelType)
			{
				case EmsFuelType.coal:
					retVal = 0.30f;
					break;
				case EmsFuelType.hydro:
					retVal = 0;
					break;
				case EmsFuelType.oil:
					retVal = 0.25f;
					break;
				case EmsFuelType.solar:
					retVal = 0;
					break;
				case EmsFuelType.wind:
					retVal = 0;
					break;
				default:
					retVal = 0;
					break;
			}

			return retVal * 0.001f;
		}

		/// <summary>
		/// Calculates price for current power based on curve for generator
		/// </summary>
		/// <param name="measuredValue">current power</param>
		/// <returns>price of power</returns>
		public float CalculatePrice(float measuredValue)
		{
            if (Renewable)
            {
                return 1;
            }

			float price = 0;
			float amount = (float)Curve.A * measuredValue + (float)Curve.B;
			price = amount * EmsFuel.UnitPrice;
			return price;
		}

		/// <summary>
		/// Calculates percentage of production for wind generators
		/// </summary>
		/// <param name="windSpeed">speed of wind</param>
		/// <returns>production percentage</returns>
		public float CalculateWindPct(float windSpeed)
		{
			float pct = 0;

			if (windSpeed < 4.5)
			{
				pct = 0;
			}
			else if (4.5 <= windSpeed && windSpeed < 5)
			{
				pct = 5;
			}
			else if (5 <= windSpeed && windSpeed < 6)
			{
				pct = 15;
			}
			else if (6 <= windSpeed && windSpeed < 6.5)
			{
				pct = 20;
			}
			else if (6.5 <= windSpeed && windSpeed < 7)
			{
				pct = 30;
			}
			else if (7 <= windSpeed && windSpeed < 7.5)
			{
				pct = 40;
			}
			else if (7.5 <= windSpeed && windSpeed < 8)
			{
				pct = 50;
			}
			else if (8 <= windSpeed && windSpeed < 8.5)
			{
				pct = 60;
			}
			else if (8.5 <= windSpeed && windSpeed < 9)
			{
				pct = 70;
			}
			else if (9 <= windSpeed && windSpeed < 9.5)
			{
				pct = 80;
			}
			else if (9.5 <= windSpeed && windSpeed < 10.5)
			{
				pct = 90;
			}
			else if (10.5 <= windSpeed && windSpeed < 25.5)
			{
				pct = 100;
			}
			else if (25.5 <= windSpeed && windSpeed < 26)
			{
				pct = 50;
			}
			else if (26 <= windSpeed && windSpeed < 26.5)
			{
				pct = 20;
			}
			else
			{
				pct = 0;
			}

			return pct;
		}

        public OptimisationModel Clone()
        {
            return new OptimisationModel()
            {
                Curve = Curve,
                EmissionFactor = EmissionFactor,
                EmsFuel = EmsFuel,
                GenericOptimizedValue = GenericOptimizedValue,
                GlobalId = GlobalId,
                LinearOptimizedValue = LinearOptimizedValue,
                Managable = Managable,
                MaxPower = MaxPower,
                MeasuredValue = MeasuredValue,
                MinPower = MinPower,
                Price = Price,
                Renewable = Renewable,
                WindPct = WindPct
            };
        }
	}
}