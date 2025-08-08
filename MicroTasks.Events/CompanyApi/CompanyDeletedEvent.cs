namespace MicroTasks.Events.CompanyApi;

public class CompanyDeletedEvent : IntegrationEvent
{
    public Guid CompanyId { get; set; }
}
