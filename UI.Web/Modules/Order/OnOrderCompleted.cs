using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Subscribers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using UI.Web.Hubs;

namespace UI.Web.Modules.Order {
	public sealed class OnOrderCompleted : ISubscriber<OrderCompletedNotificationMessage> {
		public async Task Notify(OrderCompletedNotificationMessage evt) {
			IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
			var msg = JsonConvert.SerializeObject(new { msg = "Order completed", type = 0 });
			var notifications = ConnectionMapping<string>.Instance.GetConnections(evt.Id.ToString())
				.Select(conn => hub.Clients.Client(conn).notify(msg)).Cast<Task>();
			await Task.WhenAll(notifications);
		}
	}
}