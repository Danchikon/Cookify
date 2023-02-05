using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cookify.Application.Expressions;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Recipe;
using Cookify.Infrastructure.Common.Extensions;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Infrastructure.Repositories;

public class RecipesRepository : EfRepository<RecipeEntity, CookifyDbContext>, IRecipesRepository
{
    public RecipesRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint page, 
        uint pageSize, 
        uint offset = 0, 
        string? titleEquals = null,
        string? titleContains = null, 
        string? ukrainianTitleEquals = null, 
        string? ukrainianTitleContains = null,
        Guid? categoryIdEquals = null,
        ICollection<Guid>? ingredientsIdsIntersects = null
        )
    {
        var entities = DbContext.Recipes.AsNoTracking();
       
        if (titleEquals is not null)
        {
            entities = entities.Where(RecipeExpressions.TitleEquals(titleEquals));
        }
        
        if (ukrainianTitleEquals is not null)
        {
            entities = entities.Where(RecipeExpressions.UkrainianTitleEquals(ukrainianTitleEquals));
        }
        
        if (titleContains is not null)
        {
            entities = entities.Where(RecipeExpressions.TitleContains(titleContains));
        }
        
        if (ukrainianTitleContains is not null)
        {
            entities = entities.Where(RecipeExpressions.UkrainianTitleContains(ukrainianTitleContains));
        }
        
        if (categoryIdEquals is not null)
        {
            entities = entities.Where(RecipeExpressions.CategoryIdEquals(categoryIdEquals));
        }
        
        if (ingredientsIdsIntersects is not null)
        {
            entities = entities.Where(RecipeExpressions.IngredientsIdsIntersects(ingredientsIdsIntersects));
        }

        entities
            .Include(recipe => recipe.Category)
            .Include(recipe => recipe.Likes)
            .Include(recipe => recipe.IngredientRecipes)
            .ThenInclude(ingredientRecipe => ingredientRecipe.Ingredient);

        return await entities
            .ProjectTo<TModel>(Mapper.ConfigurationProvider)
            .PaginateByPageSizeAsync(page, pageSize, offset);
    }
    
    public async Task<List<RecipeEntity>> WherePdfLinkIsNullAsync()
    {
        var entities = DbContext.Recipes.AsNoTracking()
            .Where(recipe => recipe.PdfLink == null || recipe.UkrainianPdfLink == null);

        entities = entities
            .Include(recipe => recipe.Category)
            .Include(recipe => recipe.Likes)
            .Include(recipe => recipe.IngredientRecipes)
            .ThenInclude(ingredientRecipe => ingredientRecipe.Ingredient);

        return await entities.ToListAsync();
    }
}