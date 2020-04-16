using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CFFA_API.Models;
using CFFA_API.Models.Config;

namespace CFFA_API
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasQueryFilter(p => p.NotSoftDeleted??false);
            builder.Entity<Post>().HasQueryFilter(p => p.NotSoftDeleted??false);
            builder.Entity<Comment>().HasQueryFilter(p => p.NotSoftDeleted??false);

            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new TagConfig());
            builder.ApplyConfiguration(new SketchConfig());

            builder.ApplyConfiguration(new UserSubscriptionsConfig());
            builder.ApplyConfiguration(new UserFavoritePostsConfig());
            builder.ApplyConfiguration(new CommentVotersConfig());
            builder.ApplyConfiguration(new PostVotersConfig());
            builder.ApplyConfiguration(new TagPostsConfig());
        }
    }
}
