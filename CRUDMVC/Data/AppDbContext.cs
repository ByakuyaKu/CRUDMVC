using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System;
using CRUDMVC.Models;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using System.Data.Entity.Validation;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Reflection;

namespace CRUDMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasIndex(o => new { o.Number, o.ProviderId }).IsUnique();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Provider> Providers { get; set; }
    }
}
