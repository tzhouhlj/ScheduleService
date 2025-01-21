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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleService _scheduleService;
        private IRequestContext _requestContext;


        public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            _logger = logger;
            _scheduleService = scheduleService;
        }

        [HttpGet("IsAvailable")]
        public bool IsAvailable([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? start, DateTime? end)
        {
            _requestContext = new RequestContext(Request.Headers);
            return _scheduleService.Check(_requestContext, start.Value, end.Value);
        }

        [HttpPost("AvailableTimes")]
        public IEnumerable<TimeSlot> QueryAvailableTimes([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, List<TimeSlot> timeSlots, int n, int duration)
        {
            _requestContext = new RequestContext(Request.Headers);
            return _scheduleService.QueryAvailableTimes(_requestContext, timeSlots, n, duration);
        }

        [HttpGet("Workload")]
        public WorkLoadResponse CheckWorkLoad([FromHeader(Name = "x-ci")][Required] string clientId, [FromHeader(Name = "x-ai")][Required] string agentId, DateTime? date)
        {
            _requestContext = new RequestContext(Request.Headers);
            return _scheduleService.CheckWorkLoad(_requestContext, date.Value);
        }
    }
}
