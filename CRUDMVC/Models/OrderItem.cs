using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDMVC.Models
{
    public class OrderItem// : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Precision(18, 3)]
        public decimal Quantity { get; set; }

        public string Unit { get; set; }

        //foreign keys
        public int? OrderId { get; set; }

        public Order? Order { get; set; }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Name.Equals(Order.Number))
        //    {
        //        yield return new ValidationResult(
        //            "Order item name cant be equal with order name",
        //            new[] { nameof(Name) });
        //    }

        //    //if()
        //}
    }
}
