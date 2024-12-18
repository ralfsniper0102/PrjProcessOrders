using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetClientById;

public class GetClientByIdValidation : AbstractValidator<GetClientByIdRequest>
{
    public GetClientByIdValidation()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);
    }
}