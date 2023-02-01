using Cookify.Domain.Favorite;
using Cookify.Domain.Like;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class FavoriteEntityTypeConfiguration : IEntityTypeConfiguration<FavoriteEntity>
{
    public void Configure(EntityTypeBuilder<FavoriteEntity> builder)
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
            .HasOne(like => like.Recipe)
            .WithMany(recipe => recipe.Favorites);

        builder
            .HasOne(like => like.User)
            .WithMany(user => user.Favorites);
    }
}