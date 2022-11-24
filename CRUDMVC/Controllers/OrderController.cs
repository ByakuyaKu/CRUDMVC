using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDMVC.Data;
using CRUDMVC.Models;
using CRUDMVC.Repository.Interfaces;
using X.PagedList;
using System.Xml.Linq;
using CRUDMVC.Models.ViewModels;

namespace CRUDMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IProviderRepository _providerRepository;

        private readonly IOrderItemRepository _orderItemRepository;

        public OrderController(IOrderRepository orderRepository, IProviderRepository providerRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _providerRepository = providerRepository;
            _orderItemRepository = orderItemRepository;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(string? sortOrder, string? searchString, string currentFilter, int? page,
        //     OrderIndexViewModel orderIndexViewModel)
        //{
        //    ViewData["CurrentSort"] = sortOrder;
        //    ViewData["CurrentSortView"] = sortOrder?.Replace("_", " ").ToLower();

        //    ViewData["CurrentDateTo"] = orderIndexViewModel.DateTo;
        //    ViewData["CurrentDateFrom"] = orderIndexViewModel.DateFrom;

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewData["CurrentFilter"] = searchString;

        //    int pageSize = 1;
        //    int pageNumber = (page ?? 1);

        //    var orders = _orderRepository.GetOrderListIQueryableAsNoTracking(searchString, orderIndexViewModel.DateFrom, orderIndexViewModel.DateTo);

        //    orderIndexViewModel.Orders = (PagedList<Order>?)orders.ToList().ToPagedList(pageNumber, pageSize);

        //    return View(orderIndexViewModel);
        //}

        // GET: Order
        public async Task<IActionResult> Index(string? sortOrder, string? searchString, string currentFilter, int? page,
            string? CurrentDateTo, string? CurrentDateFrom)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentSortView"] = sortOrder?.Replace("_", " ").ToLower();

            ViewData["CurrentDateTo"] = CurrentDateTo;
            ViewData["CurrentDateFrom"] = CurrentDateFrom;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            int pageSize = 1;
            int pageNumber = (page ?? 1);

            var model = new OrderIndexViewModel();

            DateTime from, to;
            IQueryable<Order> orders;

            if (CurrentDateFrom != null && CurrentDateTo != null)
                if (DateTime.TryParse(CurrentDateFrom, out from) && DateTime.TryParse(CurrentDateTo, out to))
                {
                    orders = _orderRepository.GetOrderListIQueryableAsNoTracking(searchString, from, to);

                    model.Orders = (PagedList<Order>?)orders.ToList().ToPagedList(pageNumber, pageSize);

                    return View(model);
                }

            orders = _orderRepository.GetOrderListIQueryableAsNoTracking(searchString);

            model.Orders = (PagedList<Order>?)orders.ToList().ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _orderRepository.GetOrderByIdAsNoTrackingAsync((int)id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            //Order order = new Order();
            //order.orderItems.Add(new OrderItem());

            ViewBag.Providers = await _providerRepository.FillProviderViewBagAsync();

            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Date,ProviderId,orderItems")] Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providers = await _providerRepository.FillProviderViewBagAsync();
                return View(order);
            }



            _orderRepository.Add(order);
            await _orderRepository.SaveChangesAsync();
            //_orderRepository.SaveChangesWithValidation();

            TempData["success"] = "Order created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _orderRepository.GetOrderByIdAsync((int)id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Date,ProviderId")] Order order)
        {
            if (order == null || id != order.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(order);

            try
            {
                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();

                TempData["success"] = "Order updated successfully!";
            }
            //catch
            //{
            //    if (!await _orderRepository.OrderExistsAsync(order.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }
            //}
            catch
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _orderRepository.GetOrderByIdAsNoTrackingAsync((int)id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order != null)
            {
                if (order.orderItems != null)
                {
                    _orderItemRepository.RemoveRange(order.orderItems);
                    await _orderRepository.SaveChangesAsync();
                }

                _orderRepository.Remove(order);
                await _orderRepository.SaveChangesAsync();
            }
            

            TempData["success"] = "Order deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

    }
}
