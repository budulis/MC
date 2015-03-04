using Core;
using Core.ReadModel;
using Infrastructure.DataBase;
using Infrastructure.ReadModel;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Product;
using Nancy;
using Nancy.TinyIoc;

namespace UI.Web {
	public class Bootstrapper : DefaultNancyBootstrapper {
		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			base.ConfigureApplicationContainer(container);
			container.Register(LoggerFactory.Default);
			container.Register<DataBaseContext>().AsMultiInstance();
			container.Register<IReadModelRepository<OrderReadModel>, OrderReadModelRepository>().AsMultiInstance();
			container.Register<IItemInfoRepository<ProductInfo>, ProductInfoRepository>().AsMultiInstance();
		}
	}
}