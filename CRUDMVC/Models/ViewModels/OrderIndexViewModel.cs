using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CRUDMVC.Models.ViewModels
{
    public class OrderIndexViewModel
    {
        public PagedList<Order>? Orders { get; set; }
        [Precision(3)]
        public DateTime? DateFrom { get; set; }// = DateTime.Now.AddMonths(-1);
        [Precision(3)]
        public DateTime? DateTo { get; set; }// = DateTime.Now;

        public OrderIndexViewModel()
        {
            var today = DateTime.Now;

            DateTo = today;

            DateFrom = today.AddMonths(-1);
        }
    }
}
