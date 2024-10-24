namespace ProjProcessOrders.UseCase.UseCases.GetClients
{
    public class GetClientsResponse
    {
        public List<ClientViewModel> Clients { get; set; }
        public int QtTotal { get; set; }
        public int QtPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ClientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
