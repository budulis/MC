using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UI.Web.Hubs {
	public class NotificationHub : Hub {
		private readonly static ConnectionMapping<string> Connections;
		static NotificationHub() {
			Connections = new ConnectionMapping<string>();
		}
		public Task Subscribe(string orderId) {
			Connections.Add(orderId, Context.ConnectionId);
			return Task.FromResult(true);
		}

		public async Task Notify() {
			Clients.Others.clear();
			await Task.FromResult(true);
		}
	}
}
