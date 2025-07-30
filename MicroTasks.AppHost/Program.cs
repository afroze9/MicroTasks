var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.MicroTasks_Company>("company");
builder.Build().Run();
