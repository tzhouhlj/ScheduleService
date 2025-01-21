# ScheduleService
## Assumptions:

	1. TimeSlot is accurate to the minute
	2. Each TimeSlot's start and end are in the same day
	3. Agent working hours/day is 8
	4. There are two FollowUpBoss provider apis:
		var timeSlots = _scheduleAdapter.GetSchedules(requestContext.clientId, requestContext.agentId, date //string);`
		var timeSlots = _scheduleAdapter.GetSchedules(requestContext.clientId, requestContext.agentId, date //DateTime);
	5. User Agent (agentId) and Client (clientId) are all active. None active/Non existed cases will be discussed but not implemented here.	
	6. For QueryAvailableTimes method: find n slots, assume the duration is another input parameter like 30 minutes or 60 minutes

 ## Api contracts:
        
        [HttpGet("Availability")]
        public bool GetAvailable([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? start, DateTime? end)
        
        [HttpPost("Availability/Slots")]
        public IEnumerable<TimeSlot> GetAvailableSlots([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, [FromBody]List<TimeSlot> timeSlots, int n, int duration)
        
        [HttpGet("Workload")]
        public WorkLoadResponse GetWorkLoad([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? date)
