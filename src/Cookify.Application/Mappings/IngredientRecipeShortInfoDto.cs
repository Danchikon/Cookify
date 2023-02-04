using AutoMapper;
using Cookify.Application.Dtos.IngredientRecipe;
using Cookify.Domain.IngredientRecipe;

namespace Cookify.Application.Mappings;

public class IngredientRecipeProfile : Profile
{
   public IngredientRecipeProfile ()
   {
      CreateMap<IngredientRecipeEntity, IngredientRecipeShortInfoDto>()
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