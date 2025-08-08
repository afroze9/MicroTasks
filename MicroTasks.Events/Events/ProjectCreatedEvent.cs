namespace MicroTasks.Events;

public class ProjectCreatedEvent
{
    public Guid ProjectId { get; set; }
    public Guid CompanyId { get; set; }
}
