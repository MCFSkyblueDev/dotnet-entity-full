using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configs
{
    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Email).IsRequired();
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(p => p.Provider).IsRequired().HasMaxLength(50);
            builder.HasOne(p => p.Login).WithOne(p => p.User).HasForeignKey<LoginEntity>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);;
        } 
    }

}