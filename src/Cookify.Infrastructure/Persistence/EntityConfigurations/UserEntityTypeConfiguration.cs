using Cookify.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.HasQueryFilter(user => user.IsActive);

        #region Properties

        builder
            .Property(user => user.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTimeOffset.UtcNow);;;
        
        builder
            .Property(user => user.IsActive)
            .IsRequired();

        builder
            .Property(user => user.Username)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .Property(user => user.PasswordHash)
            .IsRequired();
        
        builder
            .Property(user => user.AvatarImageLink)
            .HasDefaultValue(null);
        
        #endregion

        #region Indexes

        builder
            .HasIndex(user => user.Username)
            .IsUnique();
        
        builder
            .HasIndex(user => user.Email)
            .IsUnique();
        
        #endregion

        #region Relations

        builder
            .HasMany(user => user.Likes)
            .WithOne(like => like.User)
            .HasForeignKey(like => like.CreatedBy)
            .IsRequired();
        
        builder
            .HasMany(user => user.Favorites)
            .WithOne(favorite => favorite.User)
            .HasForeignKey(favorite => favorite.CreatedBy)
            .IsRequired();

        builder
            .HasMany(user => user.Recipes)
            .WithOne(recipe => recipe.User)
            .HasForeignKey(recipe => recipe.CreatedBy);
        
        #endregion
    }
}