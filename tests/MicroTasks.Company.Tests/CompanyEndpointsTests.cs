namespace MicroTasks.CompanyApi.Tests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using Aspire.Hosting;
using System.Net.Http;
using System.Net;

public class CompanyAppFixture : IAsyncLifetime
{
    public DistributedApplication App { get; private set; } = default!;
    public HttpClient HttpClient { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MicroTasks_AppHost>();
        builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        App = await builder.BuildAsync();
        await App.StartAsync();
        using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await App.ResourceNotifications.WaitForResourceHealthyAsync("company", cts.Token);
        HttpClient = App.CreateHttpClient("company");
    }

    public async Task DisposeAsync()
    {
        if (App != null)
        {
            await App.DisposeAsync();
        }
        return;
    }
}

public class CompanyEndpointsTests : IClassFixture<CompanyAppFixture>
{
    private readonly CompanyAppFixture _fixture;

    public CompanyEndpointsTests(CompanyAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync("/companies");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateCompanyReturnsCreatedStatusCode()
    {
        var company = new { Name = "TestCo", Tags = new[] { new { Name = "demo" } } };
        HttpResponseMessage response = await _fixture.HttpClient.PostAsJsonAsync("/companies", company);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetCompanyByIdReturnsOkStatusCode()
    {
        var company = new { Name = "TestCo2", Tags = new[] { new { Name = "demo" } } };
        HttpResponseMessage createResponse = await _fixture.HttpClient.PostAsJsonAsync("/companies", company);
        JsonElement createdCompanyJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        string id = createdCompanyJson.GetProperty("id").GetString();
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync($"/companies/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompanyReturnsNoContentStatusCode()
    {
        var company = new { Name = "TestCo3", Tags = new[] { new { Name = "demo" } } };
        HttpResponseMessage createResponse = await _fixture.HttpClient.PostAsJsonAsync("/companies", company);
        JsonElement createdCompanyJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        string id = createdCompanyJson.GetProperty("id").GetString();
        var updatedCompany = new { Name = "TestCo3 Updated", Tags = new[] { new { Name = "updated" } } };
        HttpResponseMessage response = await _fixture.HttpClient.PutAsJsonAsync($"/companies/{id}", updatedCompany);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCompanyReturnsNoContentStatusCode()
    {
        var company = new { Name = "TestCo4", Tags = new[] { new { Name = "demo" } } };
        HttpResponseMessage createResponse = await _fixture.HttpClient.PostAsJsonAsync("/companies", company);
        JsonElement createdCompanyJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        string id = createdCompanyJson.GetProperty("id").GetString();
        HttpResponseMessage response = await _fixture.HttpClient.DeleteAsync($"/companies/{id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}