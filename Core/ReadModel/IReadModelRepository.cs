using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.ReadModel
{
	public interface IReadModelRepository<TViewModel> where TViewModel : IReadModel
	{
		Task<TViewModel> GetByIdAsync(string id);
		Task<IEnumerable<TViewModel>> GetAllAsync(int maxRecords = 100);
		Task AddAsync(TViewModel viewModel);
	}
}