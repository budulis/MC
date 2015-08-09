using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Subscribers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using UI.Web.Hubs;

namespace UI.Web.Modules.Order
{
	public sealed class OnOrderFailure : ISubscriber<SelfServiceOrderStartFailedNotificationMessage> {
		public async Task Notify(SelfServiceOrderStartFailedNotificationMessage evt) {
			var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
			var msg = JsonConvert.SerializeObject(new { msg = "Order failed; " + evt.Reason, type = 1 });
			var notifications = ConnectionMapping<string>.Instance.GetConnections(evt.Id.ToString())
				.Select(conn => hub.Clients.Client(conn).notify(msg)).Cast<Task>();
			await Task.WhenAll(notifications);
		}
	}
}