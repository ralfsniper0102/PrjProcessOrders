using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetListOrderByClient
{
    public class GetListOrderByClientValidation : AbstractValidator<GetListOrderByClientRequest>
    {
        public GetListOrderByClientValidation()
        {
            RuleFor(x => x.ClientId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
