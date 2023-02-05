using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Common.Repositories;

namespace Cookify.Domain.Recipe;

public interface IRecipesRepository : IRepository<RecipeEntity>
{
    Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint page,
        uint pageSize,
        uint offset = 0,
        string? titleEquals = null,
        string? titleContains = null,
        string? ukrainianTitleEquals = null,
        string? ukrainianTitleContains = null,
        Guid? categoryIdEquals = null,
        ICollection<Guid>? ingredientsIdsIntersects = null
    );

    Task<List<RecipeEntity>> WherePdfLinkIsNullAsync();
}