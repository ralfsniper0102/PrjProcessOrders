using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.UpdateClient;

public class UpdateClientValidation : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientValidation()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(3);
    }
}