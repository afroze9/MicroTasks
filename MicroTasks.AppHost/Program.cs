IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak("keycloak", 9173)
    .WithDataBindMount(@"I:\Keycloak\Data");

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("companydb")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<ProjectResource> companyApi = builder
    .AddProject<Projects.MicroTasks_Company>("company")
    .WaitFor(postgres)
    .WithReference(postgres);

builder.AddNpmApp("webapp", "../microtasks-webapp")
    .WithReference(companyApi)
    .WaitFor(companyApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
