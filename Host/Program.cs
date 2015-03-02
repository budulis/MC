using System.Security.Cryptography.X509Certificates;
using Core;
using Infrastructure.Dispatchers;
using Infrastructure.Initialization;
using Infrastructure.Queuing;
using Infrastructure.Services.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Host {
	class Program {

		static void Main(string[] args) {

			Bootstrapper.Default.RunInitializationTasks();
			var logger = LoggerFactory.Default;

			var commandDispatcher = CommandDispatcher.GetDirect(EventDispather.Domain.GetQueued(logger),logger);
			var eventDispatcher = EventDispather.Domain.GetDirect(() => commandDispatcher, logger);

			var cts = new CancellationTokenSource();

			Task.Run(() => Reciever.ForRabbitCommand(logger).Recieve(async s => await commandDispatcher.Dispatch(await s), cts.Token), cts.Token);

			Task.Run(() => Reciever.ForRabbitEventNotification(logger).Recieve(async s => await eventDispatcher.Dispatch(await s), cts.Token), cts.Token);

			ConsoleKeyInfo keyInfo;

			do {
				keyInfo = Console.ReadKey();
			} while (keyInfo.Key != ConsoleKey.Q);

			cts.Cancel();

		}
	}
}
