using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.DeleteOrder;

public class DeleteOrderValidation : AbstractValidator<DeleteOrderRequest>
{
    public DeleteOrderValidation()
    {
        RuleFor(x => x.OrderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);
    }
}