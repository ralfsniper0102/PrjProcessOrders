using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.UpdateProduct
{
    public class UpdateProductValidation : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidation()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.ProductName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.ProductPrice)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
