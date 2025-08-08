namespace MicroTasks.Events.CompanyApi;

public class CompanyCreatedEvent : IntegrationEvent
{
    public Guid CompanyId { get; set; }
}
