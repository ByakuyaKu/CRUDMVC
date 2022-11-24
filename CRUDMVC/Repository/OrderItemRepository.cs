using CRUDMVC.Data;
using CRUDMVC.Models;
using CRUDMVC.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUDMVC.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<IEnumerable<OrderItem>> FillOrderItemListAsync(string? searchString)
        {
            IEnumerable<OrderItem> OrderItemList;
            if (searchString != null)
            {
                OrderItemList = await _appDbContext.OrderItems
                     .AsNoTracking()
                     .Include(o => o.Order)
                     .Where(o => o.Name.Contains(searchString))
                     .ToListAsync();

                return OrderItemList;
            }

            OrderItemList = await _appDbContext.OrderItems
                .AsNoTracking()
                .Include(o => o.Order)
                .ToListAsync();

            return OrderItemList;
        }

        public async Task<IEnumerable<OrderItem>> GetAllIEnumerable() => await _appDbContext.OrderItems.ToListAsync();

        public IQueryable<OrderItem> GetAllIQueryable() => _appDbContext.OrderItems.AsQueryable();

        public IQueryable<OrderItem> GetAllIQueryableAsNoTracking() => _appDbContext.OrderItems.AsNoTracking().AsQueryable();

        public async Task<OrderItem> GetOrderItemByIdAsNoTrackingAsync(int id) => await _appDbContext.OrderItems
            .AsNoTracking()
            .Include(o => o.Order)
            .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<OrderItem> GetOrderItemByIdAsync(int id) => await _appDbContext.OrderItems
            .Include(o => o.Order)
            .FirstOrDefaultAsync(o => o.Id == id);

        public IQueryable<OrderItem> GetOrderItemListIQueryableAsNoTracking(string? searchString)
        {
            IQueryable<OrderItem> OrderItemList;
            if (searchString != null)
            {
                OrderItemList = _appDbContext.OrderItems
                     .AsNoTracking()
                     .Include(o => o.Order)
                     .Where(o => o.Name.Contains(searchString));

                return OrderItemList;
            }

            OrderItemList = _appDbContext.OrderItems
                .AsNoTracking()
                .Include(o => o.Order);

            return OrderItemList;
        }

        public async Task<bool> OrderItemExistsAsync(int id) => await _appDbContext.OrderItems.AnyAsync(e => e.Id == id);

        public IEnumerable<OrderItem> SortOrderItems(string? sortOrder, IEnumerable<OrderItem> OrderList)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OrderItem> SortOrderItems(string? sortOrder, IQueryable<OrderItem> OrderList)
        {
            throw new NotImplementedException();
        }
    }
}
