using MicroTasks.ProjectApi.Endpoints;
using MicroTasks.ProjectApi.Data;
using Microsoft.EntityFrameworkCore;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using MicroTasks.ProjectApi.Auth;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<ICurrentDateTimeService, CurrentDateTimeService>();
builder.Services.AddScoped<AuditEntitySaveChangesInterceptor>();

builder.Services.AddDbContext<ProjectDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("projectdb"));
    AuditEntitySaveChangesInterceptor interceptor = sp.GetRequiredService<AuditEntitySaveChangesInterceptor>();
    options.AddInterceptors(interceptor);
});


if (Environment.GetEnvironmentVariable("ASPIRE_TEST_AUTH") == "1")
{
    builder.Services.AddAuthentication("Test")
        .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ProjectRead", policy => policy.RequireRole("project_manager", "project_contributor", "project_viewer"));
        options.AddPolicy("ProjectWrite", policy => policy.RequireRole("project_manager", "project_contributor"));
        options.AddPolicy("ProjectDelete", policy => policy.RequireRole("project_manager"));
        options.AddPolicy("WorkItemRead", policy => policy.RequireRole("workitem_manager", "workitem_contributor", "workitem_viewer"));
        options.AddPolicy("WorkItemWrite", policy => policy.RequireRole("workitem_manager", "workitem_contributor"));
        options.AddPolicy("WorkItemDelete", policy => policy.RequireRole("workitem_manager"));
    });
}
else
{
    builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ProjectRead", policy =>
            policy.RequireResourceRoles("project_manager", "project_contributor", "project_viewer"));
        options.AddPolicy("ProjectWrite", policy =>
            policy.RequireResourceRoles("project_manager", "project_contributor"));
        options.AddPolicy("ProjectDelete", policy =>
            policy.RequireResourceRoles("project_manager"));
        options.AddPolicy("WorkItemRead", policy =>
            policy.RequireResourceRoles("workitem_manager", "workitem_contributor", "workitem_viewer"));
        options.AddPolicy("WorkItemWrite", policy =>
            policy.RequireResourceRoles("workitem_manager", "workitem_contributor"));
        options.AddPolicy("WorkItemDelete", policy =>
            policy.RequireResourceRoles("workitem_manager"));
    }).AddKeycloakAuthorization(builder.Configuration);
}

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();

using (IServiceScope scope = app.Services.CreateScope())
{
    ProjectDbContext db = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
    DbSeeder.Seed(db);
}

app.MapProjectEndpoints();
app.MapWorkItemEndpoints();

app.Run();
