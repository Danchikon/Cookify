using Cookify.Domain.Like;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class LikeEntityTypeConfiguration : IEntityTypeConfiguration<LikeEntity>
{
    public void Configure(EntityTypeBuilder<LikeEntity> builder)
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
            .WithMany(recipe => recipe.Likes);

        builder
            .HasOne(like => like.User)
            .WithMany(user => user.Likes);
    }
}