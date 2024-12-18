using AutoMapper;
using ProjProcessOrders.Domain.Entities;

namespace ProjProcessOrders.UseCase.UseCases.GetProductById
{
    public class GetProductByIdMapper : Profile
    {
        public GetProductByIdMapper()
        {
            CreateMap<Product, GetProductByIdResponse>();
        }
    }
}
