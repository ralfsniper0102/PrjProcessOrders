using AutoMapper;
using ProjProcessOrders.Domain.Entities;

namespace ProjProcessOrders.UseCase.UseCases.GetOrderById
{
    public class GetOrderByIdMapper : Profile
    {
        public GetOrderByIdMapper()
        {
            CreateMap<Order, GetOrderByIdResponse>();
        }
    }
}
