using MicroTasks.ProjectApi.Models;

namespace MicroTasks.ProjectApi.Data;

public static class DbSeeder
{
    public static void Seed(ProjectDbContext db)
    {
        db.Database.EnsureCreated();

        // Seed Projects
        if (!db.Projects.Any())
        {
            List<WorkItem> workItems1 = new List<WorkItem>
            {
                new WorkItem(Guid.NewGuid(), "Setup project repo", "Initialize repository and CI/CD")
                {
                    CreatedBy = "seeder",
                    UpdatedBy = "seeder"
                },
                new WorkItem(Guid.NewGuid(), "Design database schema", "Model entities and relationships")
                {
                    CreatedBy = "seeder",
                    UpdatedBy = "seeder"
                }
            };
            List<WorkItem> workItems2 = new List<WorkItem>
            {
                new WorkItem(Guid.NewGuid(), "Create login page", "Implement authentication UI")
                {
                    CreatedBy = "seeder",
                    UpdatedBy = "seeder"
                },
                new WorkItem(Guid.NewGuid(), "Integrate API", "Connect frontend to backend services")
                {
                    CreatedBy = "seeder",
                    UpdatedBy = "seeder"
                }
            };
            db.Projects.AddRange(
                new Project(
                    name: "MicroTasks Platform",
                    description: "Platform for managing micro tasks",
                    workItems: workItems1
                )
                {
                    CreatedBy = "seeder",
                    UpdatedBy = "seeder"
                },
                new Project(
                    name: "Client Portal",
                    description: "Portal for client interactions",
                    workItems: workItems2
                )
                {
                    CreatedBy = "seeder",
                    UpdatedBy = "seeder"
                }
            );
            db.SaveChanges();
        }
    }
}
