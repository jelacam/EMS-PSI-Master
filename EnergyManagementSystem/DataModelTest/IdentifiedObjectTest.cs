//-----------------------------------------------------------------------
// <copyright file="IdentifiedObjectTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
	using EMS.Services.NetworkModelService.DataModel.Core;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing IdentifiedObject
	/// </summary>
	[TestFixture]
	public class IdentifiedObjectTest
	{
		/// <summary>
		/// Unit test for constructor with parameters
		/// </summary>
		/// <param name="globalId">globalId for the constructor</param>
		[Test]
		[TestCase(1623)]
		public void Constructor(long globalId)
		{
			IdentifiedObject io = new IdentifiedObject(globalId);
			Assert.IsNotNull(io);
		}

		/// <summary>
		/// Unit test for IdentifiedObject GlobalId setter
		/// </summary>
		/// <param name="globalId">globalId property being set and asserted</param>
		[Test]
		[TestCase(10)]
		public void GlobalIdPropertySet(long globalId)
		{
			IdentifiedObject io = new IdentifiedObject(1623);
			io.GlobalId = globalId;
			Assert.AreEqual(io.GlobalId, globalId);
		}

		/// <summary>
		/// Unit test for IdentifiedObject Name setter
		/// </summary>
		/// <param name="name">name property being set and asserted</param>
		[Test]
		[TestCase("name")]
		public void NamePropertySet(string name)
		{
			IdentifiedObject io = new IdentifiedObject(1623);
			io.Name = name;
			Assert.AreEqual(io.Name, name);
			Assert.IsNotEmpty(io.Name);
			Assert.IsNotNull(io.Name);
		}

		/// <summary>
		/// Unit test for IdentifiedObject Mrid setter
		/// </summary>
		/// <param name="mrid">mrid property being set and asserted</param>
		[Test]
		[TestCase("mrid")]
		public void MridPRopertySet(string mrid)
		{
			IdentifiedObject io = new IdentifiedObject(1623);
			io.Mrid = mrid;
			Assert.AreEqual(io.Mrid, mrid);
			Assert.IsNotEmpty(io.Mrid);
			Assert.IsNotNull(io.Mrid);
		}
	}
}
