using Application.Common.BehavioursPipe;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Application
{
    public static class ConfigureService
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //validation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //CQRS = Mediator
            //collection add => service provider get => DI
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachedQueryBehaviours<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
    }
}
