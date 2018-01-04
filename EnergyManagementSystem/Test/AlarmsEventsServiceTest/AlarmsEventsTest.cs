//-----------------------------------------------------------------------
// <copyright file="AlarmsEventsTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace AlarmsEventsServiceTest
{
	using System;
	using EMS.CommonMeasurement;
	using EMS.Services.AlarmsEventsService;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing AlarmsEvents
	/// </summary>
	[TestFixture]
	public class AlarmsEventsTest
	{
		/// <summary>
		/// Instance of AlarmsEvents
		/// </summary>
		private AlarmsEvents ae;

		/// <summary>
		/// Instance of AlarmHelper
		/// </summary>
		private AlarmHelper ah;

		/// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
		public void SetupTest()
		{
			this.ae = new AlarmsEvents();
			this.ah = new AlarmHelper(1623, 50, 0, 100, DateTime.Now);
			this.ah.Message = "message";
			this.ah.Type = AlarmType.eguMax;
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "AlarmsEventsConstructor")]
		public void Constructor()
		{
			AlarmsEvents alarmse = new AlarmsEvents();
			Assert.IsNotNull(alarmse);
		}

		/// <summary>
		/// Unit test for AlarmsEvents Alarms property
		/// </summary>
		[Test]
		[TestCase(TestName = "AlarmsEventsAlarmsProperty")]
		public void AlarmsProperty()
		{
			Assert.IsNotNull(this.ae.Alarms);
		}

		/// <summary>
		/// Unit test for AlarmsEvents Publisher property
		/// </summary>
		[Test]
		[TestCase(TestName = "AlarmsEventsPublisherProperty")]
		public void PublisherProperty()
		{
			Assert.IsNotNull(this.ae.Publisher);
		}

		/// <summary>
		/// Unit test for AlarmsEvents AddAlarm method
		/// </summary>
		[Test]
		[TestCase(TestName = "AlarmsEventsAddAlarmMethod")]
		public void AddAlarmMethod()
		{
			this.ae.AddAlarm(this.ah);
		}
	}
}