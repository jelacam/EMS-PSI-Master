//-----------------------------------------------------------------------
// <copyright file="AnalogLocationTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACommandingTest
{
	using EMS.Services.NetworkModelService.DataModel.Meas;
	using EMS.Services.SCADACommandingService;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing CMDAnalogLocation
	/// </summary>
	[TestFixture]
    public class CMDAnalogLocationTest
    {
        /// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
        [TestCase(TestName = "CMDAnalogLocationConstructor")]
        public void Constructor()
        {
            CMDAnalogLocation al = new CMDAnalogLocation();
            Assert.IsNotNull(al);
        }

		/// <summary>
		/// Unit test for CMDAnalogLocation Analog setter
		/// </summary>
		[Test]
		[TestCase(TestName = "CMDAnalogLocationAnalogProperty")]
		public void AnalogProperty()
		{
			CMDAnalogLocation al = new CMDAnalogLocation();
			al.Analog = new Analog(1623);
			Assert.IsNotNull(al.Analog);
		}

		/// <summary>
		/// Unit test for CMDAnalogLocation StartAddress setter
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "CMDAnalogLocationStartAddressProperty")]
		public void StartAddressProperty(int startAddress)
		{
			CMDAnalogLocation al = new CMDAnalogLocation();
			al.StartAddress = startAddress;
			Assert.AreEqual(al.StartAddress, startAddress);
		}

		/// <summary>
		/// Unit test for CMDAnalogLocation Length setter
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "CMDAnalogLocationLengthProperty")]
		public void LengthProperty(int length)
		{
			CMDAnalogLocation al = new CMDAnalogLocation();
			al.Length = length;
			Assert.AreEqual(al.Length, length);
		}
	}
}
