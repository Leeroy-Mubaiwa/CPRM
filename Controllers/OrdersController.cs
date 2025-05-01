using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPRM.Data;
using CPRM.Models;

namespace CPRM.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CPRMDbContext _context;

        public OrdersController(CPRMDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var cPRMDbContext = _context.Orders.Include(o => o.Partner);
            return View(await cPRMDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Partner)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "FullName");
            ViewBag.Products = _context.Products
                .Where(p => p.IsActive == true)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.Price
                })
                .ToList();
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartnerId,OrderDate,Status,TotalAmount,OrderItems")] Order order)
        {
            if (ModelState.IsValid)
            {
                // Set default values
                order.OrderDate = order.OrderDate ?? DateTime.Now;
                order.Status = order.Status ?? "Pending";
                order.TotalAmount = order.TotalAmount ?? 0;

                // Add order items
                foreach (var item in order.OrderItems)
                {
                    item.Order = order;
                }

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed; redisplay form
            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "FullName");
            ViewBag.Products = _context.Products
                .Where(p => p.IsActive == true)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.Price
                })
                .ToList();
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "PartnerId", order.PartnerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,PartnerId,OrderDate,Status,TotalAmount")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "PartnerId", order.PartnerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Partner)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
