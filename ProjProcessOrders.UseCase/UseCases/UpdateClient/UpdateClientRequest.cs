using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.UpdateClient
{
    public class UpdateClientRequest : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
