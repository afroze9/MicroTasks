namespace MicroTasks.ProjectApi.Models;

public enum WorkItemStatus
{
    New,
    InProgress,
    Done,
    Blocked,
    Cancelled
}

public static class WorkItemStatusExtensions
{
    public static string ToWorkItemStatusString(this WorkItemStatus status)
    {
        return status.ToString();
    }

    public static WorkItemStatus FromWorkItemStatusString(string value)
    {
        if (Enum.TryParse<WorkItemStatus>(value, true, out WorkItemStatus result))
            return result;
        throw new ArgumentException($"Invalid WorkItemStatus value: {value}");
    }
}
