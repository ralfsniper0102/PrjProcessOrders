using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.DeleteProduct
{
    public class DeleteProductValidation : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductValidation()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
        }
    }
}
