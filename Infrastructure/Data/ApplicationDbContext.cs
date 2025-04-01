using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data.Configs;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;


namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Users = Set<UserEntity>();
            Logins = Set<LoginEntity>();
            Items = Set<ItemEntity>();
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LoginEntity> Logins { get; set; }
        public DbSet<ItemEntity> Items { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Rename to snake case
            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName();
                if (tableName != null) entity.SetTableName(mapper.TranslateMemberName(tableName));
                foreach (var property in entity.GetProperties())
                {
                    var columnName = property.GetColumnName();
                    if (columnName != null) property.SetColumnName(mapper.TranslateMemberName(columnName));
                }
            }

            //Apply Configuration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // modelBuilder.ApplyConfiguration(new ItemConfig());
            // var configurations = Assembly.GetExecutingAssembly()
            //     .GetTypes()
            //     .Where(t => t.GetInterfaces()
            //         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
            //         && t != typeof(BaseConfiguration<>)) 
            //     .Select(Activator.CreateInstance)
            //     .Cast<dynamic>();
            // foreach (var config in configurations)
            // {
            //     modelBuilder.ApplyConfiguration(config);
            // }
        }



    }
}