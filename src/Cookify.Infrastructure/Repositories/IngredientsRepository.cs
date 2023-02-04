using AutoMapper;
using Cookify.Domain.Ingredient;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class IngredientsRepository : EfRepository<IngredientEntity, CookifyDbContext>, IIngredientsRepository
{
    public IngredientsRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}