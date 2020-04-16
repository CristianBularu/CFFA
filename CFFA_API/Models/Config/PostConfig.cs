using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(89);

            builder
                .Property(p => p.BodyText)
                .IsRequired(false)
                .HasMaxLength(4000);

            builder
                .Property(u => u.NotSoftDeleted)
                .IsRequired(true)
                .HasDefaultValue(true);

            builder
                .Property(u => u.CreationTime)
                .IsRequired(true)
                .HasDefaultValue(DateTime.UtcNow);

            builder
                .HasOne(p => p.Sketch)
                .WithMany(s => s.Posts)
                .HasForeignKey(u => u.SketchId);
        }
    }
}
