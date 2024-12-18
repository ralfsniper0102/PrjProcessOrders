using FluentValidation;
using ProjProcessOrders.UseCase.DTO.Validations;
using ProjProcessOrders.UseCase.UseCases.CreateOrder;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleForEach(x => x.OrderProducts)
            .SetValidator(new OrderProductDTOValidation());
    }
}
