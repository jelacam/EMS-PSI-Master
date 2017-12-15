//-----------------------------------------------------------------------
// <copyright file="AnalogLocationTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SCADACrunchingServiceTest
{
	using EMS.Services.NetworkModelService.DataModel.Meas;
	using EMS.Services.SCADACrunchingService;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing AnalogLocation
	/// </summary>
	[TestFixture]
	public class AnalogLocationTest
	{
		/// <summary>
		/// Unit test for constructor without parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "AnalogLocationConstructor")]
		public void Constructor()
		{
			AnalogLocation al = new AnalogLocation();
			Assert.IsNotNull(al);
		}

		/// <summary>
		/// Unit test for AnalogLocation Analog setter
		/// </summary>
		[Test]
		[TestCase(TestName = "AnalogLocationAnalogProperty")]
		public void AnalogProperty()
		{
			AnalogLocation al = new AnalogLocation();
			al.Analog = new Analog(1623);
			Assert.IsNotNull(al.Analog);
		}

		/// <summary>
		/// Unit test for AnalogLocation StartAddress setter
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "AnalogLocationStartAddressProperty")]
		public void StartAddressProperty(int startAddress)
		{
			AnalogLocation al = new AnalogLocation();
			al.StartAddress = startAddress;
			Assert.AreEqual(al.StartAddress, startAddress);
		}

		/// <summary>
		/// Unit test for AnalogLocation Length setter
		/// </summary>
		[Test]
		[TestCase(1623, TestName = "AnalogLocationLengthProperty")]
		public void LengthProperty(int length)
		{
			AnalogLocation al = new AnalogLocation();
			al.Length = length;
			Assert.AreEqual(al.Length, length);
		}
	}
}
