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
            db.Projects.AddRange(
                new Project(
                    name: "MicroTasks Platform",
                    description: "Platform for managing micro tasks",
                    workItems: new List<WorkItem>
                    {
                        new WorkItem(Guid.NewGuid(), "Setup project repo", "Initialize repository and CI/CD"),
                        new WorkItem(Guid.NewGuid(), "Design database schema", "Model entities and relationships")
                    }
                ),
                new Project(
                    name: "Client Portal",
                    description: "Portal for client interactions",
                    workItems: new List<WorkItem>
                    {
                        new WorkItem(Guid.NewGuid(), "Create login page", "Implement authentication UI"),
                        new WorkItem(Guid.NewGuid(), "Integrate API", "Connect frontend to backend services")
                    }
                )
            );
            db.SaveChanges();
        }
    }
}
