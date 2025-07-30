using Microsoft.EntityFrameworkCore;
using MicroTasks.Company.Models;

namespace MicroTasks.Company.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}
