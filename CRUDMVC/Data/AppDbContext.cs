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
        private readonly StreamWriter _logStream = new StreamWriter("log.txt", true);

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderItem>().Property(o => o.Quantity).HasPrecision(18, 3);
            //modelBuilder.Entity<Order>().Property(o => o.Date).HasPrecision(2, 7);

            modelBuilder.Entity<Order>().HasIndex(o => new { o.Number, o.ProviderId }).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Provider> Providers { get; set; }

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();

            await _logStream.DisposeAsync();
        }

        //public static ImmutableList<ValidationResult> SaveChangesWithValidation(this DbContext context)
        //{
        //    var result = ExecuteValidation(context);
        //    if (result.Any())
        //        return result;

        //    context.SaveChanges();

        //    return result;
        //}

        //private static ImmutableList<ValidationResult> ExecuteValidation(this DbContext context)
        //{
        //    var result = new List<ValidationResult>();
        //    foreach (var entry in
        //    context.ChangeTracker.Entries()
        //    .Where(e =>
        //        (e.State == EntityState.Added) ||
        //        (e.State == EntityState.Modified)))
        //    {

        //        var entity = entry.Entity;
        //        var valProvider = new ValidationDbContextServiceProvider(context);
        //        var valContext = new ValidationContext(entity, valProvider, null);
        //        var entityErrors = new List<ValidationResult>();
        //        if (!Validator.TryValidateObject(
        //        entity, valContext, entityErrors, true))
        //        {
        //            result.AddRange(entityErrors);
        //        }
        //    }
        //    return result.ToImmutableList();
        //}
    }
}
