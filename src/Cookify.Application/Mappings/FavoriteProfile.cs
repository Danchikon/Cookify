using AutoMapper;
using Cookify.Application.Dtos.Recipe;
using Cookify.Domain.Favorite;

namespace Cookify.Application.Mappings;

public class FavoriteProfile : Profile
{
    public FavoriteProfile()
    {
        CreateMap<FavoriteEntity, RecipeShortInfoDto>()
            .ForMember(
                destinationMember: dto => dto.Id, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Recipe.Id)
            )
            .ForMember(
                destinationMember: dto => dto.Title, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Recipe.Title)
            )
            .ForMember(
                destinationMember: dto => dto.UkrainianTitle, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Recipe.UkrainianTitle)
            )
            .ForMember(
                destinationMember: dto => dto.ImageLink, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Recipe.ImageLink)
            )
            .ForMember(
                destinationMember: dto => dto.LikesCount, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Recipe.Likes.Count)
            )
            .ForMember(
                destinationMember: dto => dto.Category, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Recipe.Category)
            )
            .ForMember(
                destinationMember: dto => dto.Ingredients, 
                memberOptions: configuration => configuration.MapFrom(entity => entity.Recipe.IngredientRecipes)
            );
    }
}