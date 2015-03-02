using System;

namespace Core.Domain.Contexts.Ordering.Commands {
	public class CompleteOrder : IDomainCommand {
		public CompleteOrder(Id id) {
			Id = id;
		}
		public Id Id { get; private set; }

	}
}