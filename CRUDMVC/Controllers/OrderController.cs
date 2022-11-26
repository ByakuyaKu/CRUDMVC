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

        public async Task<IActionResult> IndexFilterDate(int? page, string? searchString,
            string? currentDateToFilter, string? currentDateFromFilter, string? currentNumberFilter,
            int? currentProviderIdFilter, string? currentProviderNameFilter, int? providerIdFilter,
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
                page = page
            });
        }
        // GET: Order
        public async Task<IActionResult> Index(int? page,
            string? currentDateToFilter, string? currentDateFromFilter, string? currentNumberFilter,
            int? currentProviderIdFilter, //int? providerIdFilter,
            string? currentProviderNameFilter, //string? providerNameFilter,
            string? currentOrderItemNameFilter, string? currentOrderItemUnitFilter,
            OrderIndexViewModel orderIndexViewModel)
        {

            var getFilterDatesRes = TryGetFilterDates(currentDateToFilter, currentDateFromFilter, orderIndexViewModel);

            if (orderIndexViewModel.ProviderIdFilter != null)
                page = 1;
            else
                orderIndexViewModel.ProviderIdFilter = currentProviderIdFilter;

            if (orderIndexViewModel.ProviderNameFilter != null)
                page = 1;
            else
                orderIndexViewModel.ProviderNameFilter = currentProviderNameFilter;

            if (orderIndexViewModel.OrderItemNameFilter != null)
                page = 1;
            else
                orderIndexViewModel.OrderItemNameFilter = currentOrderItemNameFilter;

            if (orderIndexViewModel.OrderItemUnitFilter != null)
                page = 1;
            else
                orderIndexViewModel.OrderItemUnitFilter = currentOrderItemUnitFilter;

            if (orderIndexViewModel.NumberFilter != null)
                page = 1;
            else
                orderIndexViewModel.NumberFilter = currentNumberFilter;

            ViewData["CurrentNumberFilter"] = orderIndexViewModel.NumberFilter;

            ViewData["CurrentProviderIdFilter"] = orderIndexViewModel.ProviderIdFilter;
            ViewData["CurrentProviderNameFilter"] = orderIndexViewModel.ProviderNameFilter;

            ViewData["CurrentOrderItemNameFilter"] = orderIndexViewModel.OrderItemNameFilter;
            ViewData["CurrentOrderItemUnitFilter"] = orderIndexViewModel.OrderItemUnitFilter;

            ViewData["CurrentDateTo"] = currentDateToFilter;
            ViewData["CurrentDateFrom"] = currentDateFromFilter;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<Order> orders;
            if (getFilterDatesRes)
                orders = _orderRepository.GetOrdersWithFilters(orderIndexViewModel.DateToFilter, orderIndexViewModel.DateFromFilter,
                    orderIndexViewModel.NumberFilter, orderIndexViewModel.ProviderIdFilter, orderIndexViewModel.ProviderNameFilter,
                    orderIndexViewModel.OrderItemNameFilter, orderIndexViewModel.OrderItemUnitFilter).ToList();
            else
                orders = _orderRepository.GetOrdersWithFilters(null, null,
                orderIndexViewModel.NumberFilter, orderIndexViewModel.ProviderIdFilter, orderIndexViewModel.ProviderNameFilter,
                orderIndexViewModel.OrderItemNameFilter, orderIndexViewModel.OrderItemUnitFilter).ToList();


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
        public async Task<IActionResult> Create([Bind("Id,Number,Date,ProviderId,OrderItems")] Order order)
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

            if (!ModelState.IsValid)
                return View(order);

            ViewBag.Providers = await _providerRepository.FillProviderViewBagAsync();
            try
            {
                if (command.Equals("Save"))
                {

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
                    //_orderItemRepository.Remove(orderItem);
                    //await _orderRepository.SaveChangesAsync();             
                    return View(order);
                }
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

            if (order != null)
            {
                if (order.OrderItems != null)
                {
                    _orderItemRepository.RemoveRange(order.OrderItems);
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
