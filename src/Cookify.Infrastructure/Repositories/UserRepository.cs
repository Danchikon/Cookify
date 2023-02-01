using AutoMapper;
using Cookify.Domain.User;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class UserRepository : EfRepository<UserEntity, CookifyDbContext>
{
    public UserRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}