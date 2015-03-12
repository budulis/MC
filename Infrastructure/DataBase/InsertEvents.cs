using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Core;
using Core.FclExtensions;

namespace Infrastructure.DataBase {
	public sealed class InsertEvents : DataBaseCommand<int> {
		public InsertEvents(ILogger logger)
			: base(logger) {
		}

		protected override async Task<int> ExecuteAsyncImpl(params object[] parameters)
		{
			return await SingleThreadedSynchronizationContext.Execute(() => Execute(parameters));
		}

		private async Task<int> Execute(params object[] parameters)
		{
			var query = "INSERT INTO [McStore].[dbo].[Events]([EventId],[EventKey],[Data],[Created])VALUES(@EventId,@EventKey,@EventData,GETDATE())";
			try {
				using (var tran = new TransactionScope())
				using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["McStoreConnection"].ToString())) {
					var data = (IEnumerable<string>)parameters[2];
					connection.Open();
					var rowsAffected = 0;
					foreach (var d in data) {
						var command = new SqlCommand(query, connection);
						command.Parameters.AddWithValue("@EventId", parameters[0]);
						command.Parameters.AddWithValue("@EventKey", parameters[1]);
						command.Parameters.AddWithValue("@EventData", d);
						rowsAffected += await command.ExecuteNonQueryAsync();
					}
					tran.Complete();
					return rowsAffected;
				}
			}
			catch (Exception ex) {
				Logger.Error(ex);
				throw;
			}
		}
	}
}