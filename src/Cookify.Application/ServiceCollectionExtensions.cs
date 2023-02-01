using System.Reflection;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Pipelines;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cookify.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
    
    public static IServiceCollection AddTransactionPipeline<TCommand, TResponse>(this IServiceCollection services) 
        where TCommand : ICommand<TResponse>
    {
       
        services.AddScoped(
            serviceType: typeof(IPipelineBehavior<TCommand, TResponse>), 
            implementationType: typeof(TransactionPipeline<TCommand, TResponse>)
        );
        return services;
    }
}