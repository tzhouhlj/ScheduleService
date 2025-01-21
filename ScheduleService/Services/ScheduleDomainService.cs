using Services.Schedule.Context;
using Services.Schedule.Domain.Adapters;
using Services.Schedule.Models;

namespace Services.Schedule.Services
{
    public class ScheduleDomainService : IScheduleService
    {
        private readonly IScheduleAdapter _scheduleAdapter;

        //now use 4 hours as a rule to determine busy or not
        //can be configurable in the production code
        private const int STANDARD_WORKING_DAY_IN_MINUTES = 4 * 60;

        public ScheduleDomainService(IScheduleAdapter scheduleAdapter)
        {
            _scheduleAdapter = scheduleAdapter;
        }

        bool IScheduleService.Check(IRequestContext requestContext, DateTime start, DateTime end)
        {
            var timeSlots = _scheduleAdapter.GetSchedules(requestContext.clientId, requestContext.agentId, start);

            if (timeSlots.Any())
            {
                foreach (var timeSlot in timeSlots)
                {
                    //overlap
                    if ((start >= timeSlot.Start && start < timeSlot.End) || (end > timeSlot.Start && end <= timeSlot.End))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Open question?
        // if there are less n (m) found, m is returned
        //
        public IEnumerable<TimeSlot> QueryAvailableTimes(IRequestContext requestContext, List<TimeSlot> timeSlots, int n, int duration)
        {
            var result = new List<TimeSlot>();
            var count = 0;
            var queryDatesGroupByDate = timeSlots.GroupBy(t => t.Start.ToShortDateString()).Select(g => new {g.Key, TimeSlots = g.Select(g1 => new TimeSlot(g1.Start, g1.End))});
            foreach (var current in queryDatesGroupByDate)
            {
                var reservedTimeSlots = _scheduleAdapter.GetSchedules(requestContext.clientId, requestContext.agentId, current.Key);
                var targetTimeSlots = current.TimeSlots.OrderBy(t => t.Start).ToList();
                foreach (var t in targetTimeSlots)
                {
                    var newTimeSlot = new TimeSlot(t.Start, t.Start.AddMinutes(duration));
                    if (!OverLap(newTimeSlot, reservedTimeSlots))
                    {
                        result.Add(newTimeSlot);
                        count++;
                        if (count >= n)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }


        //To do: Need to handle client not active, agent not active cases
        //below is assuming all active
        public WorkLoadResponse CheckWorkLoad(IRequestContext requestContext, DateTime date)
        {
            WorkLoadResponse response = new WorkLoadResponse();
            var timeSlots = _scheduleAdapter.GetSchedules(requestContext.clientId, requestContext.agentId, date);

            if (timeSlots.Any())
            {
                var totalScheduledMinutes = 0;
                //Also assuming timeslots have no overlapping
                //in real case, it need to be handled
                var sortedTimeSlots = timeSlots.OrderBy(t => t.Start);


                var availableTimeSlots = new List<TimeSlot>();
                //also assuming the agent working hours are 8-5
                //in real case, this can be configuable
                var freeTimeSlotStart = new DateTime(date.Year, date.Month, date.Day, 8, 8, 0);

                foreach (var timeSlot in sortedTimeSlots)
                {
                    totalScheduledMinutes += timeSlot.End.Subtract(timeSlot.Start).Minutes;                    
                    if (timeSlot.Start > freeTimeSlotStart)
                    {
                        response.AvailableTimeSlots.Add(new TimeSlot(freeTimeSlotStart, timeSlot.Start));
                        var end = timeSlot.End;
                        freeTimeSlotStart = new DateTime(end.Year, end.Month, end.Day, end.Hour, end.Minute, 0);
                    }
                }
                response.Busy = totalScheduledMinutes >= STANDARD_WORKING_DAY_IN_MINUTES; 
            } 

            return response;
        }

        private bool OverLap(TimeSlot t1, IList<TimeSlot> reservedTimeSlots)
        {
            foreach (var t2 in reservedTimeSlots)
            {
                if (
                    (t1.End > t2.Start && t1.End <= t2.End) ||
                    (t1.Start >= t2.Start && t1.Start < t2.End) ||
                    (t1.Start >= t2.Start && t1.End >= t2.End)
                    )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
