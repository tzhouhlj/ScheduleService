# ScheduleService
## Assumptions:
	TimeSlot is accurate to the minute
	Each TimeSlot's start and end are in the same day
	Agent working hours/day is 8
	There are two FollowUpBoss provider apis:
		var timeSlots = _scheduleAdapter.GetSchedules(requestContext.clientId, requestContext.agentId, date //string);
		var timeSlots = _scheduleAdapter.GetSchedules(requestContext.clientId, requestContext.agentId, date //DateTime);
	User Agent (agentId) and Client (clientId) are all active. None active/Non existed cases will be discussed but not implemented here.	
	For QueryAvailableTimes method: find n slots, assume the duration is another input parameter like 30 minutes or 60 minutes

 ## Api contracts:
        [HttpGet("IsAvailable")]
        public bool IsAvailable([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? start, DateTime? end)
        [HttpPost("AvailableTimes")]
        public IEnumerable<TimeSlot> QueryAvailableTimes([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, List<TimeSlot> timeSlots, int n, int duration)
        [HttpGet("Workload")]
        public WorkLoadResponse CheckWorkLoad([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? date)
