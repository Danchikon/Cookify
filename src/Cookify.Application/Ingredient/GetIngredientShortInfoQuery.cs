using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Ingredient;

namespace Cookify.Application.Ingredient;

public record GetIngredientShortInfoQuery(Guid Id) : QueryBase<IngredientShortInfoDto>;