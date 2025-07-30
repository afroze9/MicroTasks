using MicroTasks.Company.Models;
using MicroTasks.Company.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace MicroTasks.Company.Endpoints
{
    public static class TodoEndpoints
    {
        public static RouteGroupBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
        {
            var todoGroup = app.MapGroup("/todos");

            todoGroup.MapGet("/", async (CompanyDbContext db) => await GetAllTodos(db));
            todoGroup.MapGet("/{id:guid}", async (CompanyDbContext db, Guid id) => await GetTodoById(db, id));
            todoGroup.MapPost("/", async (CompanyDbContext db, TodoItem todo) => await CreateTodo(db, todo));
            todoGroup.MapPut("/{id:guid}", async (CompanyDbContext db, Guid id, TodoItem updated) => await UpdateTodo(db, id, updated));
            todoGroup.MapDelete("/{id:guid}", async (CompanyDbContext db, Guid id) => await DeleteTodo(db, id));

            return todoGroup;
        }

        private static async Task<IResult> GetAllTodos(CompanyDbContext db)
            => Results.Ok(await db.TodoItems.ToListAsync());

        private static async Task<IResult> GetTodoById(CompanyDbContext db, Guid id)
        {
            var item = await db.TodoItems.FindAsync(id);
            return item is not null ? Results.Ok(item) : Results.NotFound();
        }

        private static async Task<IResult> CreateTodo(CompanyDbContext db, TodoItem todo)
        {
            todo.Id = Guid.NewGuid();
            todo.CreatedAt = DateTime.UtcNow;
            if (todo.IsCompleted)
                todo.CompletedAt = DateTime.UtcNow;
            db.TodoItems.Add(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/todos/{todo.Id}", todo);
        }

        private static async Task<IResult> UpdateTodo(CompanyDbContext db, Guid id, TodoItem updated)
        {
            var existing = await db.TodoItems.FindAsync(id);
            if (existing is null) return Results.NotFound();
            existing.Title = updated.Title;
            existing.Description = updated.Description;
            existing.IsCompleted = updated.IsCompleted;
            if (updated.IsCompleted && existing.CompletedAt == null)
                existing.CompletedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return Results.Ok(existing);
        }

        private static async Task<IResult> DeleteTodo(CompanyDbContext db, Guid id)
        {
            var item = await db.TodoItems.FindAsync(id);
            if (item is null) return Results.NotFound();
            db.TodoItems.Remove(item);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
