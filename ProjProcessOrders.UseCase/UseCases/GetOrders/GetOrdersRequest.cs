﻿using MediatR;

namespace ProjProcessOrders.UseCase.UseCases.GetOrders
{
    public class GetOrdersRequest : IRequest<GetOrdersResponse>
    {
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
