namespace MicroTasks.ProjectApi.Models;

public enum ProjectStatus
{
    New,
    Active,
    Completed,
    Archived,
    Cancelled
}

public static class ProjectStatusExtensions
{
    public static string ToProjectStatusString(this ProjectStatus status)
    {
        return status.ToString();
    }

    public static ProjectStatus FromProjectStatusString(this string value)
    {
        if (Enum.TryParse<ProjectStatus>(value, true, out var result))
            return result;
        throw new ArgumentException($"Invalid ProjectStatus value: {value}");
    }
}
