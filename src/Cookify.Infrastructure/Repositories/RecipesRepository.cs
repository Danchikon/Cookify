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

    public async Task<List<RecipeEntity>> WherePdfLinkIsNullAsync(CancellationToken cancellationToken)
    {
        var entities = DbContext.Recipes.AsNoTracking()
            .Where(recipe => recipe.PdfLink == null || recipe.UkrainianPdfLink == null);

        entities = entities
            .Include(recipe => recipe.Category)
            .Include(recipe => recipe.Likes)
            .Include(recipe => recipe.IngredientRecipes)
            .ThenInclude(ingredientRecipe => ingredientRecipe.Ingredient);

        return await entities.ToListAsync(cancellationToken);
    }
}