namespace MicroTasks.Events;

public class ProjectUpdatedEvent
{
    public Guid ProjectId { get; set; }
    public Guid CompanyId { get; set; }
}
