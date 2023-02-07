using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Common.Repositories;

namespace Cookify.Domain.Recipe;

public interface IRecipesRepository : IRepository<RecipeEntity>
{
    Task<List<RecipeEntity>> WherePdfLinkIsNullAsync(CancellationToken cancellationToken);
}