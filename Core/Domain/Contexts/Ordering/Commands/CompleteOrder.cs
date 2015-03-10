using System;
using Core.Domain.Contexts.Production.Commands;

namespace Core.Domain.Contexts.Ordering.Commands {
	public class CompleteOrder : IDomainCommand {
		public Id Id { get; private set; }
		public OrderType OrderType { get; private set; }

		public CompleteOrder(Id id, OrderType orderType)
		{
			OrderType = orderType;
			Id = id;
		}
	}
}