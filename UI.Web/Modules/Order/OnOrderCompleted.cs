using System;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering.Events;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Subscribers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using UI.Web.Hubs;

namespace UI.Web.Modules.Order {
	public sealed class OnOrderCompleted : ISubscriber<OrderCompletedNotificationMessage> {
		public async Task Notify(OrderCompletedNotificationMessage evt) {
			IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
			//Get info from ConnectionMap object to send to a specific client
			var msg = JsonConvert.SerializeObject(new { msg = "Order [" + evt.Id + "] completed", type = 0 });
			await hub.Clients.All.notify(msg);
		}
	}
}