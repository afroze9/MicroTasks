using MicroTasks.CompanyApi.Models;

namespace MicroTasks.CompanyApi.Data;

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

        // Seed Companies
        if (!db.Companies.Any())
        {
            db.Companies.AddRange(
                new Company(
                    name: "Acme Corp",
                    tags: new[] {
                        new Tag { Value = "enterprise" },
                        new Tag { Value = "technology" }
                    }
                )
                { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Company(
                    name: "Beta Solutions",
                    tags: new[] {
                        new Tag { Value = "startup" },
                        new Tag { Value = "consulting" }
                    }
                )
                { CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
            db.SaveChanges();
        }
    }
}
