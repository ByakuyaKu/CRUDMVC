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

        public IEnumerable<Order> SortOrders(string? sortOrder, IEnumerable<Order> OrderList);

        public IQueryable<Order> SortOrders(string? sortOrder, IQueryable<Order> OrderList);

        public Task<Order> GetOrderByIdAsync(int id);

        public Task<Order> GetOrderByIdAsNoTrackingAsync(int id);

        public Task<IEnumerable<Order>> FillOrderListAsync(string? searchString);

        public IQueryable<Order> GetOrderListIQueryableAsNoTracking(string? searchString);

        public IQueryable<Order> GetOrderListIQueryableAsNoTracking(string? searchString, DateTime? dateFrom, DateTime? dateTo);

        public Task<bool> OrderExistsAsync(int id);
    }
}
