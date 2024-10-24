using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.CreateClient
{
    public class CreateClientRequest : IRequest<CreateClientResponse>
    {
        public string Name { get; set; }
    }
}
