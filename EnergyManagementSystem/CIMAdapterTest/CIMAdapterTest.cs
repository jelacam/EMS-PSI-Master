using EMS.CIMAdapter;
using EMS.Common;
using EMS.ServiceContracts;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIMAdapterTest
{
    [TestFixture]
    public class CIMAdapterTest
    {
        #region Declarations

        private NetworkModelGDAProxy GdaQueryProxy;
        private CIMAdapter cimAdapterUnderTest;
        private FileStream stream;
        private string textBoxCimFile = string.Empty;
        private string log;
        private Delta nmsDelta;

        #endregion Declarations

        #region Setup

        [OneTimeSetUp]
        public void SetupTest()
        {
            cimAdapterUnderTest = new CIMAdapter();

            GdaQueryProxy = Substitute.For<NetworkModelGDAProxy>("NetworkModelGDAEndpoint");
            GdaQueryProxy.ApplyUpdate(nmsDelta).Returns(new UpdateResult()
            {
                Message = "OK"
            });

            textBoxCimFile = string.Format(@"C:\Users\jelac\OneDrive\PSI_Master\EMS-Team-Repo\EnergyManagementSystem\Resources\EmsProjectXML.xml");

            nmsDelta = null;
            log = string.Empty;
        }

        #endregion Setup

        #region Tests

        [Test]
        [TestCase(TestName = "CreateDelta")]
        [Ignore("Ucitavanje assembly failuje")]
        public void CreateDeltaTest()
        {
            using (stream = File.Open(textBoxCimFile, FileMode.Open))
            {
                nmsDelta = cimAdapterUnderTest.CreateDelta(stream, EMS.CIMAdapter.Manager.SupportedProfiles.EMSData, out log);

                Assert.NotNull(nmsDelta);
            }
        }

        [Test]
        [TestCase(TestName = "ApplyUpdates")]
        public void ApplyUpdatesTest()
        {
            string updateResult = cimAdapterUnderTest.ApplyUpdates(nmsDelta).ToString();
            Assert.NotNull(updateResult);
            Assert.IsNotEmpty(updateResult);
        }

        #endregion Tests
    }
}