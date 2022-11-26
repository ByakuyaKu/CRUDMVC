using CRUDMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace CRUDMVC.Models
{
    public class Order : IValidatableObject
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }

        [Precision(3)]
        public DateTime Date { get; set; }

        //foreign keys
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public int ProviderId { get; set; }

        public Provider? Provider { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var currContext = validationContext.GetService(typeof(DbContext));

            var _context = (AppDbContext)validationContext
                         .GetService(typeof(AppDbContext));

            if (int.TryParse(Number, out int number) && ProviderId == number)
                yield return new ValidationResult(
                    "Number and ProviderId cant be equal",
                    new[] { nameof(Number), nameof(ProviderId) });

            if (OrderItems.Where(i => i.Name.Equals(Number)).Any())
            {
                yield return new ValidationResult(
                    "Order number cant be equal with order item name",
                    new[] { nameof(Number) });
            }

            var res = _context?.Orders.FirstOrDefault(o => o.Number.Equals(Number) && o.ProviderId == ProviderId);
            if ( res != null && _context?.Entry(res).State == EntityState.Added)
                yield return new ValidationResult(
                        "Pair order number and providerId must be unique",
                        new[] { nameof(Number) });
        }
    }
}
