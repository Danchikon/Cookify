using Cookify.Domain.Ingredient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<IngredientEntity>
{
    public void Configure(EntityTypeBuilder<IngredientEntity> builder)
    {
        builder.HasKey(ingredient => ingredient.Id);
        
        builder.HasQueryFilter(ingredient => ingredient.IsActive);

        #region Properties

        builder
            .Property(ingredient => ingredient.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTimeOffset.UtcNow);
        
        builder
            .Property(ingredient => ingredient.IsActive)
            .IsRequired();
        
        builder
            .Property(ingredient => ingredient.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder
            .Property(ingredient => ingredient.UkrainianName)
            .IsRequired()
            .HasMaxLength(150);
        
        builder
            .Property(ingredient => ingredient.Description)
            .HasMaxLength(4000)
            .HasDefaultValue(null);
        
        builder
            .Property(ingredient => ingredient.ImageLink)
            .HasDefaultValue(null);
        
        #endregion

        builder
            .HasIndex(ingredient => ingredient.Name)
            .IsUnique();

        builder
            .HasMany(ingredient => ingredient.IngredientRecipes)
            .WithOne(ingredientRecipe => ingredientRecipe.Ingredient)
            .HasForeignKey(ingredientRecipe => ingredientRecipe.IngredientId)
            .IsRequired();
    }
}