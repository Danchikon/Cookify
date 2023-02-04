using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Ingredient;

namespace Cookify.Application.Ingredient;

public record GetIngredientQuery(Guid Id) : QueryBase<IngredientDto>;