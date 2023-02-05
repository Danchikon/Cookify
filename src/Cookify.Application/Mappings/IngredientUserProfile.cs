using AutoMapper;
using Cookify.Application.Dtos.Ingredient;
using Cookify.Application.Dtos.IngredientRecipe;
using Cookify.Application.Dtos.IngredientUser;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.IngredientUser;

namespace Cookify.Application.Mappings;

public class IngredientUserProfile : Profile
{
   public IngredientUserProfile ()
   {
      CreateMap<IngredientUserEntity, IngredientUserShortInfoDto>()
         .ForMember(
            destinationMember: dto => dto.Name,
            memberOptions: configuration => configuration.MapFrom(entity => entity.Ingredient.Name)
         )
         .ForMember(
            destinationMember: dto => dto.UkrainianName,
            memberOptions: configuration => configuration.MapFrom(entity => entity.Ingredient.UkrainianName)
         )
         .ForMember(
            destinationMember: dto => dto.ImageLink,
            memberOptions: configuration => configuration.MapFrom(entity => entity.Ingredient.ImageLink)
         )
         .ForMember(
            destinationMember: dto => dto.IngredientId,
            memberOptions: configuration => configuration.MapFrom(entity => entity.IngredientId)
         );
   }
}