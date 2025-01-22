using Microsoft.AspNetCore.Mvc;
using Services.Schedule.Context;
using Services.Schedule.Models;
using Services.Schedule.Services;
using System.ComponentModel.DataAnnotations;

namespace ScheduleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleService _scheduleService;
        private IRequestContext _requestContext;


        public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            _logger = logger;
            _scheduleService = scheduleService;
        }

        [HttpGet("Availability")]
        public bool GetAvailable([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? start, DateTime? end)
        {
            _requestContext = new RequestContext(Request.Headers);
            return _scheduleService.Check(_requestContext, start.Value, end.Value);
        }

        [HttpPost("Availability/Slots")]
        public IEnumerable<TimeSlot> GetAvailableSlots([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, [FromBody]List<TimeSlot> timeSlots, int n, int duration)
        {
            _requestContext = new RequestContext(Request.Headers);
            return _scheduleService.QueryAvailableTimes(_requestContext, timeSlots, n, duration);
        }

        [HttpGet("Workload")]
        public WorkLoadResponse GetWorkLoad([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? date)
        {
            _requestContext = new RequestContext(Request.Headers);
            return _scheduleService.CheckWorkLoad(_requestContext, date.Value);
        }

        [HttpGet("Schedules")]
        public IList<TimeSlot> GetSchedules([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] List<string> agentIds, DateTime? start, DateTime? end)
        {
            return null;
        }

        [HttpPost("Schedules")]
        public IList<TimeSlot> CreateSchedules([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, [FromBody]List<TimeSlot> timeSlots)
        {
            return null;
        }

        //Timeslot will need schedule id
        [HttpPut("Schedules")]
        public IList<TimeSlot> ModifySchedules([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, List<TimeSlot> timeSlots)
        {
            return null;
        }
    }
}
