using FluentValidation;
using ProjProcessOrders.UseCase.DTO.Validations;

namespace ProjProcessOrders.UseCase.UseCases.UpdateOrder
{
    public class UpdateOrderValidation : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderValidation() 
        {
            RuleFor(x => x.OrderId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .GreaterThan(0);

            RuleForEach(x => x.OrderProducts)
                .SetValidator(new OrderProductDTOValidation());

        }
    }
}
