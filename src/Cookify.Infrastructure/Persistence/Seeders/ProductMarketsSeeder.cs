using Cookify.Application.Expressions;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.ProductMarket;
using Cookify.Infrastructure.Common.Seeders;
using Cookify.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Persistence.Seeders;

public class ProductMarketsSeeder : SeederBase
{
    private readonly SilpoProductMarketOptions _silpoProductMarketOptions;
    private readonly IProductMarketsRepository _productMarketsRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ProductMarketsSeeder(
        IOptions<SilpoProductMarketOptions> silpoProductMarketOptions,
        IProductMarketsRepository productMarketRepository, 
        IUnitOfWork unitOfWork
        )
    {
        _silpoProductMarketOptions = silpoProductMarketOptions.Value;
        _productMarketsRepository = productMarketRepository;
        _unitOfWork = unitOfWork;
    }
    
    public override async Task SeedAsync()
    {
        await SeedProductMarketAsync(new ProductMarketEntity("Silpo", "Сільпо", _silpoProductMarketOptions.SiteUrl)
        {
            ImageLink = _silpoProductMarketOptions.ImageUrl
        });
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task SeedProductMarketAsync(ProductMarketEntity productMarket)
    {
        if (!await _productMarketsRepository.AnyAsync(ProductMarketExpressions.NameEquals(productMarket.Name)))
        {
            await _productMarketsRepository.AddAsync(productMarket);
        }
    }
}