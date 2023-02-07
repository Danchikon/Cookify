using System.Reflection;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Pipelines;
using Cookify.Application.Recipe;
using Cookify.Application.User.Avatar;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cookify.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAsyncDisposingPipeline<UploadCurrentUserAvatarCommand, string>();
        services.AddAsyncDisposingPipeline<CreateRecipeCommand, Guid>();
        services.AddTransactionPipeline<UploadCurrentUserAvatarCommand, string>();
        services.AddTransactionPipeline<CreateRecipeCommand, Guid>();
        services.AddTransactionPipeline<DeleteCurrentUserAvatarCommand, Unit>();
        
        return services;
    }

    #region Pipelines

    public static IServiceCollection AddTransactionPipeline<TCommand, TResponse>(this IServiceCollection services) 
        where TCommand : CommandBase<TResponse>
    {
       
        services.AddScoped(
            serviceType: typeof(IPipelineBehavior<TCommand, TResponse>), 
            implementationType: typeof(TransactionPipeline<TCommand, TResponse>)
        );
        return services;
    }

    public static IServiceCollection AddAsyncDisposingPipeline<TCommand, TResponse>(this IServiceCollection services) 
        where TCommand : CommandBase<TResponse>, IAsyncDisposable
    {
       
        services.AddScoped(
            serviceType: typeof(IPipelineBehavior<TCommand, TResponse>), 
            implementationType: typeof(AsyncDisposingPipeline<TCommand, TResponse>)
        );
        return services;
    }
    
    #endregion
}