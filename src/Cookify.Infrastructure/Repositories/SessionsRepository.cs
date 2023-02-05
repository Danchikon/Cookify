using AutoMapper;
using Cookify.Domain.Session;
using Cookify.Domain.User;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class SessionsRepository : EfRepository<SessionEntity, CookifyDbContext>, ISessionsRepository
{
    public SessionsRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}