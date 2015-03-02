using System.Threading.Tasks;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Commands;
using Core.Domain.Contexts.Production.Commands;
using Core.Handlers;
using System;
using Microsoft.CSharp.RuntimeBinder;

namespace Infrastructure.Dispatchers {
	internal class DirectCommandDispatcher : IDomainCommandDispatcher {
		private readonly IInfrastructureService _services;
		private readonly IDomainEventDispather _eventDispather;

		public DirectCommandDispatcher(IInfrastructureService services, IDomainEventDispather eventDispather) {
			_services = services;
			_eventDispather = eventDispather;
		}

		public async Task Dispatch(IDomainCommand command) {
			try {
				await DispatchCommand((dynamic)command);
			}
			catch (RuntimeBinderException ex) {
				throw new Exception("Error in configuration. Must exist single handler for " + command.GetType(), ex);
			}
		}

		private Task DispatchCommand(CreateOrder c) {
			return new HandlerForCreateOrder(_services, _eventDispather).Handle(c);
		}

		private Task DispatchCommand(CreateSelfServiceOrder c) {
			return new HandlerForCreateSelfServiceOrder(_services, _eventDispather)
				.Handle(c);
		}

		private Task DispatchCommand(PayForOrder c) {
			return new HandlerForPayForOrder(_services.EventStore, _services.DiscountService, _services.CardPaymentService, _services.Logger, _eventDispather)
				.Handle(c);
		}

		private Task DispatchCommand(CompleteOrder c) {
			return new HandlerForCompleteOrder(_services.EventStore, _services.DiscountService, _services.CardPaymentService, _services.Logger, _eventDispather)
				.Handle(c);
		}

		private Task DispatchCommand(CreateProductionOrder c) {
			return new HandlerForCreateProductionOrder(_services, _eventDispather)
				.Handle(c);
		}

		private Task DispatchCommand(ProduceProductionOrder c) {
			return new HandlerForProduceProductionOrder(_services.EventStore, _services.DiscountService, _services.CardPaymentService, _services.Logger, _eventDispather)
			.Handle(c);
		}

		public void Dispose() {

		}
	}
}