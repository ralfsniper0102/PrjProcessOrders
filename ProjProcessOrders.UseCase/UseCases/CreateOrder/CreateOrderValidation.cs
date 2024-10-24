using FluentValidation;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0);

        RuleFor(x => x.OrderProducts)
            .NotEmpty();

        RuleForEach(x => x.OrderProducts).SetValidator(new OrderProductViewModelValidator());
    }
}

public class OrderProductViewModelValidator : AbstractValidator<OrderProductViewModel>
{
    public OrderProductViewModelValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);
    }
}
