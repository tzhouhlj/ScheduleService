namespace Services.Schedule.Context
{
    public interface IRequestContext
    {
        string clientId { get; set; }
        string agentId { get; set; }
    }
}
