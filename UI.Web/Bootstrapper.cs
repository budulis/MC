using System;
using System.Security.Cryptography.X509Certificates;
using Core;
using Core.Domain.Contexts.Ordering.Messages;
using Core.ReadModel;
using Infrastructure.DataBase;
using Infrastructure.Dispatchers;
using Infrastructure.ReadModel;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Product;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;
using Nancy.TinyIoc;
using UI.Web.Modules.Order;

namespace UI.Web {
	public class Bootstrapper : DefaultNancyBootstrapper {
		protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
		{
			base.RequestStartup(container, pipelines, context);
		}

		private static Response GetResponse()
		{
			return new Response
			{
				StatusCode = HttpStatusCode.InternalServerError
			};
		}

		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			base.ConfigureApplicationContainer(container);
			container.Register(LoggerFactory.Default);
			container.Register<DataBaseContext>().AsMultiInstance();
			container.Register<IReadModelRepository<OrderReadModel>, OrderReadModelRepository>().AsMultiInstance();
			container.Register<IItemInfoRepository<ProductInfo>, ProductInfoRepository>().AsMultiInstance();
		}

		public static IDomainCommandDispatcher GetCommandDispatcher(ILogger logger, Func<IDomainCommandDispatcher> commandDispatcher)
		{
			var appEventDispatcher = EventDispathers.Application.GetDirect(logger);
			
			var domainEventDispatcher = EventDispathers.Domain.GetDirect(commandDispatcher, () => appEventDispatcher, logger);
			domainEventDispatcher = domainEventDispatcher.Register(typeof(OrderCompletedNotificationMessage), x => new OnOrderCompleted().Notify((OrderCompletedNotificationMessage)x));
			domainEventDispatcher = domainEventDispatcher.Register(typeof(SelfServiceOrderStartFailedNotificationMessage), x => new OnOrderFailure().Notify((SelfServiceOrderStartFailedNotificationMessage)x));
			domainEventDispatcher = domainEventDispatcher.Register(typeof(SelfServiceOrderCreatedNotificationMessage), x => new OnSelfServiceOrderCreated().Notify((SelfServiceOrderCreatedNotificationMessage)x));

			return CommandDispatchers.GetDirect(domainEventDispatcher, logger);
		}
	}
}