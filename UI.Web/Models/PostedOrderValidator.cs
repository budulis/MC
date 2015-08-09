using FluentValidation;

namespace UI.Web.Models
{
	public class PostedOrderValidator : AbstractValidator<PostedOrder>
	{
		public PostedOrderValidator()
		{
			RuleFor(x => x.ProductIDs).NotEmpty().WithMessage("[Select at least one product]");
			RuleFor(x => x.CardNumber).NotEmpty().WithMessage("[Enter payment card's number]");
		}
	}
}