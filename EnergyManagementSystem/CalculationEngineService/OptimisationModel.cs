//-----------------------------------------------------------------------
// <copyright file="OptimisationModel.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
	using System;
	using Common;
	using CommonMeasurement;
	using NetworkModelService.DataModel.Wires;
	using NetworkModelService.DataModel.Production;

	/// <summary>
	/// class for OptimisationModel
	/// </summary>
	public class OptimisationModel
	{
		/// <summary>
		/// globalId for OptimisationModel
		/// </summary>
		public long GlobalId { get; set; }

		/// <summary>
		/// price for OptimisationModel
		/// </summary>
		public float Price { get; set; }

		/// <summary>
		/// measuredValue for OptimisationModel
		/// </summary>
		public float MeasuredValue { get; set; }

		/// <summary>
		/// linearOptimizedValue for OptimisationModel
		/// </summary>
		public float LinearOptimizedValue { get; set; }

		/// <summary>
		/// genericOptimizedValue for OptimisationModel
		/// </summary>
		public float GenericOptimizedValue { get; set; }

		/// <summary>
		/// minPower for OptimisationModel
		/// </summary>
		public float MinPower { get; set; }

		/// <summary>
		/// maxPower for OptimisationModel
		/// </summary>
		public float MaxPower { get; set; }

		/// <summary>
		/// managable for OptimisationModel
		/// </summary>
		public int Managable { get; set; }

		/// <summary>
		/// renewable for OptimisationModel
		/// </summary>
		public bool Renewable { get; set; }

		/// <summary>
		/// windPct for OptimisationModel
		/// </summary>
		public float WindPct { get; set; }

		/// <summary>
		/// emsFuel for OptimisationModel
		/// </summary>
		public EMSFuel EmsFuel { get; private set; }

		public SynchronousMachineCurveModel Curve { get; set; }

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
			Managable = 1;
			Renewable = false;
			WindPct = 1;
			Curve = new SynchronousMachineCurveModel();
			EmsFuel = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OptimisationModel" /> class
		/// </summary>
		public OptimisationModel(SynchronousMachine sm, EMSFuel emsf, MeasurementUnit mu, float windSpeed, SynchronousMachineCurveModel smcm)
		{
			GlobalId = sm.GlobalId;
			MeasuredValue = mu.CurrentValue;
			LinearOptimizedValue = 0; //izracunati
			GenericOptimizedValue = 0; //izracunati			
			EmsFuel = emsf;
			Curve = smcm;		

			Managable = sm.Active ? 1 : 0;
			Renewable = (emsf.FuelType.Equals(EmsFuelType.wind) || emsf.FuelType.Equals(EmsFuelType.solar)) ? true : false;
			Price = Renewable ? 1 : CalculatePrice(MeasuredValue);
			WindPct = emsf.FuelType.Equals(EmsFuelType.wind) ? CalculateWindPct(windSpeed) : 100;

			MinPower = ((Managable == 0) || (Renewable && WindPct == 0)) ? 0 : sm.MinQ;

			if((Managable == 0) || (Renewable && WindPct == 0))
			{
				MaxPower = 0;
			}
			else if(Renewable && WindPct>0)
			{
				MaxPower = ((((sm.MaxQ - sm.MinQ) / 100) * WindPct) + sm.MinQ);
			}
			else
			{
				MaxPower = sm.MaxQ;
			}	
		}

		public float CalculatePrice(float measuredValue)
		{
			float price = 0;
			float amount = (float)Curve.A * measuredValue * measuredValue + (float)Curve.B * measuredValue + (float)Curve.C;
			price = amount * EmsFuel.UnitPrice;
			return price; 
		}

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

	}
}