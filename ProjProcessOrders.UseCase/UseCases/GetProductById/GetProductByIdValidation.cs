using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetProductById;

public class GetProductByIdValidation: AbstractValidator<GetProductByIdRequest>
{
    public GetProductByIdValidation()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);
    }
}