using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class UserSubscriptionsConfig : IEntityTypeConfiguration<UserSubscriptions>
    {
        public void Configure(EntityTypeBuilder<UserSubscriptions> modelBuilder)
        {
            modelBuilder
                .HasOne(u => u.User)
                .WithMany(u => u.Subscriptions);
        }
    }
}
