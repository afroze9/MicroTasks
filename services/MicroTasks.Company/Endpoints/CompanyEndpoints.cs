using MicroTasks.CompanyApi.Data;
using Microsoft.EntityFrameworkCore;
using MicroTasks.CompanyApi.Models;

namespace MicroTasks.CompanyApi.Endpoints
{
    public static class CompanyEndpoints
    {
        public static RouteGroupBuilder MapCompanyEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder companyGroup = app.MapGroup("/companies");

            companyGroup.MapGet("/", GetAllCompanies);
            companyGroup.MapGet("/{id:guid}", GetCompanyById);
            companyGroup.MapPost("/", CreateCompany);
            companyGroup.MapPut("/{id:guid}", UpdateCompany);
            companyGroup.MapDelete("/{id:guid}", DeleteCompany);

            return companyGroup;
        }

        private static async Task<IResult> GetAllCompanies(CompanyDbContext db)
            => Results.Ok(await db.Companies.Include(c => c.Tags).ToListAsync());

        private static async Task<IResult> GetCompanyById(CompanyDbContext db, Guid id)
        {
            var company = await db.Companies.Include(c => c.Tags).FirstOrDefaultAsync(c => c.Id == id);
            return company is not null ? Results.Ok(company) : Results.NotFound();
        }

        private static async Task<IResult> CreateCompany(CompanyDbContext db, CompanyDto dto)
        {
            var tags = dto.Tags?.Select(t => new Tag { Value = t.Value }).ToList() ?? new List<Tag>();
            var company = new Company(dto.Name, tags)
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            db.Companies.Add(company);
            await db.SaveChangesAsync();
            return Results.Created($"/companies/{company.Id}", company);
        }

        private static async Task<IResult> UpdateCompany(CompanyDbContext db, Guid id, CompanyDto dto)
        {
            var existing = await db.Companies.FindAsync(id);
            if (existing is null) return Results.NotFound();
            existing.ChangeName(dto.Name);
            var tags = dto.Tags?.Select(t => new Tag { Value = t.Value }).ToList() ?? new List<Tag>();
            existing.AddTags(tags);
            existing.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return Results.Ok(existing);
        }

        private static async Task<IResult> DeleteCompany(CompanyDbContext db, Guid id)
        {
            var company = await db.Companies.Include(c => c.Tags).FirstOrDefaultAsync(c => c.Id == id);
            if (company is null) return Results.NotFound();
            company.ClearTags(); // Remove links to tags
            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
