using AutoMapper;
using ProjProcessOrders.Domain.Entities;

namespace ProjProcessOrders.UseCase.UseCases.GetClientById
{
    public class GetClientByIdMapper : Profile
    {
        public GetClientByIdMapper()
        {
            CreateMap<Client, GetClientByIdResponse>();
        }
    }
}
