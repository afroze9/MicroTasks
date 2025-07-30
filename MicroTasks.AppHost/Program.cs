IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("companydb");

builder.AddProject<Projects.MicroTasks_Company>("company")
    .WithReference(postgres);
builder.Build().Run();
