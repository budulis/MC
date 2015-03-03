using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Messages;
using Core.ReadModel;
using Core.Subscribers;

namespace Infrastructure.Services.Reporting
{
	public sealed class OnOrderStartedCreateReceipt : Subscriber<OrderStartedApplicationNotificationMessage,ReceiptReadModel> {

		public OnOrderStartedCreateReceipt(IReadModelRepository<ReceiptReadModel> readModelRepository)
			: base(null, readModelRepository) {
			}

		public override async Task Notify(OrderStartedApplicationNotificationMessage evt)
		{
			var receipt = new ReceiptReadModel
			{
				Id = evt.Id.ToString(),
				Amount = evt.Amount,
				LoyaltyCard = evt.LoyaltyCard,
				AmountCharged = evt.AmountCharged,
				Discount = evt.Discount,
				Products = String.Join(";", evt.Products.Select(x=>x.ToString())),
				PaymentType = evt.Payment
			};
			await ReadModelRepository.AddAsync(receipt);
		}
	}
}