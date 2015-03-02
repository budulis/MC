using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DataBase;

namespace Infrastructure.Services.Product {
	public class CachedProductInfoRepository : IItemInfoRepository<ProductInfo> {
		private static readonly object SyncRoot = new object();
		private readonly IList<ProductInfo> _data;
		public CachedProductInfoRepository(IItemInfoRepository<ProductInfo> repository) {
			_data = new List<ProductInfo>(repository.GetAllAsync().Result);
		}

		public Task<IEnumerable<ProductInfo>> GetAllAsync() {
			lock (SyncRoot)
				return Task.FromResult(_data.AsEnumerable());
		}

		public Task<ProductInfo> GetByIdAsync(string id) {
			lock (SyncRoot)
				return Task.FromResult(_data.FirstOrDefault(x => x.Id == id));
		}

		public Task<ProductInfo> GetByNameAsync(string name) {
			lock (SyncRoot)
				return Task.FromResult(_data.FirstOrDefault(x => x.Name == name));
		}
	}
}