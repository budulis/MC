using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.ReadModel;

namespace Infrastructure.ReadModel
{
	public class InMemoryReadModelRepository : IReadModelRepository<OrderReadModel>
	{
		private static readonly BlockingCollection<OrderReadModel> Data;

		static InMemoryReadModelRepository()
		{
			Data = new BlockingCollection<OrderReadModel>();
		}

		public Task<OrderReadModel> GetByIdAsync(string id)
		{
			return Task.FromResult(Data.First(x => x.Id == id));
		}

		public Task<IEnumerable<OrderReadModel>> GetAllAsync(int maxRecords = 100)
		{
			return Task.FromResult(Data.Take(maxRecords));
		}

		public Task AddAsync(OrderReadModel viewModel)
		{
			Data.Add(viewModel);
			return Task.FromResult(true);
		}
	}
}