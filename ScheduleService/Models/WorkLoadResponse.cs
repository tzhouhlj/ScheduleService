namespace Services.Schedule.Models
{
    public class WorkLoadResponse
    {
        public bool Busy { get; set; }
        public List<TimeSlot> AvailableTimeSlots { get; set; }
        public WorkLoadResponse() 
        {
            AvailableTimeSlots = new List<TimeSlot>();
        }
    }
}
