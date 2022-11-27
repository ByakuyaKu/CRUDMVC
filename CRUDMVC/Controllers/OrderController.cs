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
using static NuGet.Packaging.PackagingConstants;
using System.Drawing.Printing;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        private bool TryGetFilterDates(string? currentDateToFilter, string? currentDateFromFilter, OrderIndexViewModel orderIndexViewModel)
        {

            if (currentDateToFilter != null && currentDateFromFilter != null)
            {
                DateTime to, from;

                if (DateTime.TryParse(currentDateFromFilter, out from) && DateTime.TryParse(currentDateToFilter, out to))
                {
                    orderIndexViewModel.DateToFilter = to;
                    orderIndexViewModel.DateFromFilter = from;

                    return true;
                }
            }

            if (orderIndexViewModel.DateToFilter == null && orderIndexViewModel.DateFromFilter == null
                && currentDateToFilter == null && currentDateFromFilter == null)
            {
                var today = DateTime.Now;

                orderIndexViewModel.DateToFilter = today;

                orderIndexViewModel.DateFromFilter = today.AddMonths(-1);
            }

            return false;
        }

        public IActionResult IndexFilterDate(int? page, string? currentNumberFilter,
            int? currentProviderIdFilter, string? currentProviderNameFilter,
            string? currentOrderItemNameFilter, string? currentOrderItemUnitFilter,
            OrderIndexViewModel orderIndexViewModel)
        {
            ViewData["CurrentDateTo"] = orderIndexViewModel.DateToFilter;
            ViewData["CurrentDateFrom"] = orderIndexViewModel.DateFromFilter;

            ViewData["CurrentProviderIdFilter"] = currentProviderIdFilter;
            ViewData["CurrentProviderNameFilter"] = currentProviderNameFilter;
            ViewData["CurrentOrderItemNameFilter"] = currentOrderItemNameFilter;
            ViewData["CurrentOrderItemUnitFilter"] = currentOrderItemUnitFilter;
            ViewData["CurrentNumberFilter"] = currentNumberFilter;

            if (orderIndexViewModel.DateToFilter == null || orderIndexViewModel.DateFromFilter == null)
                return NotFound();

            return RedirectToAction(nameof(Index), new
            {
                currentDateToFilter = orderIndexViewModel.DateToFilter.ToString(),
                currentDateFromFilter = orderIndexViewModel.DateFromFilter.ToString(),
                currentNumberFilter = currentNumberFilter,
                currentProviderIdFilter = currentProviderIdFilter,
                currentOrderItemNameFilter = currentOrderItemNameFilter,
                currentOrderItemUnitFilter = currentOrderItemUnitFilter,
                currentProviderNameFilter = currentProviderNameFilter,
                page = 1
            });
        }

        public IActionResult IndexFilterAdditional(int? page,
            string? currentDateToFilter, string? currentDateFromFilter,
            OrderIndexViewModel orderIndexViewModel)
        {
            ViewData["CurrentDateTo"] = currentDateToFilter;
            ViewData["CurrentDateFrom"] = currentDateFromFilter;

            ViewData["CurrentProviderIdFilter"] = orderIndexViewModel.ProviderIdFilter;
            ViewData["CurrentProviderNameFilter"] = orderIndexViewModel.ProviderNameFilter;

            ViewData["CurrentOrderItemNameFilter"] = orderIndexViewModel.OrderItemNameFilter;
            ViewData["CurrentOrderItemUnitFilter"] = orderIndexViewModel.OrderItemUnitFilter;

            ViewData["CurrentNumberFilter"] = orderIndexViewModel.NumberFilter;   

            return RedirectToAction(nameof(Index), new
            {
                currentDateToFilter = currentDateToFilter,
                currentDateFromFilter = currentDateFromFilter,
                currentNumberFilter = orderIndexViewModel.NumberFilter,
                currentProviderIdFilter = orderIndexViewModel.ProviderIdFilter,
                currentOrderItemNameFilter = orderIndexViewModel.OrderItemNameFilter,
                currentOrderItemUnitFilter = orderIndexViewModel.OrderItemUnitFilter,
                currentProviderNameFilter = orderIndexViewModel.ProviderNameFilter,               
                page = 1
            });
        }
        // GET: Order
        public async Task<IActionResult> Index(int? page,
            string? currentDateToFilter, string? currentDateFromFilter, string? currentNumberFilter,
            int? currentProviderIdFilter, string? currentProviderNameFilter,
            string? currentOrderItemNameFilter, string? currentOrderItemUnitFilter,
            OrderIndexViewModel orderIndexViewModel)
        {

            var getFilterDatesRes = TryGetFilterDates(currentDateToFilter, currentDateFromFilter, orderIndexViewModel);

            ViewData["CurrentNumberFilter"] = currentNumberFilter;

            ViewData["CurrentProviderIdFilter"] = currentProviderIdFilter;
            ViewData["CurrentProviderNameFilter"] = currentProviderNameFilter;

            ViewData["CurrentOrderItemNameFilter"] = currentOrderItemNameFilter;
            ViewData["CurrentOrderItemUnitFilter"] = currentOrderItemUnitFilter;

            ViewData["CurrentDateTo"] = currentDateToFilter;
            ViewData["CurrentDateFrom"] = currentDateFromFilter;

            orderIndexViewModel.OrderItemUnitFilter = currentOrderItemUnitFilter;
            orderIndexViewModel.OrderItemNameFilter = currentOrderItemNameFilter;
            orderIndexViewModel.NumberFilter = currentNumberFilter;
            orderIndexViewModel.ProviderIdFilter = currentProviderIdFilter;
            orderIndexViewModel.ProviderNameFilter = currentProviderNameFilter;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<Order> orders;
            if (getFilterDatesRes)
                orders = await _orderRepository.GetOrdersWithFilters(orderIndexViewModel.DateToFilter, orderIndexViewModel.DateFromFilter,
                    orderIndexViewModel.NumberFilter, orderIndexViewModel.ProviderIdFilter, orderIndexViewModel.ProviderNameFilter,
                    orderIndexViewModel.OrderItemNameFilter, orderIndexViewModel.OrderItemUnitFilter).ToListAsync();
            else
                orders = await _orderRepository.GetOrdersWithFilters(null, null,
                orderIndexViewModel.NumberFilter, orderIndexViewModel.ProviderIdFilter, orderIndexViewModel.ProviderNameFilter,
                orderIndexViewModel.OrderItemNameFilter, orderIndexViewModel.OrderItemUnitFilter).ToListAsync();

            orderIndexViewModel.Orders = (PagedList<Order>?)orders.ToPagedList(pageNumber, pageSize);

            return View(orderIndexViewModel);
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
            ViewBag.Providers = await _providerRepository.FillProviderViewBagAsync();

            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Date,ProviderId,OrderItems")] Order order, string command, int? idItemForDelete)
        {
            ViewBag.Providers = await _providerRepository.FillProviderViewBagAsync();          
            try
            {
                if (command.Equals("Create"))
                {
                    if (!ModelState.IsValid)
                        return View(order);

                    _orderRepository.Add(order);
                    await _orderRepository.SaveChangesAsync();

                    TempData["success"] = "Order created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                if (command.Equals("Add"))
                {
                    order.OrderItems.Add(new OrderItem());
                    return View(order);
                }

                if (command.Equals("Delete"))
                {
                    if (idItemForDelete == null)
                    {
                        ModelState.AddModelError("", "Error idItemForDelete is null");
                        return View(order);
                    }

                    var orderItem = order.OrderItems.FirstOrDefault(i => i.Id == idItemForDelete);

                    if (orderItem == null)
                        return NotFound();

                    order.OrderItems.Remove(orderItem);
                    return View(order);
                }
                ModelState.AddModelError("", "Error no command");
                return View(order);
            }
            catch
            {
                ModelState.AddModelError("", "Error while create");
                return View(order);
            }
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _orderRepository.GetOrderByIdAsync((int)id);

            if (order == null)
                return NotFound();

            ViewBag.Providers = await _providerRepository.FillProviderViewBagAsync();

            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Provider,Date,ProviderId,OrderItems")] Order order, string command, int? idItemForDelete)
        {
            if (order == null || id != order.Id)
                return NotFound();

            ViewBag.Providers = await _providerRepository.FillProviderViewBagAsync();

            
            try
            {
                if (command.Equals("Save"))
                {
                    if (!ModelState.IsValid)
                        return View(order);

                    _orderRepository.Update(order);
                    await _orderRepository.SaveChangesAsync();

                    TempData["success"] = "Order updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                if (command.Equals("Add"))
                {
                    order.OrderItems.Add(new OrderItem());
                    return View(order);
                }

                if (command.Equals("Delete"))
                {
                    if (idItemForDelete == null)
                    {
                        ModelState.AddModelError("", "Error idItemForDelete is null");
                        return View(order);
                    }

                    var orderItem = order.OrderItems.FirstOrDefault(i => i.Id == idItemForDelete);

                    if (orderItem == null)
                        return NotFound();

                    order.OrderItems.Remove(orderItem);
                    return View(order);
                }
                ModelState.AddModelError("", "Error no command");
                return View(order);
            }
            catch
            {
                ModelState.AddModelError("", "Error while update");
                return View(order);
            }
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

            if (order == null)
                return NotFound();

            if (order.OrderItems != null)
                _orderItemRepository.RemoveRange(order.OrderItems);

            _orderRepository.Remove(order);
            await _orderRepository.SaveChangesAsync();

            TempData["success"] = "Order deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

    }
}
