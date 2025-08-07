using MicroTasks.ProjectApi.Endpoints;
using MicroTasks.ProjectApi.Data;
using Microsoft.EntityFrameworkCore;
// using Keycloak.AuthServices.Authentication;
// using Keycloak.AuthServices.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("projectdb")));

// Uncomment and configure authentication/authorization as needed
// if (Environment.GetEnvironmentVariable("ASPIRE_TEST_AUTH") == "1")
// {
//     builder.Services.AddAuthentication("Test")
//         .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
//     builder.Services.AddAuthorization(options =>
//     {
//         options.AddPolicy("ProjectRead", policy => policy.RequireRole("project_manager", "project_contributor", "project_viewer"));
//         options.AddPolicy("ProjectWrite", policy => policy.RequireRole("project_manager", "project_contributor"));
//         options.AddPolicy("ProjectDelete", policy => policy.RequireRole("project_manager"));
//     });
// }
// else
// {
//     builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
//     builder.Services.AddAuthorization(options =>
//     {
//         options.AddPolicy("ProjectRead", policy =>
//             policy.RequireResourceRoles("project_manager", "project_contributor", "project_viewer"));
//         options.AddPolicy("ProjectWrite", policy =>
//             policy.RequireResourceRoles("project_manager", "project_contributor"));
//         options.AddPolicy("ProjectDelete", policy =>
//             policy.RequireResourceRoles("project_manager"));
//     }).AddKeycloakAuthorization(builder.Configuration);
// }

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();
app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
    DbSeeder.Seed(db);
}

app.MapProjectEndpoints();
app.MapWorkItemEndpoints();

app.Run();
