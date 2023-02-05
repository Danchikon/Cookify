using AutoMapper;
using Cookify.Domain.Favorite;
using Cookify.Domain.Like;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class LikesRepository : EfRepository<LikeEntity, CookifyDbContext>, ILikesRepository
{
    public LikesRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}