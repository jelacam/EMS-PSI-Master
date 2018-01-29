using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using EMS.Services.AlarmsEventsService.PubSub;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmsEventsServiceTest
{
    [TestFixture]
    public class PublisherServiceTest
    {
        #region Declarations
        private PublisherService publisherUnderTest;
        private AlarmHelper alarmHelper;
        private PublishingStatus status;
        #endregion 

        [OneTimeSetUp]
        public void SetupTest()
        {
            publisherUnderTest = new PublisherService();
            alarmHelper = new AlarmHelper();
            status = PublishingStatus.INSERT;
        }

        #region Tests

        [Test]
        [TestCase(TestName = "PublisherConstructor")]
        public void ConstructorTest()
        {
            publisherUnderTest = new PublisherService();
            Assert.NotNull(publisherUnderTest);
        }


        [Test]
        [TestCase(TestName = "PublishAlarmsEvents")]
        public void PublishAlarmsEventsTest()
        {
            publisherUnderTest.PublishAlarmsEvents(alarmHelper, status);
        }

        [Test]
        [TestCase(TestName = "TestPublishAlarmEvents")]
        public void TestPusblishAlarmsEvents()
        {
            Assert.DoesNotThrow(PublishAlarmsEventsTest);
        }

        #endregion


    }
}
