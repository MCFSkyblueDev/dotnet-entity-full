using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configs
{
    public class ItemConfiguration : BaseConfiguration<ItemEntity>
    {
        public override void  Configure(EntityTypeBuilder<ItemEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(e => e.Price).IsRequired().HasColumnType("NUMERIC(18,2)");
            builder.Property(e => e.Quantity).HasDefaultValue(1);
            
        }
    }
}