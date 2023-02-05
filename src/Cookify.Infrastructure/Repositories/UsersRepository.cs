using AutoMapper;
using Cookify.Domain.User;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class UsersRepository : EfRepository<UserEntity, CookifyDbContext>, IUsersRepository
{
    public UsersRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}