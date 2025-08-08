using MassTransit;
using MicroTasks.Events;
using MicroTasks.CompanyApi.Data;
using MicroTasks.CompanyApi.Models;

namespace MicroTasks.CompanyApi.Consumers;

public class ProjectDeletedEventConsumer : IConsumer<ProjectDeletedEvent>
{
    private readonly CompanyDbContext _dbContext;

    public ProjectDeletedEventConsumer(CompanyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ProjectDeletedEvent> context)
    {
        ProjectDeletedEvent eventData = context.Message;
        Company? company = await _dbContext.Companies.FindAsync(eventData.CompanyId);
        if (company != null && company.ProjectCount > 0)
        {
            company.SetProjectCount(company.ProjectCount - 1);
            await _dbContext.SaveChangesAsync();
        }
        // else: company not found or project count already zero
    }
}
