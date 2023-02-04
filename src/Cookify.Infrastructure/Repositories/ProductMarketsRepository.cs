using AutoMapper;
using Cookify.Domain.ProductMarket;
using Cookify.Infrastructure.Common.Repositories;
using Cookify.Infrastructure.Persistence;

namespace Cookify.Infrastructure.Repositories;

public class ProductMarketsRepository : EfRepository<ProductMarketEntity, CookifyDbContext>, IProductMarketsRepository
{
    public ProductMarketsRepository(CookifyDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}