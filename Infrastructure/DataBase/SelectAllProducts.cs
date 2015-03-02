using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Infrastructure.Services.Product;

namespace Infrastructure.DataBase {
	public sealed class SelectAllProducts : DataBaseCommand<IEnumerable<ProductInfo>> {
		public SelectAllProducts(ILogger logger) : base(logger) { }

		protected override async Task<IEnumerable<ProductInfo>> ExecuteAsyncImpl(params object[] parameters)
		{
			string query = "SELECT [ProductId],[Name],[Price],[Description] FROM [McStore].[dbo].[Products]";

			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {

				try {
					connection.Open();
				}
				catch (Exception ex) {
					Logger.Error(ex);
					throw;
				}

				var results = new List<ProductInfo>();

				using (var reader = await command.ExecuteReaderAsync()) {
					while (reader.Read()) {
						var order = new ProductInfo {
							Id = reader["ProductId"].ToString(),
							Name = reader["Name"].ToString(),
							Price = Convert.ToDecimal(reader["Price"]),
							Description = reader["Description"] != null ? reader["Description"].ToString() : null,
						};
						results.Add(order);
					}
				}
				return results;
			}
		}
	}
}