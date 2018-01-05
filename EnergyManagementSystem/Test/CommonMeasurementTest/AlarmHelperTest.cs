//-----------------------------------------------------------------------
// <copyright file="AlarmHelperTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace CommonMeasurementTest
{
	using EMS.CommonMeasurement;
	using NUnit.Framework;
	using System;

	/// <summary>
	/// Class for unit testing AlarmHelper
	/// </summary>
	[TestFixture]
	public class AlarmHelperTest
	{
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
			this.ah = new AlarmHelper();
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "AlarmHelperConstructor")]
		public void Constructor()
		{
			AlarmHelper alarmHelper = new AlarmHelper();
			Assert.IsNotNull(alarmHelper);
		}

		/// <summary>
		/// Unit test for constructor with parameters
		/// </summary>
		[Test]
		[TestCase(1623, 50, 0, 100, TestName = "AlarmHelperConstructor2")]
		public void ConstructorWithParameters(long gid, float value, float minValue, float maxValue)
		{
			AlarmHelper alarmHelper = new AlarmHelper(gid, value, minValue, maxValue, DateTime.Now);
			Assert.IsNotNull(alarmHelper);
		}

		/// <summary>
		/// Unit test for AlarmHelper Gid property
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "AlarmHelperGidProperty")]
		public void GidProperty(long gid)
		{
			this.ah.Gid = gid;
			Assert.AreEqual(this.ah.Gid, gid);
		}

		/// <summary>
		/// Unit test for AlarmHelper Value property
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "AlarmHelperValueProperty")]
		public void ValueProperty(float value)
		{
			this.ah.Value = value;
			Assert.AreEqual(this.ah.Value, value);
		}

		/// <summary>
		/// Unit test for AlarmHelper MinValue property
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "AlarmHelperMinValueProperty")]
		public void MinValueProperty(float minValue)
		{
			this.ah.MinValue = minValue;
			Assert.AreEqual(this.ah.MinValue, minValue);
		}

		/// <summary>
		/// Unit test for AlarmHelper MaxValue property
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "AlarmHelperMaxValueProperty")]
		public void MaxValueProperty(float maxValue)
		{
			this.ah.MaxValue = maxValue;
			Assert.AreEqual(this.ah.MaxValue, maxValue);
		}

		/// <summary>
		/// Unit test for AlarmHelper TimeStamp property
		/// </summary>
		[Test]
		[TestCase(TestName = "AlarmHelperTimeStampProperty")]
		public void TimeStampProperty()
		{
			DateTime time = DateTime.Now;
			this.ah.TimeStamp = time;
			Assert.AreEqual(this.ah.TimeStamp, time);
		}

		/// <summary>
		/// Unit test for AlarmHelper Type property
		/// </summary>
		[Test]
		[TestCase(AlarmType.eguMax, TestName = "AlarmHelperTypeProperty")]
		public void TypeProperty(AlarmType type)
		{
			this.ah.Type = type;
			Assert.AreEqual(this.ah.Type, type);
		}

		/// <summary>
		/// Unit test for AlarmHelper Message property
		/// </summary>
		[Test]
		[TestCase("message", TestName = "AlarmHelperMessageProperty")]
		public void MessageProperty(string message)
		{
			this.ah.Message = message;
			Assert.AreEqual(this.ah.Message, message);
		}
	}
}