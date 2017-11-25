using CIM.Model;
using EMS.CIMAdapter;
using EMS.CIMAdapter.Importer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIMAdapterTest.ImporterTest
{
    [TestFixture]
    public class EMSImporterTest
    {
        #region Declarations

        private EMSImporter EMSImporter;
        private TransformAndLoadReport report;
        private ConcreteModel concreteModel;

        #endregion Declarations

        #region Setup

        [OneTimeSetUp]
        public void SetupTest()
        {
            EMSImporter = new EMSImporter();
            concreteModel = new ConcreteModel();
        }

        #endregion Setup

        #region Tests

        [Test]
        [TestCase(TestName = "Reset")]
        public void ResetTest()
        {
            //EMSImporter.Reset();
            //Assert.AreEqual(EMSImporter.Instance.NMSDelta, )
        }

        [Test]
        [TestCase(TestName = "CreateNMSDelta")]
        public void CreateNMSDeltaTest()
        {
            Assert.IsNotNull(EMSImporter.Instance.CreateNMSDelta(concreteModel));
        }

        #endregion Tests
    }
}