namespace ProjProcessOrders.UseCase.UseCases.GetOrders
{
    public class GetOrdersResponse
    {
        public List<OrdersViewModel> Pedidos { get; set; }
        public int QtTotal { get; set; }
        public int QtPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class OrdersViewModel
    {
        public int CodigoPedido { get; set; }
        public int CodigoCliente { get; set; }
        public List<OrdersItemViewModel> Items { get; set; } 
    }

    public class OrdersItemViewModel
    {
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
