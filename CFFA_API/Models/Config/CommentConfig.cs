using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasOne(p => p.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.PostId)
                .IsRequired();

            builder
                .Property(p => p.BodyText)
                .IsRequired()
                .HasMaxLength(500);

            builder
                .HasOne(p => p.Parent)
                .WithMany(p => p.Replies)
                .HasForeignKey(p => p.ParentId);

            builder
                .Property(u => u.NotSoftDeleted)
                .IsRequired(true)
                .HasDefaultValue(true);

            builder
                .Property(u => u.CreationTime)
                .IsRequired(true)
                .HasDefaultValue(DateTime.UtcNow);
        }
    }
}
