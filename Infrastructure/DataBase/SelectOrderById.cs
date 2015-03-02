using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;
using Core.ReadModel;

namespace Infrastructure.DataBase {
	public sealed class SelectOrderById : DataBaseCommand<OrderReadModel> {
		public SelectOrderById(ILogger logger)
			: base(logger) {
		}

		protected override async Task<OrderReadModel> ExecuteAsyncImpl(params object[] parameters) {

			OrderReadModel order = null;

			const string query = "SELECT TOP 1 [OrderId],[Products],[Total],[Status],[Created] FROM [McStore].[dbo].[Orders] where OrderId = @OrderId ORDER BY Id DESC ";
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {

				command.Parameters.AddWithValue("@OrderId", parameters[0]);
				try {
					connection.Open();
					using (var reader = await command.ExecuteReaderAsync()) {
						while (reader.Read()) {
							order = new OrderReadModel {
								Id = reader["OrderId"].ToString(),
								Products = reader["Products"] == null ? null : reader["Products"].ToString(),
								Total = Convert.ToDecimal(reader["Total"]),
								Status = reader["Status"].ToString(),
								Created = Convert.ToDateTime(reader["Created"])
							};
						}
					}
				}
				catch (Exception ex) {
					Logger.Error(ex);
					throw;
				}

			}
			return order;
		}
	}
}