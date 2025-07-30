
using MicroTasks.CompanyApi.Endpoints;
using MicroTasks.CompanyApi.Data;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.Services.AddDbContext<CompanyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("companydb")));

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

// Seed initial TodoItems if database is empty
using (IServiceScope scope = app.Services.CreateScope())
{
    CompanyDbContext db = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();
    DbSeeder.Seed(db);
}

// Register TodoItem endpoints using vertical slice best practices
app.MapTodoEndpoints();
app.MapCompanyEndpoints();

app.Run();