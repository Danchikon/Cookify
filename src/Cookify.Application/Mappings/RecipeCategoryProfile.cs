using AutoMapper;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Domain.MealCategory;

namespace Cookify.Application.Mappings;

public class RecipeCategoryProfile : Profile
{
    public RecipeCategoryProfile()
    {
        CreateMap<RecipeCategoryEntity, RecipeCategoryDto>();
        CreateMap<RecipeCategoryEntity, RecipeCategoryShortInfoDto>();
    }
}