using Cookify.Domain.MealCategory;
using Cookify.Domain.ProductMarket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class ProductMarketEntityTypeConfiguration : IEntityTypeConfiguration<ProductMarketEntity>
{
    public void Configure(EntityTypeBuilder<ProductMarketEntity> builder)
    {
        builder.HasKey(mealCategory => mealCategory.Id);
        
        builder.HasQueryFilter(mealCategory => mealCategory.IsActive);

        #region Properties

        builder
            .Property(mealCategory => mealCategory.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTimeOffset.UtcNow);
        
        builder
            .Property(mealCategory => mealCategory.IsActive)
            .IsRequired();
        
        builder
            .Property(mealCategory => mealCategory.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder
            .Property(mealCategory => mealCategory.SiteLink)
            .IsRequired();

        builder
            .Property(mealCategory => mealCategory.ImageLink)
            .IsRequired();
        
        #endregion

        builder
            .HasIndex(mealCategory => mealCategory.Name)
            .IsUnique();
    }
}