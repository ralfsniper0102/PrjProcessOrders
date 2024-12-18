using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.UpdateClient
{
    public class UpdateClientRequestHandler : IRequestHandler<UpdateClientRequest, Unit>
    {
        private readonly IGenericRepository<Client, int> _clientRepository;
        private readonly Resources _resources;

        public UpdateClientRequestHandler(IGenericRepository<Client, int> clientRepository, Resources resources)
        {
            _clientRepository = clientRepository;
            _resources = resources;
        }

        public async Task<Unit> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var result = await _clientRepository.Queryable(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ClientNotExists());

            result.Name = request.Name;

            var resultName = await _clientRepository.Queryable(x => x.Name == result.Name).FirstOrDefaultAsync(cancellationToken);

            if (resultName != null)
                throw new ConflictException(_resources.ClientAlreadyExists());

            await _clientRepository.UpdateAsync(result, cancellationToken);

            return Unit.Value;
        }
    }
}
