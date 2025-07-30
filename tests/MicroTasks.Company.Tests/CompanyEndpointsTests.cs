using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MicroTasks.Company.Tests;

public class CompanyEndpointsTests
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MicroTasks_AppHost>();

        builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using Aspire.Hosting.DistributedApplication app = await builder.BuildAsync();
        await app.StartAsync();

        // Act
        System.Net.Http.HttpClient httpClient = app.CreateHttpClient("company");
        using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await app.ResourceNotifications.WaitForResourceHealthyAsync("company", cts.Token);
        System.Net.Http.HttpResponseMessage response = await httpClient.GetAsync("/companies");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}