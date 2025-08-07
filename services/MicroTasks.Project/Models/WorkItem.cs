namespace MicroTasks.ProjectApi.Models;

public class WorkItem
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public WorkItemStatus Status { get; private set; } = WorkItemStatus.New;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // DDD: Private constructor for EF Core
    private WorkItem() { }

    // DDD: Public constructor for new WorkItem
    public WorkItem(Guid projectId, string title, string description = "")
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Title = title;
        Description = description;
        Status = WorkItemStatus.New;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    // Business logic
    public void ChangeTitle(string newTitle)
    {
        if (!string.IsNullOrWhiteSpace(newTitle) && newTitle != Title)
        {
            Title = newTitle;
        }
    }

    public void ChangeDescription(string newDescription)
    {
        if (newDescription != Description)
        {
            Description = newDescription;
        }
    }

    public void ChangeStatus(WorkItemStatus newStatus)
    {
        if (newStatus != Status)
        {
            Status = newStatus;
        }
    }

    public void MarkCompleted()
    {
        Status = WorkItemStatus.Done;
        CompletedAt = DateTime.UtcNow;
    }
}
