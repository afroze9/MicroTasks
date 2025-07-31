using MicroTasks.CompanyApi.Models;

namespace MicroTasks.CompanyApi.Data;

public static class DbSeeder
{
    public static void Seed(CompanyDbContext db)
    {
        db.Database.EnsureCreated();

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
