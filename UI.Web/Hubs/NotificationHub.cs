using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace UI.Web.Hubs {
	class NotificationHub : Hub {

		public Task Subscribe(string id) {
			return Task.FromResult(true);
		}

		public Task BroadcastClear() {
			return Clients.Others.clear();
		}

	}
}
