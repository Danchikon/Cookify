using Cookify.Domain.IngredientRecipe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class IngredientRecipeEntityTypeConfiguration : IEntityTypeConfiguration<IngredientRecipeEntity>
{
    public void Configure(EntityTypeBuilder<IngredientRecipeEntity> builder)
    {
        builder.HasKey(ingredientRecipe => new { ingredientRecipe.RecipeId, ingredientRecipe.IngredientId });
        
        builder.HasQueryFilter(ingredientRecipe => ingredientRecipe.Ingredient.IsActive && ingredientRecipe.Recipe.IsActive);
        
        #region Properties

        builder
            .Property(ingredientRecipe => ingredientRecipe.Measure)
            .IsRequired()
            .HasMaxLength(150);

        builder
            .Property(ingredientRecipe => ingredientRecipe.UkrainianMeasure)
            .IsRequired()
            .HasMaxLength(150);
        
        #endregion

        #region Relations
        
        builder
            .HasOne(ingredientRecipe => ingredientRecipe.Ingredient)
            .WithMany(ingredient => ingredient.IngredientRecipes)
            .HasForeignKey(ingredientRecipe => ingredientRecipe.IngredientId)
            .IsRequired();

        builder
            .HasOne(ingredientRecipe => ingredientRecipe.Recipe)
            .WithMany(ingredient => ingredient.IngredientRecipes)
            .HasForeignKey(ingredientRecipe => ingredientRecipe.RecipeId)
            .IsRequired();
        
        #endregion
    }
}