using Microsoft.EntityFrameworkCore;
using MicroTasks.CompanyApi.Data;
using System.Linq;

namespace MicroTasks.CompanyApi.Tests;

public class DbSeederTests
{
    [Fact]
    public void Seed_AddsDefaultCompanies()
    {
        var options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDbSeeder")
            .Options;
        using var db = new CompanyDbContext(options);

        // Act
        DbSeeder.Seed(db);

        // Assert
        var companies = db.Companies.Include(c => c.Tags).ToList();
        Assert.True(companies.Count >= 2);
        Assert.Contains(companies, c => c.Name == "Acme Corp");
        Assert.Contains(companies, c => c.Name == "Beta Solutions");
        Assert.All(companies, c => Assert.NotEmpty(c.Tags));
    }
}
