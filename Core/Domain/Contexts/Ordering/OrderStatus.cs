namespace Core.Domain.Contexts.Ordering
{
	public enum OrderStatus {
		New, WaitingForPayment, Payed, InProcess, Completed,Failed
	}
}