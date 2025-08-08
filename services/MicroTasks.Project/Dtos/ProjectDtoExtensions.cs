using MicroTasks.ProjectApi.Models;

namespace MicroTasks.ProjectApi.Dtos;

public static class ProjectDtoExtensions
{
    public static ProjectDto FromEntity(this Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            CompanyId = project.CompanyId,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Status = project.Status.ToProjectStatusString()
        };
    }
}
