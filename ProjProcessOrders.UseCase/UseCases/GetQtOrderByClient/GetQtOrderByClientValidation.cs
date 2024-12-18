using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetQtOrderByClient
{
    public class GetQtOrderByClientValidation : AbstractValidator<GetQtOrderByClientRequest>
    {
        public GetQtOrderByClientValidation()
        {
            RuleFor(x => x.ClientId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
