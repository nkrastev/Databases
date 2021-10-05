using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {

        }
        public SalesContext(DbContextOptions options)
            : base(options)
        {

        }

        //DB Sets
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }                
        public DbSet<Sale> Sales { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SalesDatabase;Integrated Security=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder
                .Entity<Product>()
                .Property(b => b.Description)
                .HasDefaultValue("No description");
            modelBuilder
                .Entity<Sale>()
                .Property(b => b.Date)
                .HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(modelBuilder);
        }
    }
}
