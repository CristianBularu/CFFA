using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFFA_API.Models.Config
{
    public class SketchConfig: IEntityTypeConfiguration<Sketch>
    {
        public void Configure(EntityTypeBuilder<Sketch> builder)
        {
            builder
                .Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(89);

            builder
                .Property(s => s.PageCount)
                .IsRequired()
                .HasMaxLength(4000);

            builder
                .Property(s => s.PageHeight)
                .IsRequired();
        }
    }
}
