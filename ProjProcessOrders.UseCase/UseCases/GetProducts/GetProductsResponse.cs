namespace ProjProcessOrders.UseCase.UseCases.GetProducts
{
    public class GetProductsResponse
    {
        public List<ProductViewModel> Products { get; set; }
        public int QtTotal { get; set; }
        public int QtPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
