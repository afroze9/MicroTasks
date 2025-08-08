using MicroTasks.CompanyApi.Endpoints;
using MicroTasks.CompanyApi.Data;
using Microsoft.EntityFrameworkCore;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using MicroTasks.CompanyApi.Auth;
using MassTransit;
using MicroTasks.Events.CompanyApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

// Register common services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<ICurrentDateTimeService, CurrentDateTimeService>();
builder.Services.AddScoped<AuditEntitySaveChangesInterceptor>();

builder.Services.AddDbContext<CompanyDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("companydb"));
    AuditEntitySaveChangesInterceptor interceptor = sp.GetRequiredService<AuditEntitySaveChangesInterceptor>();
    options.AddInterceptors(interceptor);
});

if (Environment.GetEnvironmentVariable("ASPIRE_TEST_AUTH") == "1")
{
    // Use test authentication handler for integration tests
    builder.Services.AddAuthentication("Test")
        .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CompanyRead", policy => policy.RequireRole("company_manager", "company_contributor", "company_viewer"));
        options.AddPolicy("CompanyWrite", policy => policy.RequireRole("company_manager", "company_contributor"));
        options.AddPolicy("CompanyDelete", policy => policy.RequireRole("company_manager"));
    });
}
else
{
    builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
    builder.Services.AddAuthorization(options =>
    {
        // company_manager: create, edit, read, delete
        // company_contributor: create, edit, read
        // company_viewer: read
        options.AddPolicy("CompanyRead", policy =>
            policy.RequireResourceRoles("company_manager", "company_contributor", "company_viewer"));
        options.AddPolicy("CompanyWrite", policy =>
            policy.RequireResourceRoles("company_manager", "company_contributor"));
        options.AddPolicy("CompanyDelete", policy =>
            policy.RequireResourceRoles("company_manager"));
    }).AddKeycloakAuthorization(builder.Configuration);
}

builder.Services.AddMassTransit(options =>
{
    options.UsingInMemory();
    options.AddRider(rider =>
    {
        rider.AddProducer<CompanyCreatedEvent>(nameof(CompanyCreatedEvent));
        rider.AddProducer<CompanyUpdatedEvent>(nameof(CompanyUpdatedEvent));
        rider.AddProducer<CompanyDeletedEvent>(nameof(CompanyDeletedEvent));
        rider.UsingKafka((context, k) =>
        {
            k.Host(builder.Configuration.GetConnectionString("kafka"));
        });
    });
});

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
    CompanyDbContext db = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();
    DbSeeder.Seed(db);

    IBusControl busControl = scope.ServiceProvider.GetRequiredService<IBusControl>();
    await busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
}

app.MapCompanyEndpoints();
app.Run();
