using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataMigration.TestProject.Models
{
    public class Sales
    {
        // first stable second postgre migration remove two attribute
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SaleId { get; set; }

        public Guid TenantId { get; set; }

        public string ProductName { get; set; }

        public string Title { get; set; }

        public int Quantity { get; set; }

        public DateTime SaleDate { get; set; }
    }

    public class SalesEntityConfiguration : IEntityTypeConfiguration<Sales>
    {
        public void Configure(EntityTypeBuilder<Sales> builder)
        {
        }
    }
}
