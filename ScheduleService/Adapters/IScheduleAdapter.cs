using Services.Schedule.Models;

namespace Services.Schedule.Domain.Adapters
{
    public interface IScheduleAdapter
    {
        IList<TimeSlot> GetSchedules(string clientId, string agentId, DateTime searchDate);
        IList<TimeSlot> GetSchedules(string clientId, string agentId, string searchDate);
    }
}
