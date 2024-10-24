using AutoMapper;
using ProjProcessOrders.Domain.Entities;

namespace ProjProcessOrders.UseCase.UseCases.GetOrder
{
    public class GetOrderByIdMapper : Profile
    {
        public GetOrderByIdMapper()
        {
            CreateMap<Order, GetOrderByIdResponse>();
        }
    }
}
