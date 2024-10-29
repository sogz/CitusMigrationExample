using Bogus;
using DataMigration.TestProject.DbContexts;
using DataMigration.TestProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataMigration.TestProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InsertSalesDataToMySQL();

            using (PostgresContext context = new PostgresContext())
            {
                context.Database.Migrate();
            }

            using (MySQLContext mysqlContext = new MySQLContext())
            {
                var saleList = mysqlContext.Sales.ToList();
                CreatePostgresSqlFile(saleList);
            }
        }

        static void InsertSalesDataToMySQL()
        {
            using (MySQLContext context = new MySQLContext())
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                foreach (var salesBatch in GenerateSalesDataInBatches(10_000_000, 1000, 1000))
                {
                    context.Sales.AddRange(salesBatch);
                    context.SaveChanges();
                }
            }
        }

        static IEnumerable<IEnumerable<Sales>> GenerateSalesDataInBatches(int totalRows, int tenantSwitchInterval, int batchSize)
        {
            var currentTenant = Guid.NewGuid();
            var faker = new Faker<Sales>()
                .RuleFor(s => s.SaleId, f => Guid.NewGuid())
                .RuleFor(s => s.TenantId, f => currentTenant)
                .RuleFor(s => s.ProductName, f => f.Commerce.ProductName())
                .RuleFor(s => s.Title, f => f.Commerce.ProductMaterial())
                .RuleFor(s => s.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(s => s.SaleDate, f => f.Date.Past(1));

            var batch = new List<Sales>(batchSize);

            for (int i = 1; i <= totalRows; i++)
            {
                if (i % tenantSwitchInterval == 0)
                {
                    currentTenant = Guid.NewGuid();
                }

                var sale = faker.Generate();
                sale.TenantId = currentTenant;
                batch.Add(sale);

                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<Sales>(batchSize);
                }
            }

            if (batch.Count > 0)
            {
                yield return batch;
            }
        }

        static void CreatePostgresSqlFile(IEnumerable<Sales> salesData)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- PostgreSQL Sales Insert Test");

            foreach (var sale in salesData)
            {
                string insertStatement = $@"
                    INSERT INTO ""sales"" (""sale_id"", ""tenant_id"", ""product_name"", ""title"", ""quantity"", ""sale_date"") 
                    VALUES (
                            '{sale.SaleId}', 
                            '{sale.TenantId}', 
                            '{sale.ProductName.Replace("'", "''")}', 
                            '{sale.Title.Replace("'", "''")}', 
                            {sale.Quantity}, 
                            '{sale.SaleDate:yyyy-MM-dd HH:mm:ss}'
                    );";

                sb.AppendLine(insertStatement);
            }

            File.WriteAllText("postgres_sales_insert.sql", sb.ToString());
        }
    }
}
