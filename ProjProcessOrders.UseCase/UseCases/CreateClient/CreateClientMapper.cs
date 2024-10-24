using AutoMapper;
using ProjProcessOrders.Domain.Entities;

namespace ProjProcessOrders.UseCase.UseCases.CreateClient
{
    public class CreateClientMapper : Profile
    {
        public CreateClientMapper()
        {
            CreateMap<CreateClientRequest, Client>();
            CreateMap<Client, CreateClientResponse>();
        }
    }
}
