using Cookify.Infrastructure.Dtos.TheMealDb;
using Refit;

namespace Cookify.Infrastructure.Services.RestApis;

public interface ITheMealDbApi
{
    [Get("/api/json/v1/1/lookup.php?i={id}")]
    Task<RecipesListDto> GetRecipeAsync(long id);

    [Get("/api/json/v1/1/search.php?f={letter}")]
    Task<RecipesListDto> GetRecipesByFirstLetterAsync(string letter);
    
    [Get("/api/json/v1/1/categories.php")]
    Task<RecipeCategoriesListDto> GetRecipeCategoriesAsync();

    [Get("/api/json/v1/1/list.php?i=list")]
    Task<IngredientsListDto> GetIngredientsAsync();
}