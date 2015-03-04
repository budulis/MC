using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DataBase;

namespace Infrastructure.Services.Product {
	public class ProductInfoRepository : IItemInfoRepository<ProductInfo> {
		private readonly DataBaseContext _db;

		public ProductInfoRepository(DataBaseContext db) {
			_db = db;
		}

		public async Task<IEnumerable<ProductInfo>> GetAllAsync() {
			return await _db.SelectAllProducts.ExecuteAsync();
		}

		public async Task<ProductInfo> GetByIdAsync(string id) {
			return (await _db.SelectAllProducts.ExecuteAsync()).FirstOrDefault(x => x.Id == id);
		}

		public async Task<ProductInfo> GetByNameAsync(string name) {
			return (await _db.SelectAllProducts.ExecuteAsync()).FirstOrDefault(x => x.Name == name);
		}
	}
}