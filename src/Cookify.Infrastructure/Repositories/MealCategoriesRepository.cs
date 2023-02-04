using AutoMapper;
using Cookify.Domain.MealCategory;
using Cookify.Domain.RecipeCategory;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class RecipeCategoriesRepository : EfRepository<RecipeCategoryEntity, CookifyDbContext>, IRecipeCategoriesRepository
{
    public RecipeCategoriesRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}