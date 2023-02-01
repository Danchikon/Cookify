using Cookify.Domain.Favorite;
using Cookify.Domain.Ingredient;
using Cookify.Domain.Like;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<IngredientEntity>
{
    public void Configure(EntityTypeBuilder<IngredientEntity> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder
            .Property(user => user.CreatedAt)
            .IsRequired();
        
        builder
            .Property(user => user.IsActive)
            .IsRequired();

        builder.HasQueryFilter(user => user.IsActive);
        
        builder
            .HasMany(ingredient => ingredient.Recipes)
            .WithMany(recipe => recipe.Ingredients);
    }
}