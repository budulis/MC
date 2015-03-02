using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Product
{
	public interface IItemInfoRepository<TItem> {
		Task<IEnumerable<TItem>> GetAllAsync();
		Task<TItem> GetByIdAsync(string id);
		Task<TItem> GetByNameAsync(string name);
	}
}