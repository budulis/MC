using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using Core;
using Core.FclExtensions;

namespace Infrastructure.DataBase
{
	public sealed class InsertReciept : DataBaseCommand<int> {
		public InsertReciept(ILogger logger) : base(logger) { }

		protected override async Task<int> ExecuteAsyncImpl(params object[] parameters) {
			return await SingleThreadedSynchronizationContext.Execute(() => Execute(parameters));
		}

		private async Task<int> Execute(params object[] parameters) {
			const string query = "INSERT INTO [McStore].[dbo].[Receipts] VALUES(@ReceiptId,@LoyaltyCard,@AmountCharged,@Discount,@Products,@Amount,DEFAULT,@PaymentType)";
			var rowsAffected = 0;
			using (var tran = new TransactionScope())
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {

				command.Parameters.AddWithValue("@ReceiptId", parameters[0]);
				command.Parameters.AddWithValue("@Amount", parameters[1]??DBNull.Value);
				command.Parameters.AddWithValue("@AmountCharged", parameters[2]);
				command.Parameters.AddWithValue("@Discount", parameters[3]);
				command.Parameters.AddWithValue("@LoyaltyCard", parameters[4]);
				command.Parameters.AddWithValue("@Products", parameters[5]);
				command.Parameters.AddWithValue("@PaymentType", parameters[6]);

				try {
					connection.Open();
					rowsAffected = await command.ExecuteNonQueryAsync();
					tran.Complete();
				}
				catch (Exception ex) {
						Logger.Error(ex);
				}
			}
			return rowsAffected;
		}
	}
}