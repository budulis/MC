using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ReadModel;
using Infrastructure.DataBase;

namespace Infrastructure.ReadModel
{
	public class ReceiptReadModelRepository : IReadModelRepository<ReceiptReadModel> {
		private readonly Func<DataBaseContext> _context;

		public ReceiptReadModelRepository(Func<DataBaseContext> context) {
			_context = context;
		}

		public async Task<ReceiptReadModel> GetByIdAsync(string id) {
			return await _context().SelectReceiptById.ExecuteAsync(id);
		}

		public async Task<IEnumerable<ReceiptReadModel>> GetAllAsync(int maxRecords = 100) {
			return await _context().SelectAllReceipts.ExecuteAsync(maxRecords);
		}

		public async Task AddAsync(ReceiptReadModel viewModel) {
			await _context().InsertReciept.ExecuteAsync(viewModel.Id, viewModel.Amount,viewModel.AmountCharged,viewModel.Discount,viewModel.LoyaltyCard,viewModel.Products,viewModel.PaymentType);
		}

		public void Dispose() {

		}
	}
}