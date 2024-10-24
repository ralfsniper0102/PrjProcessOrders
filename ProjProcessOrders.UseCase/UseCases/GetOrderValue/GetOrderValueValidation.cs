using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetOrderValue
{
    public class GetOrderValueValidation : AbstractValidator<GetOrderValueRequest>
    {
        public GetOrderValueValidation()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();
        }
    }
}