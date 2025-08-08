namespace MicroTasks.Events;

public class ProjectDeletedEvent
{
    public Guid ProjectId { get; set; }
    public Guid CompanyId { get; set; }
}
