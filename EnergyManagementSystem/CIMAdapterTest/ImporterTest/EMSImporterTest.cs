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

        private TransformAndLoadReport report;
        private ConcreteModel concreteModel;
        private ConcreteModel failConcreteModel;

        #endregion Declarations

        #region Setup

        [OneTimeSetUp]
        public void SetupTest()
        {
            concreteModel = new ConcreteModel();
            failConcreteModel = null;
        }

        #endregion Setup

        #region Tests

        [Test]
        [TestCase(TestName = "CreateNMSDelta")]
        public void CreateNMSDeltaTest()
        {
            report = EMSImporter.Instance.CreateNMSDelta(concreteModel) as TransformAndLoadReport;
            Assert.AreEqual(true, report.Success);
        }

        [Test]
        [TestCase(TestName = "CreateNMSDeltaFail")]
        public void CreateNMSDeltaFailTest()
        {
            report = EMSImporter.Instance.CreateNMSDelta(failConcreteModel) as TransformAndLoadReport;
            Assert.AreEqual(true, report.Success);
        }

        #endregion Tests
    }
}