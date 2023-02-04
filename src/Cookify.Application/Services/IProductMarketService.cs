using Cookify.Application.Models;

namespace Cookify.Application.Services;

public interface IProductMarketService
{
    Task<MarketProductModel?> GetProductAsync(string productName);
}