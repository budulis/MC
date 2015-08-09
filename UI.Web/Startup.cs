using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace UI.Web
{
	public class Startup {
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR("/Content/signalr", new HubConfiguration
			{
				EnableDetailedErrors = true,
			});
			app.UseNancy();
		}
	}
}