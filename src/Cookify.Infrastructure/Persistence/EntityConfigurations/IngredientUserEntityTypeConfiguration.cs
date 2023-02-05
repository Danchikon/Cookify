using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.IngredientUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class IngredientUserStorageEntityTypeConfiguration : IEntityTypeConfiguration<IngredientUserEntity>
{
    public void Configure(EntityTypeBuilder<IngredientUserEntity> builder)
    {
        builder.HasKey(ingredientRecipe => new { ingredientRecipe.UserId, ingredientRecipe.IngredientId });
        
        builder.HasQueryFilter(ingredientRecipe => ingredientRecipe.Ingredient.IsActive && ingredientRecipe.User.IsActive);
        
        #region Properties

        builder
            .Property(ingredientRecipe => ingredientRecipe.UkrainianMeasure)
            .IsRequired()
            .HasMaxLength(150);
        
        #endregion

        #region Relations
        
        builder
            .HasOne(ingredientUser => ingredientUser.User)
            .WithMany(user => user.IngredientUsers)
            .HasForeignKey(ingredientUser => ingredientUser.UserId)
            .IsRequired();

        builder
            .HasOne(ingredientUser => ingredientUser.Ingredient)
            .WithMany(ingredient => ingredient.IngredientUsers)
            .HasForeignKey(ingredientUser => ingredientUser.IngredientId)
            .IsRequired();
        
        #endregion
    }
}