using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjProcessOrders.UseCase.UseCases.GetListOrderByClient
{
    public class GetListOrderByClientValidation : AbstractValidator<GetListOrderByClientRequest>
    {
        public GetListOrderByClientValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty();
        }
    }
}
