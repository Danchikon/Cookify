using Cookify.Application.Common.Cqrs;

namespace Cookify.Application.Recipe.Favorite;

public record CreateFavoriteRecipeCommand(Guid RecipeId) : CommandBase;