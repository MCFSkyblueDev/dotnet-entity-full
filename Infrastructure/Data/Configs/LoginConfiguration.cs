using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configs
{
    public class LoginConfiguration : BaseConfiguration<LoginEntity>
    {
        public override void Configure(EntityTypeBuilder<LoginEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Email).IsRequired();
            builder.HasAlternateKey(e => e.Email);
            builder.Property(e => e.Password).IsRequired();
            builder.Property(e => e.UserId).IsRequired();
            builder.HasOne(e => e.User).WithOne(e => e.Login).HasForeignKey<LoginEntity>(e => e.UserId).OnDelete(DeleteBehavior.Cascade);;
        }
    }
}