using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
	public interface IInfrastructureService
	{
		IEventStore EventStore { get; }
		IDiscountService DiscountService{ get; }
		ICardPaymentService CardPaymentService{ get; }
		ILogger Logger { get; }
		ICashierRepository Cashiers { get; }
		IChefRepository Chefs { get; }
		ISelfServicePaymentService SelfServicePaymentService { get; }
	}
}
