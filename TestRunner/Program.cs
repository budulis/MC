using System.Collections.Concurrent;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Commands;
using Infrastructure.DataBase;
using Infrastructure.Dispatchers;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Product;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


namespace TestRunner {

	abstract class Base {
		
		private Base() {
			Console.WriteLine("B.ctor");
		}

		public class Derived : Base
		{
			public Derived()
			{
				Console.WriteLine("D.ctor");
			}
		}
	}


	class Program {
		private static IDomainCommandDispatcher _domainCommandDispatcher;
		private static readonly ILogger Logger = LoggerFactory.Default;

		static void Main() {
			
			Base b= new Base.Derived();


			//_domainCommandDispatcher = CommandDispatchers.GetDirect(EventDispathers.Domain.GetDirect(() => _domainCommandDispatcher, Logger), Logger);
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
			//var rnd = new Random();
			IItemInfoRepository<ProductInfo> products = new CachedProductInfoRepository(new ProductInfoRepository(new DataBaseContext(LoggerFactory.Get<NullLogger>())));
			Parallel.ForEach(Enumerable.Range(0, 1000), new ParallelOptions { MaxDegreeOfParallelism = 8 }, async x => {
				var id = new Id(Guid.NewGuid());
				var prods = (await products.GetAllAsync()).Select(p => new Product(p.Id, p.Name, p.Price));
				var create = new CreateOrder(id, prods.ToArray(), "John Doe", "Some comments", "CA000000-0000-0000-0000-000000000001");

				await _domainCommandDispatcher.Dispatch(create);
				//Thread.Sleep(rnd.Next(100, 800));
				target.Post(id);
			});

			target.Complete();
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
