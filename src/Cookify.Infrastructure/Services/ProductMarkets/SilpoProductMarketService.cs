using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Infrastructure.Dtos.Silpo;
using Cookify.Infrastructure.Options;
using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Options;

namespace Cookify.Infrastructure.Services.ProductMarkets;

public class SilpoProductMarketService : IProductMarketService
{ 
    private readonly IGraphQLClient _graphQlClient;
    private readonly SilpoProductMarketOptions _options;

    public SilpoProductMarketService(IGraphQLClient graphQlClient, IOptions<SilpoProductMarketOptions> options)
    { 
        _graphQlClient = graphQlClient;
        _options = options.Value;
    }
    
    public async Task<MarketProductModel?> GetProductAsync(string productName, CancellationToken cancellationToken)
    {
        var productRequest = new GraphQLRequest { 
            Query = @"{
                search(category: ACTION, pagingInfo: {offset: 0, limit: 0}, query: ""$productName"") {
                  paging {           
                    count
                  }
                }
            }",
            OperationName = "ACTION",
            Variables = new { productName }
        };

        var response = await _graphQlClient.SendQueryAsync<ResponseData>(productRequest, cancellationToken);

        if (response.Data.Search?.Paging?.Count is 0 or null)
        {
            return null;
        }

        var productLink = $"{_options.SiteUrl}/search?category=all&search={productName}";

        return new MarketProductModel(productName, productLink, response.Data.Search.Paging.Count);
    }
  
}