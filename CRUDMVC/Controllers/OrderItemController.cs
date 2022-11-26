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
using CRUDMVC.Repository;
using Microsoft.Data.SqlClient;
using X.PagedList;

namespace CRUDMVC.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        // GET: OrderItem
        public async Task<IActionResult> Index(string? sortOrder, string? searchString, string currentFilter, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentSortView"] = sortOrder?.Replace("_", " ").ToLower();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var orders = _orderItemRepository.GetOrderItemListIQueryableAsNoTracking(searchString);

            orders = _orderItemRepository.SortOrderItems(sortOrder, orders);

            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        // GET: OrderItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var orderItem = await _orderItemRepository.GetOrderItemByIdAsNoTrackingAsync((int)id);

            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        // GET: OrderItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Quantity,Unit,OrderId")] OrderItem orderItem)
        {
            if (!ModelState.IsValid)
                return View(orderItem);

            _orderItemRepository.Add(orderItem);
            await _orderItemRepository.SaveChangesAsync();

            TempData["success"] = "Order item created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: OrderItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync((int)id);

            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        // POST: OrderItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,Unit,OrderId, Order")] OrderItem orderItem)
        {
            if (orderItem == null || id != orderItem.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(orderItem);

            //var o = await _orderItemRepository.GetOrderItemByIdAsNoTrackingAsync(id);
            //orderItem.Order = o.Order;
            //orderItem.OrderId = o.OrderId;

            //try
            {
                //_orderItemRepository.Update(orderItem);
                await _orderItemRepository.SaveChangesAsync();

                TempData["success"] = "Order updated successfully!";
            }
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!await _orderItemRepository.OrderItemExistsAsync(orderItem.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }
            //}
            return RedirectToAction(nameof(Index), "Order");
        }

        // GET: OrderItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var orderItem = await _orderItemRepository.GetOrderItemByIdAsNoTrackingAsync((int)id);

            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        // POST: OrderItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);

            if (orderItem != null)
                _orderItemRepository.Remove(orderItem);

            await _orderItemRepository.SaveChangesAsync();

            TempData["success"] = "Order item deleted successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
