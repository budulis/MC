namespace Core.Domain.Contexts.Ordering.Commands
{
	public class CompleteSelfServiceOrder : IDomainCommand {
		public Id Id { get; private set; }

		public CompleteSelfServiceOrder(Id id) {
			Id = id;
		}
	}
}