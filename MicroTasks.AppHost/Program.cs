IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak("keycloak", 9173)
    .WithDataBindMount(@"I:\Keycloak\Data")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<KafkaServerResource> kafka = builder
    .AddKafka("kafka", 9092)
    .WithKafkaUI();

IResourceBuilder<PostgresServerResource> companyDb = builder
    .AddPostgres("companydb")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<ProjectResource> companyApi = builder
    .AddProject<Projects.MicroTasks_Company>("company")
    .WithReference(kafka)
    .WaitFor(companyDb)
    .WithReference(companyDb);

IResourceBuilder<PostgresServerResource> projectDb = builder
    .AddPostgres("projectdb")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<ProjectResource> projectApi = builder
    .AddProject<Projects.MicroTasks_Project>("project")
    .WithReference(kafka)
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
