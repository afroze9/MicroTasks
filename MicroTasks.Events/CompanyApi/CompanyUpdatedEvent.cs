namespace MicroTasks.Events.CompanyApi;

public class CompanyUpdatedEvent : IntegrationEvent
{
    public Guid CompanyId { get; set; }
}
