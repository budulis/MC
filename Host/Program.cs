using System.Security.Cryptography.X509Certificates;
using Core;
using Infrastructure.Dispatchers;
using Infrastructure.Initialization;
using Infrastructure.Queuing;
using Infrastructure.Services.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering.Messages;

namespace Host {
	class Program {

		static void Main(string[] args) {

			var logger = LoggerFactory.Default;

			var commandDispatcher = CommandDispatchers.GetDirect(EventDispathers.Domain.GetQueued(logger), logger);
			var appEventDispatcher = EventDispathers.Application.GetQueued(logger);
			var eventDispatcher = EventDispathers.Domain.GetDirect(() => commandDispatcher, () => appEventDispatcher, logger);
			eventDispatcher.Register(typeof (OrderCompletedNotificationMessage), Senders.ForRabbitEventNotification(logger).SendAsync);

			var cts = new CancellationTokenSource();

			Task.Run(() => Receivers.ForRabbitCommand(logger)
				.Recieve(async s => await commandDispatcher.Dispatch(await s), cts.Token), cts.Token);

			Task.Run(() => Receivers.ForRabbitEventNotification(logger)
				.Recieve(async s => await eventDispatcher.Dispatch(await s), cts.Token), cts.Token);

			//Task.Run(() => Receivers.ForRabbitApplicationEventNotification(logger)
			//	.Recieve(async s => await appEventDispatcher.Dispatch(await s), cts.Token), cts.Token);

			ConsoleKeyInfo keyInfo;

			do {
				keyInfo = Console.ReadKey();
			} while (keyInfo.Key != ConsoleKey.Q);

			cts.Cancel();
		}
	}
}
