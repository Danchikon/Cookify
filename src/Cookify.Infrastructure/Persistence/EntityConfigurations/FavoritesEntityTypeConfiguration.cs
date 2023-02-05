using Cookify.Domain.Favorite;
using Cookify.Domain.Like;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class FavoriteEntityTypeConfiguration : IEntityTypeConfiguration<FavoriteEntity>
{
    public void Configure(EntityTypeBuilder<FavoriteEntity> builder)
    {
        builder.HasKey(favorite => favorite.Id);
        
        builder.HasQueryFilter(favorite => favorite.IsActive);

        #region Properties

        builder
            .Property(favorite => favorite.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTimeOffset.UtcNow);;
        
        builder
            .Property(favorite => favorite.IsActive)
            .IsRequired();
        
        #endregion

        #region Relations

        builder
            .HasOne(favorite => favorite.Recipe)
            .WithMany(recipe => recipe.Favorites)
            .HasForeignKey(favorite => favorite.RecipeId);

        builder
            .HasOne(favorite => favorite.User)
            .WithMany(user => user.Favorites)
            .HasForeignKey(favorite => favorite.RecipeId);
        
        #endregion

        builder
            .HasIndex(favorite => new { favorite.RecipeId, favorite.CreatedBy })
            .IsUnique();
    }
}