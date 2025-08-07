using MicroTasks.ProjectApi.Models;

namespace MicroTasks.ProjectApi.Dtos;

public static class WorkItemDtoExtensions
{
    public static WorkItemDto FromEntity(this WorkItem workItem)
    {
        return new WorkItemDto
        {
            Id = workItem.Id,
            ProjectId = workItem.ProjectId,
            Title = workItem.Title,
            Description = workItem.Description,
            Status = workItem.Status.ToWorkItemStatusString(),
            CreatedAt = workItem.CreatedAt,
            UpdatedAt = workItem.UpdatedAt,
            CompletedAt = workItem.CompletedAt
        };
    }
}
