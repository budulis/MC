namespace Core.Domain
{
	public interface IDomainEvent{
		Id Id { get; }
		IDomainEventNotificationMessage ToMessage();
	}
}