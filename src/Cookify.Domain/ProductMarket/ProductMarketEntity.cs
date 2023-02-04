using Cookify.Domain.Common.Entities;

namespace Cookify.Domain.ProductMarket;

public class ProductMarketEntity : IEntity<Guid>
{
    public Guid Id { get; init; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public string Name { get; set; } = null!;
    public string UkrainianName { get; set; } = null!;
    public string ImageLink { get; set; } = null!;
    public string SiteLink { get; set; } = null!;

    public ProductMarketEntity()
    {
        
    }

    public ProductMarketEntity(string name, string ukrainianName, string siteLink)
    {
        Name = name;
        UkrainianName = ukrainianName;
        SiteLink = siteLink;
    }
}