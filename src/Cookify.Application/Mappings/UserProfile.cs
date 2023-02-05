using AutoMapper;
using Cookify.Application.Dtos.User;
using Cookify.Domain.User;

namespace Cookify.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDto>()
            .ForMember(
                destinationMember: dto => dto.FavoriteRecipes,
                memberOptions: configuration => configuration.MapFrom(entity => entity.Favorites)
            )
            .ForMember(
                destinationMember: dto => dto.LikedRecipes,
                memberOptions: configuration => configuration.MapFrom(entity => entity.Likes)
            )
            .ForMember(
                destinationMember: dto => dto.AvailableIngredients,
                memberOptions: configuration => configuration.MapFrom(entity => entity.IngredientUsers)
            );
        
        CreateMap<UserEntity, UserShortInfoDto>();
    }   
}