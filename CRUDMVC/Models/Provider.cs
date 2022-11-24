using System.ComponentModel.DataAnnotations;

namespace CRUDMVC.Models
{
    public class Provider
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }

        //foreign keys
        public List<Order>? Orders { get; set; } = new List<Order>();
    }
}
