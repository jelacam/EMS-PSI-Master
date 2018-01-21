using EMS.ServiceContracts;
using EMS.Services.CalculationEngineService.PubSub;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CalculationEngineServiceTest
{
    [TestFixture]
    public class PublisherServiceTest
    {
        #region Declarations

        private PublisherService publisherUnderTest;
        private List<MeasurementUI> resultUnderTest;

        #endregion Declarations

        #region TestSetup

        [OneTimeSetUp]
        public void SetupTest()
        {
            publisherUnderTest = new PublisherService();
            resultUnderTest = new List<MeasurementUI>();
            resultUnderTest.Add(new MeasurementUI()
            {
                AlarmType = "Optimized alarm",
                Gid = 10120310403204,
                CurrentValue = 15f,
                TimeStamp = DateTime.Now
            });
        }

        #endregion TestSetup

        [Test]
        [TestCase(TestName = "PublisherConstructorTest")]
        public void ContructorTest()
        {
            publisherUnderTest = new PublisherService();
            Assert.NotNull(publisherUnderTest);
        }

        [Test]
        [TestCase(TestName = "PublishTest")]
        public void PublishOptimizationResultsTest()
        {
            publisherUnderTest.PublishOptimizationResults(resultUnderTest);
        }

        [Test]
        [TestCase(TestName = "PublishDoesNotThrow")]
        public void PublishOptimizationResultsDoesNotThrow()
        {
            Assert.DoesNotThrow(PublishOptimizationResultsTest);
        }
    }
}