using AutoMapper;
using Cookify.Application.Dtos.Recipe;
using Cookify.Domain.MealCategory;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Mappings;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<RecipeEntity, RecipeDto>()
            .ForMember(
                destinationMember: dto => dto.LikesCount, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Likes.Count)
                )
            .ForMember(
                destinationMember: dto => dto.Ingredients, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.IngredientRecipes)
                );
        
        CreateMap<RecipeEntity, RecipeShortInfoDto>()
            .ForMember(
                destinationMember:dto => dto.LikesCount, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Likes.Count)
                )
            .ForMember(
                destinationMember: dto => dto.Ingredients, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.IngredientRecipes)
                );
    }
}