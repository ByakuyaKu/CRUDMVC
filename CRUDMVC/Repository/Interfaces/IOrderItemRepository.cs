using CRUDMVC.Models;

namespace CRUDMVC.Repository.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public Task<IEnumerable<OrderItem>> GetAllIEnumerable();

        public IQueryable<OrderItem> GetAllIQueryable();

        public IQueryable<OrderItem> GetAllIQueryableAsNoTracking();

        public IEnumerable<OrderItem> SortOrderItems(string? sortOrder, IEnumerable<OrderItem> OrderList);

        public IQueryable<OrderItem> SortOrderItems(string? sortOrder, IQueryable<OrderItem> OrderList);

        public Task<OrderItem> GetOrderItemByIdAsync(int id);

        public Task<OrderItem> GetOrderItemByIdAsNoTrackingAsync(int id);

        public Task<IEnumerable<OrderItem>> FillOrderItemListAsync(string? searchString);

        public IQueryable<OrderItem> GetOrderItemListIQueryableAsNoTracking(string? searchString);

        public Task<bool> OrderItemExistsAsync(int id);
    }
}
