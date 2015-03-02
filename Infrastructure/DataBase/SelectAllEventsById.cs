using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;
using Core.ReadModel;

namespace Infrastructure.DataBase {
	public sealed class SelectAllEventsByKey : DataBaseCommand<IEnumerable<string>> {
		public SelectAllEventsByKey(ILogger logger)
			: base(logger) {
		}

		protected override async Task<IEnumerable<string>> ExecuteAsyncImpl(params object[] parameters) {
			var query = "SELECT [Id], [Data] FROM [McStore].[dbo].[Events] WHERE EventKey = @EventKey ORDER BY Id ASC";

			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {
				command.Parameters.AddWithValue("@EventKey", parameters[0]);
				try {
					connection.Open();
				}
				catch (Exception ex) {
					Logger.Error(ex);
					throw;
				}

				var result = new List<string>();

				using (var reader = await command.ExecuteReaderAsync()) {
					while (reader.Read())
						result.Add(reader["Data"].ToString());
				}

				return result;
			}
		}
	}
}