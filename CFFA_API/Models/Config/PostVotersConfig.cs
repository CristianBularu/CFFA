using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class PostVotersConfig : IEntityTypeConfiguration<PostVoters>
    {
        public void Configure(EntityTypeBuilder<PostVoters> modelBuilder)
        {
            modelBuilder
                .HasOne(u => u.Post)
                .WithMany(u => u.Voters);
            modelBuilder.Property(pv => pv.PositiveVot).HasDefaultValue(true);
        }
    }
}
