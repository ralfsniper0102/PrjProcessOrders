using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetOrderById
{
    public class GetOrderByIdValidation : AbstractValidator<GetOrderByIdRequest>
    {
        public GetOrderByIdValidation()
        {
            RuleFor(x => x.OrderId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
