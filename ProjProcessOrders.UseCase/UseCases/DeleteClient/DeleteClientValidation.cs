using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.DeleteClient;

public class DeleteClientValidation : AbstractValidator<DeleteClientRequest>
{
    public DeleteClientValidation()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);
    }
}