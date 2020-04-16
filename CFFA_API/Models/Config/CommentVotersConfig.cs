using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class CommentVotersConfig : IEntityTypeConfiguration<CommentVoters>
    {
        public void Configure(EntityTypeBuilder<CommentVoters> modelBuilder)
        {
            modelBuilder
                .HasOne(u => u.Comment)
                .WithMany(u => u.Voters);

            modelBuilder
                .Property(cv => cv.PositiveVot)
                .HasDefaultValue(true);
        }
    }
}
