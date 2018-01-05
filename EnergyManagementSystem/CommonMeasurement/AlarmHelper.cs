//-----------------------------------------------------------------------
// <copyright file="AlarmHelper.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.CommonMeasurement
{
	using System;

	/// <summary>
	/// Class for alarms
	/// </summary>
	public class AlarmHelper
	{
		/// <summary>
		/// stores gid of the entity
		/// </summary>
		private long gid;

		/// <summary>
		/// stores value of the entity
		/// </summary>
		private float value;

		/// <summary>
		/// stores minValue of the entity
		/// </summary>
		private float minValue;

		/// <summary>
		/// stores maxValue of the entity
		/// </summary>
		private float maxValue;

		/// <summary>
		/// stores timeStamp of the entity
		/// </summary>
		private DateTime timeStamp;

		/// <summary>
		/// stores type of the entity
		/// </summary>
		private AlarmType type;

		/// <summary>
		/// stores the message
		/// </summary>
		private string message;

		/// <summary>
		/// Initializes a new instance of the <see cref="AlarmHelper" /> class
		/// </summary>
		public AlarmHelper()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AlarmHelper" /> class
		/// </summary>
		public AlarmHelper(long gid, float value, float minValue, float maxValue, DateTime timeStamp)
		{
			this.gid = gid;
			this.value = value;
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.timeStamp = timeStamp;
			this.message = "";
		}

		/// <summary>
		/// Gets or sets Gid of the entity
		/// </summary>
		public long Gid
		{
			get
			{
				return this.gid;
			}

			set
			{
				this.gid = value;
			}
		}

		public float Value
		{
			get
			{
				return this.value;
			}

			set
			{
				this.value = value;
			}
		}

		public float MinValue
		{
			get
			{
				return this.minValue;
			}

			set
			{
				this.minValue = value;
			}
		}

		public float MaxValue
		{
			get
			{
				return this.maxValue;
			}

			set
			{
				this.maxValue = value;
			}
		}

		public DateTime TimeStamp
		{
			get
			{
				return this.timeStamp;
			}

			set
			{
				this.timeStamp = value;
			}
		}

		public AlarmType Type
		{
			get
			{
				return this.type;
			}

			set
			{
				this.type = value;
			}
		}

		public string Message
		{
			get
			{
				return this.message;
			}

			set
			{
				this.message = value;
			}
		}
	}
}