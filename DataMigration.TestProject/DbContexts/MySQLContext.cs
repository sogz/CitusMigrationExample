using DataMigration.TestProject.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class MySQLContext : DbContext
{
    public DbSet<Sales> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string mysqlUri = Environment.GetEnvironmentVariable("MYSQL_URI");
            optionsBuilder.UseMySql(mysqlUri, ServerVersion.AutoDetect(mysqlUri));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.SetMaxIdentifierLength(64);
        modelBuilder.ApplyConfiguration(new SalesEntityConfiguration());
        SetPropertyBoolToZeroOneConverter(modelBuilder);
    }

    private void SetPropertyBoolToZeroOneConverter(ModelBuilder modelBuilder)
    {
        BoolToZeroOneConverter<byte> zeroOneConverter = new BoolToZeroOneConverter<byte>();
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.ClrType.GetProperties()
                .Where(x => x.PropertyType == typeof(bool)))
            {
                modelBuilder.Entity(entity.Name)
                    .Property(property.Name)
                    .HasConversion(zeroOneConverter);
            }
        }
    }
}
