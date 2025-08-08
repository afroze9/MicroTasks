using Microsoft.EntityFrameworkCore;
using MicroTasks.CompanyApi.Data;
using System.Linq;

namespace MicroTasks.CompanyApi.Tests;

public class DbSeederTests
{
    [Fact]
    public void Seed_AddsDefaultCompanies()
    {
        DbContextOptions<CompanyDbContext> options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDbSeeder")
            .Options;
        using CompanyDbContext db = new CompanyDbContext(options);

        // Act
        DbSeeder.Seed(db);

        // Assert
        System.Collections.Generic.List<CompanyApi.Models.Company> companies = db.Companies.Include(c => c.Tags).ToList();
        Assert.True(companies.Count >= 2);
        Assert.Contains(companies, c => c.Name == "Acme Corp");
        Assert.Contains(companies, c => c.Name == "Beta Solutions");
        Assert.All(companies, c => Assert.NotEmpty(c.Tags));
    }
}
