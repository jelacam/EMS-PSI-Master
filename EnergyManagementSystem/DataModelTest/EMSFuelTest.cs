//-----------------------------------------------------------------------
// <copyright file="EMSFuelTest.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace DataModelTest
{
	using System;
	using System.Collections.Generic;
	using EMS.Common;
	using EMS.Services.NetworkModelService.DataModel.Core;
	using EMS.Services.NetworkModelService.DataModel.Production;
	using NUnit.Framework;

	/// <summary>
	/// Class for unit testing EMSFuel
	/// </summary>
	[TestFixture]
	public class EMSFuelTest
	{
		/// <summary>
		/// Instance of EMSFuel
		/// </summary>
		private EMSFuel emsf1;

		/// <summary>
		/// Instance of EMSFuel
		/// </summary>
		private EMSFuel emsf2;

		/// <summary>
		/// Instance of EMSFuel
		/// </summary>
		private EMSFuel emsf3;

		/// <summary>
		/// Instance of EMSFuel
		/// </summary>
		private EMSFuel emsf4;

		/// <summary>
		/// Instance of EMSFuel
		/// </summary>
		private EMSFuel emsf5;

		/// <summary>
		/// Container for fuelType
		/// </summary>
		private EmsFuelType fuelType1;

		/// <summary>
		/// Container for fuelType
		/// </summary>
		private EmsFuelType fuelType2;

		/// <summary>
		/// Container for unitPrice
		/// </summary>
		private float unitPrice1;

		/// <summary>
		/// Container for unitPrice
		/// </summary>
		private float unitPrice2;

		/// <summary>
		/// Container for globalId
		/// </summary>
		private long globalId1;

		/// <summary>
		/// Container for globalId
		/// </summary>
		private long globalId2;

		/// <summary>
		/// Container for true result
		/// </summary>
		private bool resultT;

		/// <summary>
		/// Container for false result
		/// </summary>
		private bool resultF;

		/// <summary>
		/// SetUp method
		/// </summary>
		[OneTimeSetUp]
		public void SetupTest()
		{
			this.globalId1 = 1623;
			this.globalId2 = 10;
			this.fuelType1 = EmsFuelType.hydro;
			this.fuelType2 = EmsFuelType.oli;
			this.unitPrice1 = 100;
			this.unitPrice2 = 1000;
			this.emsf1 = new EMSFuel(this.globalId1);
			this.emsf2 = new EMSFuel(this.globalId1);
			this.emsf2.FuelType = this.fuelType1;
			this.emsf2.UnitPrice = this.unitPrice1;
			this.emsf3 = new EMSFuel(this.globalId1);
			this.emsf3.FuelType = this.fuelType2;
			this.emsf3.UnitPrice = this.unitPrice2;
			this.emsf4 = new EMSFuel(this.globalId2);
			this.emsf4.FuelType = this.fuelType1;
			this.emsf4.UnitPrice = this.unitPrice1;
			this.emsf5 = null;
		}

		/// <summary>
		/// Unit test for constructor with parameters
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelConstructor")]
		public void Constructor()
		{
			EMSFuel emsf = new EMSFuel(this.globalId1);
			Assert.IsNotNull(emsf);
		}

		/// <summary>
		/// Unit test for EMSFuel FuelType setter
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelFuelTypeProperty")]
		public void FuelTypeProperty()
		{
			this.emsf1.FuelType = this.fuelType1;
			Assert.AreEqual(this.emsf1.FuelType, this.fuelType1);
			Assert.IsNotNull(this.emsf1.FuelType);
		}

		/// <summary>
		/// Unit test for EMSFuel UnitPrice setter
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelUnitPriceProperty")]
		public void UnitPriceProperty()
		{
			this.emsf1.UnitPrice = this.unitPrice1;
			Assert.AreEqual(this.emsf1.UnitPrice, this.unitPrice1);
			Assert.IsNotNull(this.emsf1.UnitPrice);
		}

		/// <summary>
		/// Unit test for EMSFuel Equals method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelEqualsMethod")]
		public void EqualsMethod()
		{
			long m1 = 1;
			long m2 = 2;
			List<long> l = new List<long>();
			l.Add(m1);
			l.Add(m2);
			this.emsf1.SynchronousMachines = l;
			this.emsf1.FuelType = this.fuelType1;
			this.emsf1.UnitPrice = this.unitPrice1;
			this.emsf2.SynchronousMachines = l;
			this.emsf3.SynchronousMachines = null;
			this.emsf4.SynchronousMachines = null;
			this.resultT = this.emsf1.Equals(this.emsf2);
			Assert.IsTrue(this.resultT);
			this.resultF = this.emsf1.Equals(this.emsf3);
			Assert.IsFalse(this.resultF);
			this.resultF = this.emsf1.Equals(this.emsf4);
			Assert.IsFalse(this.resultF);
			this.resultF = this.emsf1.Equals(this.emsf5);
			Assert.IsFalse(this.resultF);
		}

		/// <summary>
		/// Unit test for EMSFuel GetHashCode method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelGetHashCodeMethod")]
		public void GetHashCodeMethod()
		{
			int result = this.emsf1.GetHashCode();
			Assert.IsNotNull(result);
		}

		/// <summary>
		/// Unit test for EMSFuel HasProperty method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelHasPropertyMethod")]
		public void HasPropertyMethod()
		{
			this.resultT = this.emsf1.HasProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
			Assert.IsTrue(this.resultT);
			this.resultT = this.emsf1.HasProperty(ModelCode.EMSFUEL_FUELTYPE);
			Assert.IsTrue(this.resultT);
			this.resultT = this.emsf1.HasProperty(ModelCode.EMSFUEL_UNITPRICE);
			Assert.IsTrue(this.resultT);
			this.resultT = this.emsf1.HasProperty(ModelCode.EMSFUEL_SYNCHRONOUSMACHINES);
			Assert.IsTrue(this.resultT);
			this.resultF = this.emsf1.HasProperty(ModelCode.EMSFUEL);
			Assert.IsFalse(this.resultF);
		}

		/// <summary>
		/// Unit test for EMSFuel GetProperty method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelGetPropertyMethod")]
		public void GetPropertyMethod()
		{
			this.emsf1.GetProperty(ModelCode.IDENTIFIEDOBJECT_MRID);
			Assert.IsNotNull(this.emsf1.Mrid);
			this.emsf1.GetProperty(ModelCode.EMSFUEL_FUELTYPE);
			Assert.IsNotNull(this.emsf1.FuelType);
			this.emsf1.GetProperty(ModelCode.EMSFUEL_UNITPRICE);
			Assert.IsNotNull(this.emsf1.UnitPrice);
			this.emsf1.GetProperty(ModelCode.EMSFUEL_SYNCHRONOUSMACHINES);
			Assert.IsNotNull(this.emsf1.SynchronousMachines);
		}

		/// <summary>
		/// Unit test for EMSFuel SetProperty method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelSetPropertyMethod")]
		public void SetPropertyMethod()
		{
			this.emsf1.SetProperty(new Property(ModelCode.IDENTIFIEDOBJECT_MRID));
			Assert.IsNotNull(this.emsf1.Mrid);
			this.emsf1.SetProperty(new Property(ModelCode.EMSFUEL_FUELTYPE));
			Assert.IsNotNull(this.emsf1.FuelType);
			this.emsf1.SetProperty(new Property(ModelCode.EMSFUEL_UNITPRICE));
			Assert.IsNotNull(this.emsf1.UnitPrice);
		}

		/// <summary>
		/// Unit test for EMSFuel IsReferenced
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelIsReferencedMethod")]
		public void IsReferencedMethod()
		{
			this.emsf1.SynchronousMachines.RemoveRange(0, this.emsf1.SynchronousMachines.Count);
			this.resultF = this.emsf1.IsReferenced;
			Assert.IsFalse(this.resultF);
			this.emsf1.SynchronousMachines.Add(1);
			this.resultT = this.emsf1.IsReferenced;
			Assert.IsTrue(this.resultT);
		}

		/// <summary>
		/// Unit test for EMSFuel GetReferences method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelGetReferencesMethod")]
		public void GetReferencesMethod()
		{
			long m1 = 1;
			long m2 = 2;
			List<long> l = new List<long>();
			l.Add(m1);
			l.Add(m2);
			this.emsf1.SynchronousMachines = l;
			Dictionary<ModelCode, List<long>> d = new Dictionary<ModelCode, List<long>>();
			this.emsf1.GetReferences(d, TypeOfReference.Target);
			this.emsf1.GetReferences(d, TypeOfReference.Both);
		}

		/// <summary>
		/// Unit test for EMSFuel AddReference method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelAddReferenceMethod")]
		public void AddReferenceMethod()
		{
			this.emsf1.AddReference(ModelCode.SYNCHRONOUSMACHINE_FUEL, 1);
			Assert.IsNotNull(this.emsf1.SynchronousMachines);
			Assert.Throws<Exception>(() => this.emsf1.AddReference(ModelCode.EMSFUEL, 1));
		}

		/// <summary>
		/// Unit test for EMSFuel RemoveReference method
		/// </summary>
		[Test]
		[TestCase(TestName = "EMSFuelRemoveReferenceMethod")]
		public void RemoveReferenceMethod()
		{
			this.emsf1.RemoveReference(ModelCode.SYNCHRONOUSMACHINE_FUEL, 1);
			this.emsf1.AddReference(ModelCode.SYNCHRONOUSMACHINE_FUEL, 2);
			this.emsf1.RemoveReference(ModelCode.SYNCHRONOUSMACHINE_FUEL, 2);
			Assert.Throws<ModelException>(() => this.emsf1.RemoveReference(ModelCode.EMSFUEL, 1));
		}
	}
}