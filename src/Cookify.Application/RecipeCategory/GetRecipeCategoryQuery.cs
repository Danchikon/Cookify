using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryQuery(Guid Id) : QueryBase<RecipeCategoryDto>;