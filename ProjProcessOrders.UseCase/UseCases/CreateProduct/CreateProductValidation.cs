using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.CreateProduct
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.ProductPrice)
                .GreaterThan(0);
        }
    }
}
