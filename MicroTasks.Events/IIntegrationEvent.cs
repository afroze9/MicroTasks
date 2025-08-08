namespace MicroTasks.Events;

public interface IIntegrationEvent<TKey>
{
    TKey Id { get; set; }
}
