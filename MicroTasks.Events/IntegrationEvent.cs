namespace MicroTasks.Events;

public abstract class IntegrationEvent : IIntegrationEvent<Guid>
{
    public Guid Id { get; set; }

    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
    }
}