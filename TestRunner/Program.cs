using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Commands;
using Infrastructure.DataBase;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Product;

namespace TestRunner {
	class Program {
		private static IDomainCommandDispatcher _domainCommandDispatcher;
		private static readonly ILogger Logger = LoggerFactory.Default;

		static void Main() {
			//var appEventDispatcher = EventDispathers.Application.GetQueued(Logger);
			//_domainCommandDispatcher = CommandDispatchers.GetDirect(EventDispathers.Domain.GetDirect(() => _domainCommandDispatcher, () => appEventDispatcher, Logger), Logger);
			//TestDomainModel();
		}


		private static void TestDomainModel() {

			var buffer = new BufferBlock<Id>();
			var consumer = Consume(buffer);

			Produce(buffer);
			consumer.Wait();

			Console.WriteLine("Ordering done!");
			Console.Read();
		}

		private static void Produce(ITargetBlock<Id> target) {
			var rnd = new Random();
			IItemInfoRepository<ProductInfo> products = new CachedProductInfoRepository(new ProductInfoRepository(new DataBaseContext(LoggerFactory.Get<NullLogger>())));
			Parallel.ForEach(Enumerable.Range(0, 10), new ParallelOptions { MaxDegreeOfParallelism = 8 }, async x => {
				var id = new Id(Guid.NewGuid());
				var prods = (await products.GetAllAsync()).Select(p => new Product(p.Id, p.Name, p.Price));
				var create = new CreateOrder(id, prods.ToArray(), "John Doe", "Some comments", "CA000000-0000-0000-0000-000000000001");

				await _domainCommandDispatcher.Dispatch(create);
				await Task.Delay(rnd.Next(500, 1500));
				target.Post(id);
			});

			//target.Complete();
		}

		private static async Task Consume(ISourceBlock<Id> source) {
			while (await source.OutputAvailableAsync()) {
				var start = new PayForOrder(source.Receive(), 200M, PaymentType.Cash, LoyaltyCardType.Silver, CardType.Credit, "4344-5555-4777-5555");
				await _domainCommandDispatcher.Dispatch(start);
				Logger.Audit(start.Id);
			}
		}
	}
}
