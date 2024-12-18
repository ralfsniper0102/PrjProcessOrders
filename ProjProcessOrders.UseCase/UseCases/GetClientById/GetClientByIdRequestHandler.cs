using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.GetClientById
{
    public class GetClientByIdRequestHandler : IRequestHandler<GetClientByIdRequest, GetClientByIdResponse>
    {
        private readonly IGenericRepository<Client, int> _clientRepository;
        private readonly IMapper _mapper;
        private readonly Resources _resources;

        public GetClientByIdRequestHandler(IGenericRepository<Client, int> clientRepository, IMapper mapper, Resources resources)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _resources = resources;
        }

        public async Task<GetClientByIdResponse> Handle(GetClientByIdRequest request, CancellationToken cancellationToken)
        {
            var result = await _clientRepository.Queryable(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ClientNotExists());

            return _mapper.Map<GetClientByIdResponse>(result);
        }
    }
}
