using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDMVC.Models
{
    public class OrderItem
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
    }
}
