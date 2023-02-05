using Cookify.Domain.Session;
using Cookify.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookify.Infrastructure.Persistence.EntityConfigurations;

public class SessionEntityTypeConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.HasKey(session => session.Id);
        
        builder.HasQueryFilter(session => session.IsActive);

        #region Properties

        builder
            .Property(session => session.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTimeOffset.UtcNow);
        
        builder
            .Property(session => session.IsActive)
            .IsRequired();
        
        builder
            .Property(session => session.SessionExpirationTime)
            .IsRequired();

        builder
            .Property(session => session.RefreshTokenHash)
            .IsRequired();

        #endregion

        #region Relations

        builder
            .HasOne(session => session.User)
            .WithOne(user => user.Session)
            .HasForeignKey<UserEntity>(user => user.SessionId)
            .OnDelete(DeleteBehavior.SetNull);

        #endregion
    }
}