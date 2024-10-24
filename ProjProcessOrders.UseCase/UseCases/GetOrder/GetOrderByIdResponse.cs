namespace ProjProcessOrders.UseCase.UseCases.GetOrder
{
    public class GetOrderByIdResponse
    {
        public int CodigoPedido { get; set; }
        public int CodigoCliente { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }

    public class OrderItemViewModel
    {
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}

