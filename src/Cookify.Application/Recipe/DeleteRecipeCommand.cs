using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Recipe;

public record DeleteRecipeCommand(Guid Id) : CommandBase;
