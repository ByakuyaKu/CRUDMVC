using CRUDMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CRUDMVC.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<IEnumerable<Order>> GetAllIEnumerable();
        public IQueryable<Order> GetAllIQueryable();
        public IQueryable<Order> GetAllIQueryableAsNoTracking();
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<Order> GetOrderByIdAsNoTrackingAsync(int id);
        public IQueryable<Order> GetOrderListFilteringByOrderItemUnitAsNoTracking(string unit);
        public IQueryable<Order> GetOrderListFilteringByOrderItemNameAsNoTracking(string name);
        public IQueryable<Order> GetOrderListFilteringByProviderNameAsNoTracking(string name);
        public IQueryable<Order> GetOrderListFilteringByProviderIdAsNoTracking(int id);
        public IQueryable<Order> GetOrderListFilteringByNumberAsNoTracking(string number);
        public IQueryable<Order> GetOrderListFilteringByDateAsNoTracking(DateTime dateFrom, DateTime dateTo);
        public IQueryable<Order> GetOrdersWithFilters(DateTime? dateToFilter, DateTime? dateFromFilter,
            string? numberFilter, int? providerIdFilter, string? providerNameFilter, 
            string? orderItemNameFilter, string? orderItemUnitFilter);
    }
}
