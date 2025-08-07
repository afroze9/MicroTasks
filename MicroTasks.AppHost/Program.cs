IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak("keycloak", 9173)
    .WithDataBindMount(@"I:\Keycloak\Data");

IResourceBuilder<PostgresServerResource> companyDb = builder
    .AddPostgres("companydb")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<ProjectResource> companyApi = builder
    .AddProject<Projects.MicroTasks_Company>("company")
    .WaitFor(companyDb)
    .WithReference(companyDb);

IResourceBuilder<PostgresServerResource> projectDb = builder
    .AddPostgres("projectdb")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<ProjectResource> projectApi = builder
    .AddProject<Projects.MicroTasks_Project>("project")
    .WaitFor(projectDb)
    .WithReference(projectDb);

builder.AddNpmApp("webapp", "../microtasks-webapp")
    .WithReference(companyApi)
    .WaitFor(companyApi)
    .WithReference(projectApi)
    .WaitFor(projectApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
