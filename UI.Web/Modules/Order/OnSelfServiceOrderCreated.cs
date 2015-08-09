using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Subscribers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using UI.Web.Hubs;

namespace UI.Web.Modules.Order
{
	public sealed class OnSelfServiceOrderCreated : ISubscriber<SelfServiceOrderCreatedNotificationMessage> {
		public async Task Notify(SelfServiceOrderCreatedNotificationMessage evt) {
			var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

			var data = new { 
				evt.AmountCharged, 
				evt.Comments, 
				evt.CustomerName, 
				evt.Date, 
				evt.CardNumber, 
				evt.LoyaltyCardNumber,
				Products = evt.Products.Select(x=>x.Name +"[" + x.Price.ToString("") + "]").Aggregate((x, y) => x + "; " +y) 
			};

			var msg = JsonConvert.SerializeObject(new { msg = data, type = 2 });
			var notifications = ConnectionMapping<string>.Instance.GetConnections(evt.Id.ToString())
				.Select(conn => hub.Clients.Client(conn).notify(msg)).Cast<Task>();
			await Task.WhenAll(notifications);
		}
	}
}