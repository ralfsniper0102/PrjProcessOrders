using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Domain.Entities;
using ProjProcessOrders.Exception.Exceptions;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.UseCase.Interfaces;

namespace ProjProcessOrders.UseCase.UseCases.DeleteClient
{
    public class DeleteClientRequestHandler : IRequestHandler<DeleteClientRequest, Unit>
    {
        private readonly IGenericRepository<Client, int> _clientRepository;
        private readonly Resources _resources;

        public DeleteClientRequestHandler(IGenericRepository<Client, int> clientRepository, Resources resources)
        {
            _clientRepository = clientRepository;
            _resources = resources;
        }

        public async Task<Unit> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
        {
            var result = await _clientRepository.Queryable(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ClientNotExists());

            await _clientRepository.DeleteAsync(result);

            return Unit.Value;
        }
    }
}
