using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Recipe.Ingredient;

public record AddIngredientToRecipeCommand(Guid IngredientId, Guid RecipeId, string UkrainianMeasure) : CommandBase;