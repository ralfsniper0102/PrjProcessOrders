using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.CreateClient
{
    public class CreateClienRequestHandler : IRequestHandler<CreateClientRequest, CreateClientResponse>
    {
        private readonly IGenericRepository<Client, int> _clientRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public CreateClienRequestHandler(IGenericRepository<Client, int> clientRepository, IMapper mapper, Resources resources)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<CreateClientResponse> Handle(CreateClientRequest request, CancellationToken cancellationToken)
        {
            var result = await _clientRepository.Queryable(x => x.Name == request.Name)
                                    .ToListAsync(cancellationToken: cancellationToken);

            if (result.Count > 0)
                throw new ConflictException(_resources.NameAlreadyExists());

            var newClient = _mapper.Map<Client>(request);
            var res = await _clientRepository.InsertAsync(newClient, cancellationToken);

            var createClientResponse = new CreateClientResponse();
            createClientResponse.Id = res.Id;

            return createClientResponse;
        }
    }
}
