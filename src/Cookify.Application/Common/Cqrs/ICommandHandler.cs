using MediatR;

namespace Cookify.Application.Common.Cqrs;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : CommandBase
{
    
}

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : CommandBase<TResult>
{
    
}