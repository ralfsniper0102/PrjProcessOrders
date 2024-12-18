using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.GetOrders
{
    public  class GetOrdersValidation : AbstractValidator<GetOrdersRequest>
    {
        public GetOrdersValidation()
        {
        }
    }
}
