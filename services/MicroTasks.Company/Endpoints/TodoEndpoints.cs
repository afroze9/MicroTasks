using MicroTasks.CompanyApi.Models;
using MicroTasks.CompanyApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroTasks.CompanyApi.Endpoints
{
    public static class TodoEndpoints
    {
        public static RouteGroupBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder todoGroup = app.MapGroup("/todos");

            todoGroup.MapGet("/", GetAllTodos);
            todoGroup.MapGet("/{id:guid}", GetTodoById);
            todoGroup.MapPost("/", CreateTodo);
            todoGroup.MapPut("/{id:guid}", UpdateTodo);
            todoGroup.MapDelete("/{id:guid}", DeleteTodo);

            return todoGroup;
        }

        private static async Task<IResult> GetAllTodos(CompanyDbContext db)
            => Results.Ok(await db.TodoItems.ToListAsync());

        private static async Task<IResult> GetTodoById(CompanyDbContext db, Guid id)
        {
            TodoItem? item = await db.TodoItems.FindAsync(id);
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
            TodoItem? existing = await db.TodoItems.FindAsync(id);
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
            TodoItem? item = await db.TodoItems.FindAsync(id);
            if (item is null) return Results.NotFound();
            db.TodoItems.Remove(item);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
