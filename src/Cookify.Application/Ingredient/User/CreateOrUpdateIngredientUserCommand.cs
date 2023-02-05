using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Ingredient.User;

public record CreateOrUpdateIngredientUserCommand(Guid IngredientId, string UkrainianMeasure) : CommandBase;