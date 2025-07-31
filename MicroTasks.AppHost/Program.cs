IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("companydb")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<ProjectResource> companyApi = builder.AddProject<Projects.MicroTasks_Company>("company")
    .WaitFor(postgres)
    .WithReference(postgres);

// Add the React frontend app
builder.AddNpmApp("webapp", "../microtasks-webapp")
    .WithReference(companyApi)
    .WaitFor(companyApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
