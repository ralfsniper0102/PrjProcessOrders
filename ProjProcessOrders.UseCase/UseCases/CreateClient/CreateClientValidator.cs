using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.CreateClient
{
    public class CreateClientValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
