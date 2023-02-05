using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Pagination;
using Cookify.Application.Dtos.Recipe;
using Cookify.Domain.Common.Pagination;

namespace Cookify.Application.Recipe;

public record GetRandomRecipeShortInfosQuery(Guid? CategoryIdEquals, uint PageSize = PaginationOptions.DefaultPageSize) 
    : QueryBase<IPaginatedList<RecipeShortInfoDto>>;
