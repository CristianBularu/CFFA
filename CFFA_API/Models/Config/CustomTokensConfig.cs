using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.Config
{
    public class CustomTokensConfig : IEntityTypeConfiguration<CustomTokens>
    {
        public void Configure(EntityTypeBuilder<CustomTokens> builder)
        {
            builder
                .HasOne(t => t.User)
                .WithOne(u => u.CustomTokens)
                .HasForeignKey<ApplicationUser>(u => u.CustomTokensId);
        }
    }
}
