using CRUDMVC.Data;
using CRUDMVC.Models;
using CRUDMVC.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;

namespace CRUDMVC.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetAllIEnumerable() => await _appDbContext.Orders.ToListAsync();

        public IQueryable<Order> GetAllIQueryable() => _appDbContext.Orders.AsQueryable();

        public IQueryable<Order> GetAllIQueryableAsNoTracking() => _appDbContext.Orders.AsNoTracking().AsQueryable();

        public async Task<Order> GetOrderByIdAsync(int id) => await _appDbContext.Orders
            .Include(o => o.Provider)
            .Include(o=> o.orderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Order> GetOrderByIdAsNoTrackingAsync(int id) => await _appDbContext.Orders
            .AsNoTracking()
            .Include(o => o.Provider)
            .Include(o => o.orderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        public IEnumerable<Order> SortOrders(string? sortOrder, IEnumerable<Order> OrderList)
        {
            switch (sortOrder)
            {
                case "Date_desc":
                    OrderList = OrderList.OrderByDescending(o => o.Date);
                    break;

                case "Date_asc":
                    OrderList = OrderList.OrderBy(o => o.Date);
                    break;

                case "ProviderId_asc":
                    OrderList = OrderList.OrderBy(o => o.ProviderId);
                    break;

                case "ProviderId_desc":
                    OrderList = OrderList.OrderByDescending(o => o.ProviderId);
                    break;
            }
            return OrderList;
        }

        public async Task<bool> OrderExistsAsync(int id) => await _appDbContext.Orders.AnyAsync(e => e.Id == id);

        public IQueryable<Order> SortOrders(string? sortOrder, IQueryable<Order> OrderList)
        {
            switch (sortOrder)
            {
                case "Date_desc":
                    OrderList = OrderList.OrderByDescending(o => o.Date);
                    break;

                case "Date_asc":
                    OrderList = OrderList.OrderBy(o => o.Date);
                    break;

                case "ProviderId_asc":
                    OrderList = OrderList.OrderBy(o => o.ProviderId);
                    break;

                case "ProviderId_desc":
                    OrderList = OrderList.OrderByDescending(o => o.ProviderId);
                    break;
            }
            return OrderList;
        }

        public async Task<IEnumerable<Order>> FillOrderListAsync(string? searchString)
        {
            IEnumerable<Order> OrderList;
            if (searchString != null)
            {
                OrderList = await _appDbContext.Orders
                     .AsNoTracking()
                     .Include(o => o.Provider)
                     .Where(o => o.Number.Contains(searchString))
                     .ToListAsync();

                return OrderList;
            }

            OrderList = await _appDbContext.Orders
                .AsNoTracking()
                .Include(o => o.Provider)
                .ToListAsync();

            return OrderList;
        }

        public IQueryable<Order> GetOrderListIQueryableAsNoTracking(string? searchString, DateTime? dateFrom, DateTime? dateTo)
        {
            IQueryable<Order> OrderList;
            if (searchString != null && (dateTo == null || dateFrom == null))
            {
                OrderList = _appDbContext.Orders
                     .AsNoTracking()
                     .Include(o => o.Provider)
                     .Where(o => o.Number.Contains(searchString));

                return OrderList;
            }

            if(searchString != null && dateTo != null && dateFrom != null)
            {
                OrderList = _appDbContext.Orders
                     .AsNoTracking()
                     .Include(o => o.Provider)
                     .Where(o => o.Number.Contains(searchString) && o.Date >= dateFrom && o.Date <= dateTo);

                return OrderList;
            }

            if(searchString == null && dateTo != null && dateFrom != null)
            {
                OrderList = _appDbContext.Orders
                     .AsNoTracking()
                     .Include(o => o.Provider)
                     .Where(o => o.Date >= dateFrom && o.Date <= dateTo);

                return OrderList;
            }
                
            OrderList = _appDbContext.Orders
                .AsNoTracking()
                .Include(o => o.Provider);

            return OrderList;
        }

        public IQueryable<Order> GetOrderListIQueryableAsNoTracking(string? searchString)
        {
            IQueryable<Order> OrderList;
            if (searchString != null)
            {
                OrderList = _appDbContext.Orders
                     .AsNoTracking()
                     .Include(o => o.Provider)
                     .Where(o => o.Number.Contains(searchString));

                return OrderList;
            }

            OrderList = _appDbContext.Orders
                .AsNoTracking()
                .Include(o => o.Provider);

            return OrderList;
        }

        //public IQueryable<Order> GetOrderListFilteringByDateAsNoTracking(DateTime dateFrom, DateTime dateTo)
        //{
        //    IQueryable<Order> OrderList;


        //    OrderList = _appDbContext.Orders
        //         .AsNoTracking()
        //         .Include(o => o.Provider)
        //         .Where(o => o.Date >= dateFrom && o.Date <= dateTo);

        //    return OrderList;



        //    return OrderList;
        //}
    }
}
