using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core;

namespace Infrastructure.DataBase {
	public sealed class SelectEventCountByKey : DataBaseCommand<int> {
		public SelectEventCountByKey(ILogger logger)
			: base(logger) {
		}

		protected override async Task<int> ExecuteAsyncImpl(params object[] parameters) {
			var query = "SELECT COUNT(*) FROM [McStore].[dbo].[Events] WHERE EventKey = @EventKey";
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString()))
			using (var command = new SqlCommand(query, connection)) {
				command.Parameters.AddWithValue("@EventKey", parameters[0]);
				try {
					connection.Open();
					return Convert.ToInt32(await command.ExecuteScalarAsync());
				}
				catch (Exception ex) {
					Logger.Error(ex);
					throw;
				}
			}
		}
	}
}