using MassTransit;
using MicroTasks.CompanyApi.Models;
using MicroTasks.Events;

namespace MicroTasks.CompanyApi.Consumers;

public class ProjectCreatedEventConsumer : IConsumer<ProjectCreatedEvent>
{
    private readonly Data.CompanyDbContext _dbContext;

    public ProjectCreatedEventConsumer(Data.CompanyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ProjectCreatedEvent> context)
    {
        ProjectCreatedEvent eventData = context.Message;
        Company? company = await _dbContext.Companies.FindAsync(eventData.CompanyId);
        if (company != null)
        {
            company.SetProjectCount(company.ProjectCount + 1);
            await _dbContext.SaveChangesAsync();
        }
    }
}
