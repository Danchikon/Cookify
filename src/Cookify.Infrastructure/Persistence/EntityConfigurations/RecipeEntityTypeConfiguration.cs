using Cookify.Domain.Recipe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<RecipeEntity>
{
    public void Configure(EntityTypeBuilder<RecipeEntity> builder)
    {
        builder.HasKey(recipe => recipe.Id);
        
        builder.HasQueryFilter(recipe => recipe.IsActive);

        #region Properties

        builder
            .Property(recipe => recipe.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTimeOffset.UtcNow);
        
        builder
            .Property(recipe => recipe.IsActive)
            .IsRequired();

        builder
            .Property(recipe => recipe.Title)
            .IsRequired()
            .HasMaxLength(150);
        
        builder
            .Property(recipe => recipe.UkrainianTitle)
            .IsRequired()
            .HasMaxLength(150);

        builder
            .Property(recipe => recipe.Instruction)
            .IsRequired()
            .HasMaxLength(40000)
            .HasDefaultValue(string.Empty);
        
        builder
            .Property(recipe => recipe.UkrainianInstruction)
            .IsRequired()
            .HasMaxLength(40000)
            .HasDefaultValue(string.Empty);
        
        builder
            .Property(recipe => recipe.ImageLink)
            .HasDefaultValue(null);
        
        builder
            .Property(recipe => recipe.PdfLink)
            .HasDefaultValue(null);
        
        builder
            .Property(recipe => recipe.UkrainianPdfLink)
            .HasDefaultValue(null);
        
        builder
            .Property(recipe => recipe.IsPublic)
            .IsRequired();
        
        #endregion
        
        builder.HasIndex(recipe => recipe.Title);

        #region Relations

        builder
            .HasMany(recipe => recipe.IngredientRecipes)
            .WithOne(ingredientRecipe => ingredientRecipe.Recipe)
            .HasForeignKey(ingredientRecipe => ingredientRecipe.RecipeId)
            .IsRequired();
        
        builder
            .HasMany(recipe => recipe.Likes)
            .WithOne(like => like.Recipe)
            .HasForeignKey(like => like.RecipeId)
            .IsRequired();

        builder
            .HasOne(recipe => recipe.Category)
            .WithMany(mealCategory => mealCategory.Recipes)
            .HasForeignKey(recipe => recipe.CategoryId)
            .IsRequired();
        
        builder
            .HasOne(recipe => recipe.User)
            .WithMany(user => user.Recipes)
            .HasForeignKey(recipe => recipe.CreatedBy);

        #endregion
    }
}