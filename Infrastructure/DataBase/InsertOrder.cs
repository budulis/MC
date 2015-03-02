using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using Core;
using Core.FclExtensions;

namespace Infrastructure.DataBase
{
	public sealed class InsertOrder : DataBaseCommand<int> {
		public InsertOrder(ILogger logger) :base(logger){}

		protected override async Task<int> ExecuteAsyncImpl(params object[] parameters) {
			return await SingleThreadedSynchronizationContext.Execute(()=>Execute(parameters));
		}

		private async Task<int> Execute(params object[] parameters) {
			const string query = "INSERT INTO [Orders] VALUES(@OrderId,@Products,@Total,@Status,DEFAULT)";
			var rowsAffected = 0;
			using (var tran = new TransactionScope())
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {

				command.Parameters.AddWithValue("@OrderId", parameters[0]);
				command.Parameters.AddWithValue("@Products", parameters[1]);
				command.Parameters.AddWithValue("@Total", parameters[2]);
				command.Parameters.AddWithValue("@Status", parameters[3]);
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