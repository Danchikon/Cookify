using AutoMapper;
using Cookify.Domain.Favorite;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class FavoritesRepository : EfRepository<FavoriteEntity, CookifyDbContext>, IFavoritesRepository
{
    public FavoritesRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}