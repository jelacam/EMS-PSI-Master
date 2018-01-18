//-----------------------------------------------------------------------
// <copyright file="OptimisationModel.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
	using System;
	using CommonMeasurement;
	using NetworkModelService.DataModel.Wires;
	using NetworkModelService.DataModel.Production;
	using Common;

	/// <summary>
	/// class for OptimisationModel
	/// </summary>
	public class OptimisationModel
    {
        /// <summary>
        /// globalId for OptimisationModel
        /// </summary>
        private long globalId;

        /// <summary>
        /// price for OptimisationModel
        /// </summary>
        private float price;

        /// <summary>
        /// measuredValue for OptimisationModel
        /// </summary>
        private float measuredValue;

        /// <summary>
        /// linearOptimizedValue for OptimisationModel
        /// </summary>
        private float linearOptimizedValue;

        /// <summary>
        /// genericOptimizedValue for OptimisationModel
        /// </summary>
        private float genericOptimizedValue;

        /// <summary>
        /// minPower for OptimisationModel
        /// </summary>
        private float minPower;

        /// <summary>
        /// maxPower for OptimisationModel
        /// </summary>
        private float maxPower;

        /// <summary>
        /// managable for OptimisationModel
        /// </summary>
        private int managable;

        /// <summary>
        /// Gets or sets GlobalId of the entity
        /// </summary>
        public long GlobalId
        {
            get
            {
                return this.globalId;
            }

            set
            {
                this.globalId = value;
            }
        }

        /// <summary>
        /// Gets or sets Price of the entity
        /// </summary>
        public float Price
        {
            get
            {
                return this.price;
            }

            set
            {
                this.price = value;
            }
        }

        /// <summary>
        /// Gets or sets MeasuredValue of the entity
        /// </summary>
        public float MeasuredValue
        {
            get
            {
                return this.measuredValue;
            }

            set
            {
                this.measuredValue = value;
            }
        }

        /// <summary>
        /// Gets or sets MinPower of the entity
        /// </summary>
        public float MinPower
        {
            get
            {
                return this.minPower;
            }

            set
            {
                this.minPower = value;
            }
        }

        /// <summary>
        /// Gets or sets MaxPower of the entity
        /// </summary>
        public float MaxPower
        {
            get
            {
                return this.maxPower;
            }

            set
            {
                this.maxPower = value;
            }
        }

        /// <summary>
        /// Gets or sets Managable of the entity
        /// </summary>
        public int Managable
        {
            get
            {
                return this.managable;
            }

            set
            {
                this.managable = value;
            }
        }

        /// <summary>
        /// Gets or sets LinearOptimizedValue of the entity
        /// </summary>
        public float LinearOptimizedValue
        {
            get
            {
                return this.linearOptimizedValue;
            }

            set
            {
                this.linearOptimizedValue = value;
            }
        }

        /// <summary>
        /// Gets or sets GenericOptimizedValue of the entity
        /// </summary>
        public float GenericOptimizedValue
        {
            get
            {
                return this.genericOptimizedValue;
            }

            set
            {
                this.genericOptimizedValue = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisationModel" /> class
        /// </summary>
        public OptimisationModel()
        {
            globalId = 0;
            price = 0;
            measuredValue = 0;
            linearOptimizedValue = 0;
            genericOptimizedValue = 0;
            minPower = 0;
            maxPower = 0;
            managable = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisationModel" /> class
        /// </summary>
        public OptimisationModel(SynchronousMachine sm, EMSFuel emsf, MeasurementUnit mu)
        {
			globalId = sm.GlobalId;
			if (emsf.FuelType.Equals(EmsFuelType.wind))
			{
				price = CalculateWindPrice();
			}
			else
			{
				price = CalculatePrice(emsf, mu.CurrentValue, sm.MinQ, sm.MaxQ);
			}
			measuredValue = mu.CurrentValue;
            linearOptimizedValue = 0; //izracunati
            genericOptimizedValue = 0; //izracunati
            minPower = sm.MinQ;
            maxPower = sm.MaxQ;
            if (sm.Active)
            {
                managable = 1;
            }
            else
            {
                managable = 0;
            }
        }

		public float CalculatePrice(EMSFuel emsf, float measuredValue, float minValue, float maxValue)
		{
			float price = 0;
			float pct = 0;

			pct = ((measuredValue - minValue) / (maxValue - minValue)) * 100;

			if (pct < 10)
			{
				price = 1;
			}
			else if (10 <= pct && pct < 20)
			{
				price = 2;
			}
			else if (20 <= pct && pct < 30)
			{
				price = 2;
			}
			else if (30 <= pct && pct < 40)
			{
				price = 3;
			}
			else if (40 <= pct && pct < 50)
			{
				price = 4;
			}
			else if (50 <= pct && pct < 60)
			{
				price = 5;
			}
			else if (60 <= pct && pct < 70)
			{
				price = 7;
			}
			else if (70 <= pct && pct < 80)
			{
				price = 7;
			}
			else if (80 <= pct && pct < 90)
			{
				price = 8;
			}
			else if (90 <= pct && pct < 100)
			{
				price = 9;
			}
			else
			{
				price = 10;
			}

			price *= emsf.UnitPrice;
			return price;
		}

		public float CalculateWindPrice()
		{
			float price = 1;

			return price;
		}
	}
}
