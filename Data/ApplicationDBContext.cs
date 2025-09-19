using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TheMealDBApp.Models;
using TheMealDBApp.DTOs;
// using TheMealDBApp.Migrations;

namespace TheMealDBApp.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() { }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Categories_Temp> Categories_Temp { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrdersDetail> OrdersDetail { get; set; }
        public DbSet<Users> Users { get; set; }
        // dummy
        public DbSet<Dummy> dummys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dummy
           modelBuilder.Entity<Dummy>(entity =>
            {
                entity.HasKey(e => e.nama_user); //primary key
                entity.Property(e => e.nama_user).ValueGeneratedOnAdd(); //auto increment
            });

            modelBuilder.Entity<Categories_Temp>()
                .HasKey(c => new { c.IdCust, c.IdCategory }); // Composite Key

            modelBuilder.Entity<Orders>()
                .HasKey(o => o.OrderID); // Primary Key

            modelBuilder.Entity<OrdersDetail>()
                .HasKey(od => od.OrderDetailID); // Primary Key

            modelBuilder.Entity<Orders>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderID);

            modelBuilder.Entity<Categories_Temp>()
                .HasOne(c => c.Users)               // tiap Category_Temp punya satu User
                .WithMany(u => u.Categories_Temps)       // tiap User bisa punya banyak Category_Temp
                .HasForeignKey(c => c.IdCust);
        }
    }
}