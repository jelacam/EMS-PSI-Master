//-----------------------------------------------------------------------
// <copyright file="ConvertorHelperTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace CommonMeasurementTest
{
	using EMS.CommonMeasurement;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing ConvertorHelper
	/// </summary>
	[TestFixture]
	public class ConvertorHelperTest
	{
		/// <summary>
		/// Instance of ConvertorHelper
		/// </summary>
		private ConvertorHelper ch;

		/// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
		public void SetupTest()
		{
			this.ch = new ConvertorHelper();
		}

		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "ConvertorHelperConstructor")]
		public void Constructor()
		{
			ConvertorHelper convertorHelper = new ConvertorHelper();
			Assert.IsNotNull(convertorHelper);
		}

		/// <summary>
		/// Unit test for ConvertorHelper MinRaw getter
		/// </summary>
		[Test]
		[TestCase(TestName = "ConvertorHelperMinRawProperty")]
		public void MinRawProperty()
		{
			Assert.IsNotNull(this.ch.MinRaw);
		}

		/// <summary>
		/// Unit test for ConvertorHelper MaxRaw getter
		/// </summary>
		[Test]
		[TestCase(TestName = "ConvertorHelperMaxRawProperty")]
		public void MaxRawProperty()
		{
			Assert.IsNotNull(this.ch.MaxRaw);
		}

		/// <summary>
		/// Unit test for ConvertorHelper ConvertFromRawToEGUValue method
		/// </summary>
		[Test]
		[TestCase(1623, 0, 100, TestName = "ConvertorHelperConvertFromRawToEGUValueMethod")]
		public void ConvertFromRawToEGUValueMethod(float value, float minEGU, float maxEGU)
		{
			float retval = this.ch.ConvertFromRawToEGUValue(value, minEGU, maxEGU);
			Assert.IsNotNull(retval);
		}

		/// <summary>
		/// Unit test for ConvertorHelper ConvertFromEGUToRawValue method
		/// </summary>
		[Test]
		[TestCase(10, 0, 100, TestName = "ConvertorHelperConvertFromEGUToRawValueMethod")]
		public void ConvertFromEGUToRawValueMethod(float value, float minEGU, float maxEGU)
		{
			float retval = this.ch.ConvertFromEGUToRawValue(value, minEGU, maxEGU);
			Assert.IsNotNull(retval);
		}
	}
}
