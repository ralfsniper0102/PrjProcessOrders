using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetClientById
{
    public class GetClientByIdRequest : IRequest<GetClientByIdResponse>
    {
        public int Id { get; set; }
    }
}
