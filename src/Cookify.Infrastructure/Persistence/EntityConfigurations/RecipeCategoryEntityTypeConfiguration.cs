using Cookify.Domain.MealCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class RecipeCategoryEntityTypeConfiguration : IEntityTypeConfiguration<RecipeCategoryEntity>
{
    public void Configure(EntityTypeBuilder<RecipeCategoryEntity> builder)
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
            .Property(mealCategory => mealCategory.UkrainianName)
            .IsRequired()
            .HasMaxLength(150);
        
        builder
            .Property(mealCategory => mealCategory.Description)
            .HasMaxLength(40000)
            .HasDefaultValue(null);
        
        builder
            .Property(mealCategory => mealCategory.UkrainianDescription)
            .HasMaxLength(40000)
            .HasDefaultValue(null);

        builder
            .Property(mealCategory => mealCategory.ImageLink)
            .HasDefaultValue(null);
        
        #endregion

        builder
            .HasIndex(mealCategory => mealCategory.Name)
            .IsUnique();

        builder
            .HasMany(mealCategory => mealCategory.Recipes)
            .WithOne(recipe => recipe.Category)
            .HasForeignKey(recipe => recipe.CategoryId)
            .IsRequired();
    }
}