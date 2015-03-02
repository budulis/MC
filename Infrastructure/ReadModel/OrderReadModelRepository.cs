using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;
using Core.ReadModel;
using Infrastructure.DataBase;

namespace Infrastructure.ReadModel {
	public class OrderReadModelRepository : IReadModelRepository<OrderReadModel> {
		private readonly Func<DataBaseContext> _context;

		public OrderReadModelRepository(Func<DataBaseContext> context) {
			_context = context;
		}

		public async Task<OrderReadModel> GetByIdAsync(string id) {
			return await _context().SelectOrderById.ExecuteAsync(id);
		}

		public async Task<IEnumerable<OrderReadModel>> GetAllAsync(int maxRecords = 100) {
			return await _context().SelectAllOrders.ExecuteAsync(maxRecords);
		}

		public async Task AddAsync(OrderReadModel viewModel) {
			await _context().InsertOrder.ExecuteAsync(viewModel.Id, viewModel.Products, viewModel.Total, viewModel.Status);
		}

		public void Dispose() {

		}
	}
}