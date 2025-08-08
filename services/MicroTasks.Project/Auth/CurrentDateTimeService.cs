namespace MicroTasks.ProjectApi.Auth;

public interface ICurrentDateTimeService
{
    DateTime UtcNow { get; }
}

public class CurrentDateTimeService : ICurrentDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
