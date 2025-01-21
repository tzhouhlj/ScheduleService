namespace Services.Schedule.Models
{
    public class TimeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeSlot(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}
