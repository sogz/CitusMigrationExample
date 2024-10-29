using DataMigration.TestProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.NameTranslation;
using System;
using System.Linq;

namespace DataMigration.TestProject.DbContexts
{
    public class PostgresContext : DbContext
    {
        public DbSet<Sales> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string postgreUri = Environment.GetEnvironmentVariable("POSTGRESQL_URI");

            optionsBuilder.UseNpgsql(postgreUri);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sales>().HasKey(p => new { p.SaleId, p.TenantId });
            modelBuilder.Model.SetMaxIdentifierLength(63);

            var mapper = new NpgsqlSnakeCaseNameTranslator();

            modelBuilder.ApplyConfiguration(new SalesEntityConfiguration());
            var types = modelBuilder.Model.GetEntityTypes().ToList();

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = mapper.TranslateMemberName(entity.GetTableName());
                entity.SetTableName(tableName);

                foreach (var property in entity.GetProperties())
                {
                    var storeObjectIdentifier = StoreObjectIdentifier.Table(entity.GetTableName(), null);
                    var columnName = mapper.TranslateMemberName(property.GetColumnName(storeObjectIdentifier));
                    property.SetColumnName(columnName);
                }

            }
        }
    }
}
