using Microsoft.Extensions.Primitives;

namespace Services.Schedule.Context
{
    public class RequestContext : IRequestContext
    {
        const string CLIENT_ID_HEADER_NAME = "x-ci";
        const string AGENT_ID_HEADER_NAME = "x-ai";
        public string clientId { get; set; }
        public string agentId { get; set; }

        public RequestContext(IHeaderDictionary headers)
        {
            headers.TryGetValue(CLIENT_ID_HEADER_NAME, out StringValues outClientId);
            headers.TryGetValue(AGENT_ID_HEADER_NAME, out StringValues outAgentId);
            this.clientId = outClientId;
            this.agentId = outAgentId;
        }
    }
}
