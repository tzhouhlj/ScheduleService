using Services.KeywordLookup.Domain.Adapters;
using Services.Schedule.Context;
using Services.Schedule.Domain.Adapters;
using Services.Schedule.Models;
using Services.Schedule.Services;

namespace ScheduleServiceTestProject
{
    public class ScheduleDomainServiceTests
    {
        private IScheduleService scheduleDomainService;
        private IScheduleAdapter scheduleAdapter;

        [SetUp]
        public void Setup()
        {
            scheduleAdapter = new FollowUpBossAdapter();
            scheduleDomainService = new ScheduleDomainService(scheduleAdapter);
        }

        [Test]
        public void CheckTestWhenNoAvailable()
        {
            IRequestContext requestContext = new RequestContext("TestClient", "BusyAgent1");
            var available = scheduleDomainService.Check(requestContext, new DateTime(2025, 1, 1, 8, 4, 0), new DateTime(2025, 1, 1, 9, 4, 0));
            Assert.IsFalse(available);
        }        
        
        [Test]
        public void CheckTestWhenAvailable()
        {
            IRequestContext requestContext = new RequestContext("TestClient", "BusyAgent1");
            var available = scheduleDomainService.Check(requestContext, new DateTime(2025, 1, 1, 9, 0, 0), new DateTime(2025, 1, 1, 10, 0, 0));
            Assert.IsTrue(available);
        }

        [Test]
        public void QueryAvailableSlotsWhenAvailable()
        {
            IRequestContext requestContext = new RequestContext("TestClient", "BusyAgent1");
            var timeSlots = new List<TimeSlot>();
            var timeSlot1 = new TimeSlot(new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc));
            var timeSlot2 = new TimeSlot(new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 16, 0, 0, DateTimeKind.Utc));
            timeSlots.Add(timeSlot1);
            timeSlots.Add(timeSlot2);


            var availableTimes = scheduleDomainService.QueryAvailableTimes(requestContext, timeSlots, 5, 30);
            Assert.IsTrue(availableTimes.Count() == 5);
        }



        [Test]
        public void WorkLoadTestWhenBusy()
        {
            IRequestContext requestContext = new RequestContext("TestClient", "BusyAgent1");
            var workLoad = scheduleDomainService.CheckWorkLoad(requestContext, new DateTime(2025, 1, 1));
            Assert.IsTrue(workLoad.Busy);
            Assert.IsTrue(workLoad.AvailableTimeSlots.Count == 3);
        }

        [Test]
        public void WorkLoadTestWhenNotBusy()
        {
            IRequestContext requestContext = new RequestContext("TestClient", "NotBusyAgent1");
            var workLoad = scheduleDomainService.CheckWorkLoad(requestContext, new DateTime(2025, 1, 1));
            Assert.IsFalse(workLoad.Busy);
            Assert.IsTrue(workLoad.AvailableTimeSlots.Count == 1);
        }
    }
}