using AutoMapper;
using ProjProcessOrders.Domain.Entities;

namespace ProjProcessOrders.UseCase.UseCases.GetClients
{
    public class GetClientsMapper : Profile
    {
        public GetClientsMapper()
        {
            CreateMap<Client, GetClientsResponse>();
        }
    }
}
