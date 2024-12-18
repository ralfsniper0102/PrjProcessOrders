using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.DeleteClient
{
    public class DeleteClientRequest : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
