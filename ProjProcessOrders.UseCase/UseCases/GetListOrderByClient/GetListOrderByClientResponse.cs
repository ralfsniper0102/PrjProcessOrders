namespace ProjProcessOrders.UseCase.UseCases.GetListOrderByClient
{
    public class GetListOrderByClientResponse
    {
        public List<ListOrderViewModel> Pedidos { get; set; }
        public int QtTotal { get; set; }
        public int QtPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ListOrderViewModel
    {
        public int CodigoPedido { get; set; }
        public int CodigoCliente { get; set; }
        public List<ListOrderItemViewModel> Items { get; set; }
    }

    public class ListOrderItemViewModel
    {
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
