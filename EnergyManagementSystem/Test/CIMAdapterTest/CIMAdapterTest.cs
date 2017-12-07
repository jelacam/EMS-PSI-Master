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

      
        private CIMAdapter cimAdapterUnderTest;
        private FileStream stream;
        private string textBoxCimFile = string.Empty;
        private string log;
        private Delta nmsDeltaNull;
        private Delta nmsDelta;
        private ResourceDescription rd;

        #endregion Declarations

        #region Setup

        [OneTimeSetUp]
        public void SetupTest()
        {
            cimAdapterUnderTest = new CIMAdapter();

            NetworkModelGDAProxy.Instance = Substitute.For<INetworkModelGDAContract>();
            NetworkModelGDAProxy.Instance.ApplyUpdate(null).ReturnsForAnyArgs(new UpdateResult()
            {
                Message = "OK"
            });
            NetworkModelGDAProxy.Instance.ApplyUpdate(null).ReturnsForAnyArgs(new UpdateResult()
            {
                Message = "OK"
            });

            textBoxCimFile = string.Format(@"C:\Users\jelac\OneDrive\PSI_Master\EMS-Team-Repo\EnergyManagementSystem\Resources\EmsProjectXML.xml");

            nmsDeltaNull = null;

            nmsDelta = new Delta();

            log = string.Empty;

            rd = new ResourceDescription(1, new List<Property>() {
                                            new Property(ModelCode.ANALOG_MAXVALUE)
            });
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
                nmsDeltaNull = cimAdapterUnderTest.CreateDelta(stream, EMS.CIMAdapter.Manager.SupportedProfiles.EMSData, out log);

                Assert.NotNull(nmsDeltaNull);
            }
        }

        [Test]
        [TestCase(TestName = "ApplyUpdatesDeltaNotNull")]
        public void ApplyUpdatesDeltaNotNullTest()
        {
            string updateResult = cimAdapterUnderTest.ApplyUpdates(nmsDeltaNull).ToString();
            Assert.NotNull(updateResult);
            Assert.IsNotEmpty(updateResult);
        }

        [Test]
        [TestCase(TestName = "ApplyUpdateDeltaOperation")]
        //[Ignore("NetworkGDAProxy puca")]
        public void ApplyUpdateDeltaOperation()
        {
            nmsDelta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
            string updateResult = cimAdapterUnderTest.ApplyUpdates(nmsDelta).ToString();
            Assert.NotNull(updateResult);
            Assert.IsNotEmpty(updateResult);
        }

        #endregion Tests
    }
}