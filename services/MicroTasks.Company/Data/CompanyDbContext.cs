using Microsoft.EntityFrameworkCore;
using MicroTasks.CompanyApi.Models;

namespace MicroTasks.CompanyApi.Data;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Value)
            .IsUnique();
    }
}