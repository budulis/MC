using System;
using Core.ReadModel;
using Infrastructure.DataBase;
using Infrastructure.Services.Logging;

namespace Infrastructure.ReadModel
{
	public class ReadModelRepositoryFactory
	{
		public IReadModelRepository<TReadModel> Get<TReadModel>() where TReadModel : IReadModel
		{
			if (typeof (TReadModel) == typeof (OrderReadModel))
				//return (IReadModelRepository<TReadModel>)new OrderReadModelRepository(() => new DataBaseContext(new NullLogger()));
				return (IReadModelRepository<TReadModel>)new InMemoryReadModelRepository();

			if (typeof(TReadModel) == typeof(ReceiptReadModel))
				return (IReadModelRepository<TReadModel>)new ReceiptReadModelRepository(() => new DataBaseContext(new NullLogger()));

			throw new NotSupportedException();
		} 
	}
}