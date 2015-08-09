using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;
using Core.ReadModel;

namespace Infrastructure.DataBase
{
	public sealed class SelectReceiptById : DataBaseCommand<ReceiptReadModel> {
		public SelectReceiptById(ILogger logger)
			: base(logger) {
			}

		protected override async Task<ReceiptReadModel> ExecuteAsyncImpl(params object[] parameters) {

			ReceiptReadModel receipt = null;

			const string query = "SELECT TOP 1 [ReceiptId],[LoyaltyCard],[AmountCharged],[Discount],[Products],[Amount] FROM [McStore].[dbo].[Receipts] where ReceiptId = @ReceiptId";
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {

				command.Parameters.AddWithValue("@ReceiptId", parameters[0]);
				try {
					connection.Open();
					using (var reader = await command.ExecuteReaderAsync()) {
						while (reader.Read()) {
							receipt = new ReceiptReadModel {
								Id = reader["ReceiptId"].ToString(),
								Products = reader["Products"] == null ? null : reader["Products"].ToString(),
								Amount = Convert.ToDecimal(reader["Amount"]),
								AmountCharged = Convert.ToDecimal(reader["AmountCharged"]),
								Discount = Convert.ToDecimal(reader["Discount"]),
								LoyaltyCard = reader["LoyaltyCard"] == null ? null : reader["LoyaltyCard"].ToString(),
							};
						}
					}
				}
				catch (Exception ex) {
					Logger.Error(ex);
					throw;
				}

			}
			return receipt;
		}
	}
}