using MicroTasks.CompanyApi.Data;
using Microsoft.EntityFrameworkCore;
using MicroTasks.CompanyApi.Models;
using MicroTasks.CompanyApi.Dtos;

namespace MicroTasks.CompanyApi.Endpoints;

public static class CompanyEndpoints
{
    public static RouteGroupBuilder MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder companyGroup = app.MapGroup("/api/companies");

        companyGroup.MapGet("/", GetAllCompanies)
            .RequireAuthorization("CompanyRead");
        companyGroup.MapGet("/{id:guid}", GetCompanyById)
            .RequireAuthorization("CompanyRead");
        companyGroup.MapPost("/", CreateCompany)
            .RequireAuthorization("CompanyWrite");
        companyGroup.MapPut("/{id:guid}", UpdateCompany)
            .RequireAuthorization("CompanyWrite");
        companyGroup.MapDelete("/{id:guid}", DeleteCompany)
            .RequireAuthorization("CompanyDelete");

        return companyGroup;
    }

    private static async Task<IResult> GetAllCompanies(CompanyDbContext db)
        => Results.Ok(await db.Companies.Include(c => c.Tags).ToListAsync());

    private static async Task<IResult> GetCompanyById(CompanyDbContext db, Guid id)
    {
        Company? company = await db.Companies.Include(c => c.Tags).FirstOrDefaultAsync(c => c.Id == id);
        return company is not null ? Results.Ok(company) : Results.NotFound();
    }

    private static async Task<IResult> CreateCompany(CompanyDbContext db, CompanyDto dto)
    {
        List<Tag> tags = new List<Tag>();
        if (dto.Tags != null && dto.Tags.Count != 0)
        {
            List<string> tagValues = dto.Tags.Select(t => t.Value).Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToList();
            List<Tag> existingTags = await db.Tags.Where(t => tagValues.Contains(t.Value)).ToListAsync();
            HashSet<string> existingTagValues = existingTags.Select(t => t.Value).ToHashSet();
            List<Tag> newTags = tagValues
                .Where(v => !existingTagValues.Contains(v))
                .Select(v => new Tag(v))
                .ToList();
            db.Tags.AddRange(newTags);
            tags.AddRange(existingTags);
            tags.AddRange(newTags);
        }
        Company company = new Company(dto.Name, tags)
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
        Company? existing = await db.Companies.Include(c => c.Tags).FirstOrDefaultAsync(c => c.Id == id);
        if (existing is null) return Results.NotFound();
        existing.ChangeName(dto.Name);
        List<Tag> tags = new List<Tag>();
        if (dto.Tags != null && dto.Tags.Count != 0)
        {
            List<string> tagValues = dto.Tags.Select(t => t.Value).Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToList();
            List<Tag> existingTags = await db.Tags.Where(t => tagValues.Contains(t.Value)).ToListAsync();
            HashSet<string> existingTagValues = existingTags.Select(t => t.Value).ToHashSet();
            List<Tag> newTags = tagValues
                .Where(v => !existingTagValues.Contains(v))
                .Select(v => new Tag(v))
                .ToList();
            db.Tags.AddRange(newTags);
            tags.AddRange(existingTags);
            tags.AddRange(newTags);
        }
        existing.ClearTags();
        existing.AddTags(tags);
        existing.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Results.Ok(existing);
    }

    private static async Task<IResult> DeleteCompany(CompanyDbContext db, Guid id)
    {
        Company? company = await db.Companies.Include(c => c.Tags).FirstOrDefaultAsync(c => c.Id == id);
        if (company is null) return Results.NotFound();
        company.ClearTags(); // Remove links to tags
        db.Companies.Remove(company);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
}
