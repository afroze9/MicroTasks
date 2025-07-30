using Microsoft.EntityFrameworkCore;
using MicroTasks.CompanyApi.Models;

namespace MicroTasks.CompanyApi.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
        public DbSet<Company> Companies => Set<Company>();
    }
}
