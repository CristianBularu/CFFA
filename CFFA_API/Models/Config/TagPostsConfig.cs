using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class TagPostsConfig : IEntityTypeConfiguration<TagPosts>
    {
        public void Configure(EntityTypeBuilder<TagPosts> modelBuilder)
        {
            modelBuilder
                .HasOne(u => u.Post)
                .WithMany(u => u.Tags);
        }
    }
}
