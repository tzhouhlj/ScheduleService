using Services.Schedule.Context;
using Services.Schedule.Models;

namespace Services.Schedule.Services;

public interface IScheduleService
{ 
    bool Check(IRequestContext requestContext, DateTime start, DateTime end);
    IEnumerable<TimeSlot> QueryAvailableTimes(IRequestContext requestContext, List<TimeSlot> timeSlots, int n, int duration);
    WorkLoadResponse CheckWorkLoad(IRequestContext requestContext, DateTime date);
}