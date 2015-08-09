using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UI.Web.Hubs {
	public class NotificationHub : Hub {
		public Task Subscribe(string orderId) {
			ConnectionMapping<string>.Instance.Add(orderId, Context.ConnectionId);
			return Task.FromResult(true);
		}
	}
}
