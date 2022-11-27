using CRUDMVC.Data;
using CRUDMVC.Models;
using CRUDMVC.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace CRUDMVC.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetAllIEnumerable() => await _appDbContext.Orders.ToListAsync();

        public IQueryable<Order> GetAllIQueryable() => _appDbContext.Orders.AsQueryable();

        public IQueryable<Order> GetAllIQueryableAsNoTracking() => _appDbContext.Orders
            .Include(o => o.Provider)
            .Include(o => o.OrderItems)
            .AsNoTracking();

        public async Task<Order> GetOrderByIdAsync(int id) => await _appDbContext.Orders
            .Include(o => o.Provider)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Order> GetOrderByIdAsNoTrackingAsync(int id) => await _appDbContext.Orders
            .Include(o => o.Provider)
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);

        public IQueryable<Order> GetOrderListFilteringByDateAsNoTracking(DateTime dateFrom, DateTime dateTo)
        {
            IQueryable<Order> OrderList;


            OrderList = _appDbContext.Orders
                 .Include(o => o.Provider)
                 .Include(o => o.OrderItems)
                 .Where(o => o.Date >= dateFrom && o.Date <= dateTo)
                 .AsNoTracking();

            return OrderList;
        }

        public IQueryable<Order> GetOrderListFilteringByNumberAsNoTracking(string number)
        {
            IQueryable<Order> OrderList;


            OrderList = _appDbContext.Orders
                 .Include(o => o.Provider)
                 .Include(o => o.OrderItems)
                 .Where(o => o.Number.Equals(number))
                 .AsNoTracking();

            return OrderList;
        }

        public IQueryable<Order> GetOrderListFilteringByProviderIdAsNoTracking(int id)
        {
            IQueryable<Order> OrderList;

            OrderList = _appDbContext.Orders
                 .Include(o => o.Provider)
                 .Include(o => o.OrderItems)
                 .Where(o => o.ProviderId == id)
                 .AsNoTracking();

            return OrderList;
        }

        public IQueryable<Order> GetOrderListFilteringByProviderNameAsNoTracking(string name)
        {
            IQueryable<Order> OrderList;

            OrderList = _appDbContext.Orders
                 .Include(o => o.Provider)
                 .Include(o => o.OrderItems)
                 .Where(o => o.Provider.Name.Equals(name))
                 .AsNoTracking();

            return OrderList;
        }

        public IQueryable<Order> GetOrderListFilteringByOrderItemNameAsNoTracking(string name)
        {
            IQueryable<Order> OrderList;

            OrderList = _appDbContext.Orders
                 .Include(o => o.Provider)
                 .Include(o => o.OrderItems)
                 .Where(o => o.OrderItems.Any(i => i.Name.Equals(name)))
                 .AsNoTracking();

            return OrderList;
        }

        public IQueryable<Order> GetOrderListFilteringByOrderItemUnitAsNoTracking(string unit)
        {
            IQueryable<Order> OrderList;

            OrderList = _appDbContext.Orders
                 .Include(o => o.Provider)
                 .Include(o => o.OrderItems)
                 .Where(o => o.OrderItems.Any(i => i.Unit.Equals(unit)))
                 .AsNoTracking();

            return OrderList;
        }

        public IQueryable<Order> GetOrdersWithFilters(DateTime? dateToFilter, DateTime? dateFromFilter,
            string? numberFilter,
            int? providerIdFilter, string? providerNameFilter,
            string? orderItemNameFilter, string? orderItemUnitFilter)
        {
            IQueryable<Order> OrderList = GetAllIQueryableAsNoTracking();

            if (dateToFilter != null && dateFromFilter != null)
                OrderList = OrderList.Where(o => o.Date >= dateFromFilter && o.Date <= dateToFilter);

            if (numberFilter != null)
                OrderList = OrderList.Where(o => o.Number.Equals(numberFilter));

            if (providerIdFilter != null)
                OrderList = OrderList.Where(o => o.ProviderId == providerIdFilter);

            if (providerNameFilter != null)
                OrderList = OrderList.Where(o => o.Provider != null && o.Provider.Name.Equals(providerNameFilter));

            if (orderItemNameFilter != null)
                OrderList = OrderList.Where(o => o.OrderItems.Any(i => i.Name.Equals(orderItemNameFilter)));

            if (orderItemUnitFilter != null)
                OrderList = OrderList.Where(o => o.OrderItems.Any(i => i.Unit.Equals(orderItemUnitFilter)));

            return OrderList;
        }
    }
}
