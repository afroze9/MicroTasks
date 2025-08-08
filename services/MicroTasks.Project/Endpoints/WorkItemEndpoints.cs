using MicroTasks.ProjectApi.Models;
using MicroTasks.ProjectApi.Dtos;
using MicroTasks.ProjectApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroTasks.ProjectApi.Endpoints;

public static class WorkItemEndpoints
{
    public static void MapWorkItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/workitems");

        group.MapGet("/", GetAllWorkItemsAsync)
            .RequireAuthorization("WorkItemRead");
        group.MapGet("/{id}", GetWorkItemByIdAsync)
            .RequireAuthorization("WorkItemRead");
        group.MapPost("/", CreateWorkItemAsync)
            .RequireAuthorization("WorkItemWrite");
        group.MapPut("/{id}", UpdateWorkItemAsync)
            .RequireAuthorization("WorkItemWrite");
        group.MapDelete("/{id}", DeleteWorkItemAsync)
            .RequireAuthorization("WorkItemDelete");

        var projectsGroup = app.MapGroup("/api/projects");
        projectsGroup.MapGet("/{projectId}/workitems", GetWorkItemsByProjectIdAsync)
            .RequireAuthorization("WorkItemRead")
            .WithName("GetProjectWorkItems")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAllWorkItemsAsync(ProjectDbContext db)
    {
        var workItems = await db.WorkItems.ToListAsync();
        var result = workItems.Select(WorkItemDtoExtensions.FromEntity);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetWorkItemsByProjectIdAsync(Guid projectId, ProjectDbContext db)
    {
        var workItems = await db.WorkItems
            .Where(wi => wi.ProjectId == projectId)
            .ToListAsync();
        var result = workItems.Select(WorkItemDtoExtensions.FromEntity);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetWorkItemByIdAsync(Guid id, ProjectDbContext db)
    {
        var wi = await db.WorkItems.FindAsync(id);
        if (wi == null)
            return Results.NotFound();
        return Results.Ok(wi.FromEntity());
    }

    private static async Task<IResult> CreateWorkItemAsync(WorkItemDto dto, ProjectDbContext db)
    {
        var workItem = new WorkItem(dto.ProjectId, dto.Title, dto.Description);
        workItem.ChangeStatus(WorkItemStatusExtensions.FromWorkItemStatusString(dto.Status));
        await db.WorkItems.AddAsync(workItem);
        await db.SaveChangesAsync();
        return Results.Created($"/workitems/{workItem.Id}", workItem.FromEntity());
    }

    private static async Task<IResult> UpdateWorkItemAsync(Guid id, WorkItemDto dto, ProjectDbContext db)
    {
        var workItem = await db.WorkItems.FindAsync(id);
        if (workItem == null)
            return Results.NotFound();
        workItem.ChangeTitle(dto.Title);
        workItem.ChangeDescription(dto.Description);
        workItem.ChangeStatus(WorkItemStatusExtensions.FromWorkItemStatusString(dto.Status));
        db.WorkItems.Update(workItem);
        await db.SaveChangesAsync();
        return Results.Ok(workItem.FromEntity());
    }

    private static async Task<IResult> DeleteWorkItemAsync(Guid id, ProjectDbContext db)
    {
        var workItem = await db.WorkItems.FindAsync(id);
        if (workItem == null)
            return Results.NotFound();
        db.WorkItems.Remove(workItem);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
}
