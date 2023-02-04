using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;

namespace Cookify.Application.Recipe;

public record GetRecipeQuery(Guid Id) : QueryBase<RecipeDto>;