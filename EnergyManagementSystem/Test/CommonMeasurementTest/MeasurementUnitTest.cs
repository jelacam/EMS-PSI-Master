//-----------------------------------------------------------------------
// <copyright file="MeasurementUnitTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace CommonMeasurementTest
{
	using EMS.CommonMeasurement;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing MeasurementUnit
	/// </summary>
	[TestFixture]
	public class MeasurementUnitTest
	{
		/// <summary>
		/// Container for gid
		/// </summary>
		private long gid;

		/// <summary>
		/// Container for currentValue
		/// </summary>
		private float currentValue;

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "MeasurementUnitConstructor")]
		public void Constructor()
		{
			MeasurementUnit mu = new MeasurementUnit();
			Assert.IsNotNull(mu);
		}

		/// <summary>
		/// Unit test for MeasurementUnit Gid setter
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "MeasurementUnitGidProperty")]
		public void GidProperty(long gid)
		{
			MeasurementUnit mu = new MeasurementUnit();
			mu.Gid = gid;
			Assert.AreEqual(mu.Gid, gid);
		}

		/// <summary>
		/// Unit test for MeasurementUnit CurrentValue setter
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "MeasurementUnitCurrentValueProperty")]
		public void CurrentValueProperty(long currentValue)
		{
			MeasurementUnit mu = new MeasurementUnit();
			mu.CurrentValue = currentValue;
			Assert.AreEqual(mu.CurrentValue, currentValue);
		}

        /// <summary>
		/// Unit test for MeasurementUnit MinValue setter
		/// </summary>
		[Test]
        [TestCase(1623, TestName = "MeasurementUnitMinValueProperty")]
        public void MinValueProperty(long minValue)
        {
            MeasurementUnit mu = new MeasurementUnit();
            mu.MinValue = minValue;
            Assert.AreEqual(mu.MinValue, minValue);
        }

        /// <summary>
		/// Unit test for MeasurementUnit MaxValue setter
		/// </summary>
		[Test]
        [TestCase(1623, TestName = "MeasurementUnitMaxValueProperty")]
        public void MaxValueProperty(long maxValue)
        {
            MeasurementUnit mu = new MeasurementUnit();
            mu.MaxValue = maxValue;
            Assert.AreEqual(mu.MaxValue, maxValue);
        }
    }
}
