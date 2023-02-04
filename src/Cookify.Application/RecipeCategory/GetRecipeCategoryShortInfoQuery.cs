using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryShortInfoQuery(Guid Id) : QueryBase<RecipeCategoryShortInfoDto>;