using AutoMapper;
using Cookify.Application.Dtos.Recipe;
using Cookify.Domain.Favorite;
using Cookify.Domain.Like;

namespace Cookify.Application.Mappings;

public class LikeProfile : Profile
{
    public LikeProfile()
    {
        CreateMap<LikeEntity, RecipeShortInfoDto>()
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