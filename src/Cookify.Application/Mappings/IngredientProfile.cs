using AutoMapper;
using Cookify.Application.Dtos.Ingredient;
using Cookify.Domain.Ingredient;
using Cookify.Domain.MealCategory;

namespace Cookify.Application.Mappings;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<IngredientEntity, IngredientDto>();
        CreateMap<IngredientEntity, IngredientShortInfoDto>();
    }
}