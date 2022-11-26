using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CRUDMVC.Models.ViewModels
{
    public class OrderIndexViewModel
    {
        public PagedList<Order>? Orders { get; set; }
        [Precision(3)]
        public DateTime? DateFromFilter { get; set; }
        [Precision(3)]
        public DateTime? DateToFilter { get; set; }

        public string? NumberFilter { get; set; }

        public int? ProviderIdFilter { get; set; }

        public string? ProviderNameFilter { get; set; }

        public string? OrderItemNameFilter { get; set; }

        public string? OrderItemUnitFilter { get; set; }
    }
}
