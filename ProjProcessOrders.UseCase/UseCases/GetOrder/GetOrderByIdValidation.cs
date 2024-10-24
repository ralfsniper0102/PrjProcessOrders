using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetOrder
{
    public class GetOrderByIdValidation : AbstractValidator<GetOrderByIdRequest>
    {
        public GetOrderByIdValidation()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();
        }
    }
}
