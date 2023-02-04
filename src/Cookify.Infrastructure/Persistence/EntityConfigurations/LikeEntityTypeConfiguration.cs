using Cookify.Domain.Like;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class LikeEntityTypeConfiguration : IEntityTypeConfiguration<LikeEntity>
{
    public void Configure(EntityTypeBuilder<LikeEntity> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.HasQueryFilter(user => user.IsActive);

        #region Properties

        builder
            .Property(user => user.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTimeOffset.UtcNow);
        
        builder
            .Property(user => user.IsActive)
            .IsRequired();
        
        #endregion

        #region Relations

        builder
            .HasOne(like => like.Recipe)
            .WithMany(recipe => recipe.Likes)
            .HasForeignKey(like => like.RecipeId)
            .IsRequired();

        builder
            .HasOne(like => like.User)
            .WithMany(user => user.Likes)
            .HasForeignKey(like => like.CreatedBy)
            .IsRequired();
        
        #endregion
    }
}