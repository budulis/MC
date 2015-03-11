namespace Core.Domain.Contexts.Ordering.Commands {
	public class CompleteOrder : IDomainCommand {
		public Id Id { get; private set; }
		public CompleteOrder(Id id)
		{
			Id = id;
		}
	}
}