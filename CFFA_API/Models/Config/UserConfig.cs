using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class UserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(55);

            builder
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder
                .HasMany(u => u.Comments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder
                .Property(u => u.NotSoftDeleted)
                .IsRequired(true)
                .HasDefaultValue(true);

            builder
                .Property(u => u.CreationTime)
                .IsRequired(true)
                .HasDefaultValue(DateTime.UtcNow);

            builder
                .HasOne(u => u.CustomTokens)
                .WithOne(t => t.User)
                .HasForeignKey<CustomTokens>(t => t.UserId);
        }
    }
}
