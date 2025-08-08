using System.Collections.Concurrent;
using MicroTasks.ProjectApi.Models;
using MicroTasks.ProjectApi.Dtos;
using MicroTasks.ProjectApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroTasks.ProjectApi.Endpoints;

public static class ProjectEndpoints
{
    private static readonly ConcurrentDictionary<Guid, Project> projects = new();

    public static void MapProjectEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/projects");

        group.MapGet("/", GetAllProjectsAsync)
            .RequireAuthorization("ProjectRead");
        group.MapGet("/{id}", GetProjectByIdAsync)
            .RequireAuthorization("ProjectRead");
        group.MapPost("/", CreateProjectAsync)
            .RequireAuthorization("ProjectWrite");
        group.MapPut("/{id}", UpdateProjectAsync)
            .RequireAuthorization("ProjectWrite");
        group.MapDelete("/{id}", DeleteProjectAsync)
            .RequireAuthorization("ProjectDelete");
    }

    private static async Task<IResult> GetAllProjectsAsync(ProjectDbContext db)
    {
        List<Project> projects = await db.Projects.ToListAsync();
        IEnumerable<ProjectDto> result = projects.Select(p => p.FromEntity());
        return Results.Ok(result);
    }

    private static async Task<IResult> GetProjectByIdAsync(Guid id, ProjectDbContext db)
    {
        Project? p = await db.Projects.FindAsync(id);
        if (p == null)
            return Results.NotFound();
        return Results.Ok(p.FromEntity());
    }

    private static async Task<IResult> CreateProjectAsync(ProjectDto dto, ProjectDbContext db)
    {
        Project project = new Project(dto.Name, dto.Description, []);
        project.ChangeStatus(dto.Status.FromProjectStatusString());
        await db.Projects.AddAsync(project);
        await db.SaveChangesAsync();
        return Results.Created($"/projects/{project.Id}", ProjectDtoExtensions.FromEntity(project));
    }

    private static async Task<IResult> UpdateProjectAsync(Guid id, ProjectDto dto, ProjectDbContext db)
    {
        Project? project = await db.Projects.FindAsync(id);
        if (project == null)
            return Results.NotFound();
        project.ChangeName(dto.Name);
        project.ChangeDescription(dto.Description);
        project.ChangeStatus(dto.Status.FromProjectStatusString());
        db.Projects.Update(project);
        await db.SaveChangesAsync();
        return Results.Ok(ProjectDtoExtensions.FromEntity(project));
    }

    private static async Task<IResult> DeleteProjectAsync(Guid id, ProjectDbContext db)
    {
        Project? project = await db.Projects.FindAsync(id);
        if (project == null)
            return Results.NotFound();
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
}
