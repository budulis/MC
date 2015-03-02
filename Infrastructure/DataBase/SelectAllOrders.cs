using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;
using Core.ReadModel;

namespace Infrastructure.DataBase {
	public sealed class SelectAllOrders : DataBaseCommand<IEnumerable<OrderReadModel>>{
		public SelectAllOrders(ILogger logger) : base(logger) { }

		protected override async Task<IEnumerable<OrderReadModel>> ExecuteAsyncImpl(params object[] parameters) {

			var query = "SELECT TOP " + Convert.ToInt32(parameters[0] ?? 100) + " [OrderId],[Products],[Total],[Status],[Created] FROM [McStore].[dbo].[Orders] ORDER BY Created desc,OrderId ";

			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {

				try {
					connection.Open();
				}
				catch (Exception ex) {
					Logger.Error(ex);
					throw;
				}

				var orders = new List<OrderReadModel>();

				using (var reader =await command.ExecuteReaderAsync()) {
					while (reader.Read()) {
						var order = new OrderReadModel {
							Id = reader["OrderId"].ToString(),
							Products = reader["Products"] == null ? null : reader["Products"].ToString(),
							Total = Convert.ToDecimal(reader["Total"]),
							Status = reader["Status"].ToString(),
							Created = Convert.ToDateTime(reader["Created"]),
						};
						orders.Add(order);
					}
				}
				return orders;
			}
		}
	}
}
