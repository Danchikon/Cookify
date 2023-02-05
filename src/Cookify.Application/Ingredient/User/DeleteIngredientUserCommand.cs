using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Ingredient.User;

public record DeleteIngredientUserCommand(Guid IngredientId) : CommandBase;