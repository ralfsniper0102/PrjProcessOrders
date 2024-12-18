using AutoMapper;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.UseCase.DTO;

namespace ProjProcessOrders.UseCase.UseCases.CreateOrder
{
    public class CreateOrderMapper : Profile
    {
        public CreateOrderMapper()
        {
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src =>
                    src.OrderProducts.Select(op => new OrderProduct
                    {
                        ProductId = op.ProductId,
                        Quantity = op.Quantity
                    })
                ));

            CreateMap<OrderProductDTO, OrderProduct>();
        }
    }
}
