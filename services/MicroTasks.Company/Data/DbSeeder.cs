using MicroTasks.Company.Data;
using MicroTasks.Company.Models;

namespace MicroTasks.Company.Data;

public static class DbSeeder
{
    public static void Seed(CompanyDbContext db)
    {
        db.Database.EnsureCreated();
        if (!db.TodoItems.Any())
        {
            db.TodoItems.AddRange(
                new TodoItem
                {
                    Title = "Seed: Buy groceries",
                    Description = "Milk, Bread, Eggs",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new TodoItem
                {
                    Title = "Seed: Finish Aspire setup",
                    Description = "Complete initial microservice integration",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
            db.SaveChanges();
        }
    }
}
