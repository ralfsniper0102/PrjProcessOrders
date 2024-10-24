using AutoMapper;
using MediatR;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProjProcessOrders.UseCase.UseCases.GetClients
{
    public class GetClientsRequestHandler : IRequestHandler<GetClientsRequest, GetClientsResponse>
    {
        private readonly IGenericRepository<Client, int> _clientRepository;
        private readonly IMapper _mapper;

        public GetClientsRequestHandler(IGenericRepository<Client, int> clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<GetClientsResponse> Handle(GetClientsRequest request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            int pageSize = request.PageSize;

            int skip = Math.Max(page - 1, 0) * pageSize;

            var query = await _clientRepository.Queryable(x => x.Id > 0)
                                               .OrderByDescending(x => x.Id)
                                               .AsNoTracking()
                                               .ToListAsync(cancellationToken: cancellationToken);

            switch (request.Search)
            {
                case "":
                    break;
                default:
                    query = query.Where(client =>
                    client.Id.ToString().Contains(request.Search.ToLower()) ||
                    client.Name.ToLower().Contains(request.Search.ToLower()))
                        .Distinct()
                        .ToList();
                    break;
            }

            var qt = query.Count();
            var qtPages = (int)Math.Ceiling((double)qt / pageSize);

            query = query
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            List<ClientViewModel> clients = new List<ClientViewModel>();

            foreach (var client in query)
            {
                ClientViewModel clientViewModel = new ClientViewModel
                {
                    Id = client.Id,
                    Name = client.Name
                };

                clients.Add(clientViewModel);
            }

            GetClientsResponse getClientsResponse = new GetClientsResponse
            {
                Page = page,
                PageSize = pageSize,
                QtTotal = qt,
                QtPages = Math.Max(qtPages, 1),
                Clients = clients
            };

            return getClientsResponse;
        }
    }
}
