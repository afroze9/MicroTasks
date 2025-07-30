var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.MapGet("/hello", () => "hello");

app.Run();