using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProjProcessOrders.Application.Shared.Behavior;
using ProjProcessOrders.UseCase.UseCases.CreateClient;

namespace ProjProcessOrders.Application.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicantionApp(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateClientRequest>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
