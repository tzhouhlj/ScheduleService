using Services.Schedule.Domain.Adapters;
using Services.Schedule.Models;

namespace Services.KeywordLookup.Domain.Adapters
{
    public class FollowUpBossAdapter : IScheduleAdapter
    {
        //Test Data 
        private Dictionary<string, Dictionary<string, List<TimeSlot>>> testData; //{agentId: {Date: [TimeSlot]}}

        public FollowUpBossAdapter()
        {
            BuildTestData();
        }

        //Assuming start/end the same date
        //Assuming to the minute accuracy        
        public IList<TimeSlot> GetSchedules(string clientId, string agentId, DateTime searchDate)
        {
            var date = searchDate.ToShortDateString();
            if (testData.ContainsKey(agentId) && testData[agentId].ContainsKey(date))
            {
                return testData[agentId][date];
            }

            return Enumerable.Empty<TimeSlot>().ToList();
        }

        public IList<TimeSlot> GetSchedules(string clientId, string agentId, string searchDate)
        {
            if (testData.ContainsKey(agentId) && testData[agentId].ContainsKey(searchDate))
            {
                return testData[agentId][searchDate]?.OrderBy(t => t.Start)?.ToList();
            }

            return Enumerable.Empty<TimeSlot>().ToList();
        }

    
        private void BuildTestData()
        {
            testData = new Dictionary<string, Dictionary<string, List<TimeSlot>>>
            {
                { "BusyAgent1", new Dictionary<string, List<TimeSlot>>() },
                { "NotBusyAgent1", new Dictionary<string, List<TimeSlot>>() }
            };
            testData["BusyAgent1"].Add("1/1/2025", new List<TimeSlot>());
            var timeSlot1 = new TimeSlot(new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 9, 0, 0, DateTimeKind.Utc));
            var timeSlot2 = new TimeSlot(new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc));
            var timeSlot3 = new TimeSlot(new DateTime(2025, 1, 1, 13, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 15, 0, 0, DateTimeKind.Utc));
            testData["BusyAgent1"]["1/1/2025"].Add(timeSlot1);
            testData["BusyAgent1"]["1/1/2025"].Add(timeSlot2);
            testData["BusyAgent1"]["1/1/2025"].Add(timeSlot3);

            testData["NotBusyAgent1"].Add("1/1/2025", new List<TimeSlot>());
            var timeSlot4 = new TimeSlot(new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 9, 0, 0, DateTimeKind.Utc));
            testData["NotBusyAgent1"]["1/1/2025"].Add(timeSlot1);
        }
    }
}
