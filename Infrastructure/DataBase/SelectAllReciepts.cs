using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;
using Core.ReadModel;

namespace Infrastructure.DataBase
{
	public sealed class SelectAllReciepts : DataBaseCommand<IEnumerable<ReceiptReadModel>> {
		public SelectAllReciepts(ILogger logger) : base(logger) { }

		protected override async Task<IEnumerable<ReceiptReadModel>> ExecuteAsyncImpl(params object[] parameters) {

			var query = "SELECT TOP " + Convert.ToInt32(parameters[0] ?? 100) + " [ReceiptId],[LoyaltyCard],[AmountCharged],[Discount],[Products],[Amount] FROM [McStore].[dbo].[Receipts] ORDER BY ReceiptId desc";

			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {

				try {
					connection.Open();
				}
				catch (Exception ex) {
					Logger.Error(ex);
					throw;
				}

				var orders = new List<ReceiptReadModel>();

				using (var reader = await command.ExecuteReaderAsync()) {
					while (reader.Read()) {
						var receipt = new ReceiptReadModel {
							Id = reader["ReceiptId"].ToString(),
							Products = reader["Products"] == null ? null : reader["Products"].ToString(),
							Amount = Convert.ToDecimal(reader["Amount"]),
							AmountCharged = Convert.ToDecimal(reader["AmountCharged"]),
							Discount = Convert.ToDecimal(reader["Discount"]),
							LoyaltyCard = reader["LoyaltyCard"] == null ? null : reader["LoyaltyCard"].ToString(),
						};
						orders.Add(receipt);
					}
				}
				return orders;
			}
		}
	}
}