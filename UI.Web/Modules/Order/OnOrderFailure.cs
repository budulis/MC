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
			IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
			//Get info from ConnectionMap object to send to a specific client
			var msg = JsonConvert.SerializeObject(new { msg = "Order [" + evt.Id + "] failed; " + evt.Reason, type = 1 });
			await hub.Clients.All.notify(msg);
		}
	}
}