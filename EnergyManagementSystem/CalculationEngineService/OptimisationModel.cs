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
            this.GlobalId = 0;
            this.Price = 0;
            this.MeasuredValue = 0;
            this.linearOptimizedValue = 0;
            this.GenericOptimizedValue = 0;
            this.MinPower = 0;
            this.MaxPower = 0;
            this.Managable = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisationModel" /> class
        /// </summary>
        public OptimisationModel(SynchronousMachine sm, EMSFuel emsf, MeasurementUnit mu)
        {
            this.GlobalId = sm.GlobalId;
            this.Price = emsf.UnitPrice; //izracunati
            this.MeasuredValue = mu.CurrentValue;
            this.linearOptimizedValue = 0; //izracunati
            this.GenericOptimizedValue = 0; //izracunati
            this.MinPower = sm.MinQ;
            this.MaxPower = sm.MaxQ;
            if (sm.Active)
            {
                this.Managable = 1;
            }
            else
            {
                this.Managable = 0;
            }
        }
    }
}
