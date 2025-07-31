IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("companydb")
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.MicroTasks_Company>("company")
    .WaitFor(postgres)
    .WithReference(postgres);
builder.Build().Run();
