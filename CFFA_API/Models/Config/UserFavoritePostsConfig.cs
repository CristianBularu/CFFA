using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class UserFavoritePostsConfig : IEntityTypeConfiguration<UserFavoritePosts>
    {
        public void Configure(EntityTypeBuilder<UserFavoritePosts> modelBuilder)
        {
            modelBuilder
                .HasOne(u => u.User)
                .WithMany(u => u.FavoritePosts);
        }
    }
}
