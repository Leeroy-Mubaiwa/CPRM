using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CPRM.Data;
using CPRM.Models;
using System.Security.Claims;

namespace CPRM.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly CPRMDbContext _context;

        public DashboardController(CPRMDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalPartners = await _context.Partners.CountAsync(),
                TotalSales = await _context.Sales.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalRevenue = await _context.Sales.SumAsync(s => s.Amount.Value)
            };

            // Get recent sales
            model.RecentSales = await _context.Sales
                .Include(s => s.Partner)
                .Include(s => s.Product)
                .OrderByDescending(s => s.SaleDate)
                .Take(5)
                .Select(s => new RecentSale
                {
                    Id = s.SaleId,
                    PartnerName = s.Partner.FullName,
                    ProductName = s.Product.Description,
                    Amount = s.Amount.Value,
                    Date = s.SaleDate.Value,

                })
                .ToListAsync();

            // Get recent orders
            model.RecentOrders = await _context.Orders
                .Include(o => o.Partner)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => new RecentOrder
                {
                    Id = o.OrderId,
                    PartnerName = o.Partner.FullName,
                    ProductName = o.OrderItems.FirstOrDefault().Product.Description,
                    Quantity = o.OrderItems.Sum(oi => oi.Quantity.Value),
                    TotalAmount = o.TotalAmount.Value,
                    OrderDate = o.OrderDate.Value,
                    Status = o.Status
                })
                .ToListAsync();

            // Get top performing partners
            model.TopPerformingPartners = await _context.Sales
                .Include(s => s.Partner)
                .GroupBy(s => new { s.PartnerId, s.Partner.FullName })
                .Select(g => new PartnerPerformance
                {
                    PartnerId = g.Key.PartnerId.Value,
                    PartnerName = g.Key.FullName,
                    TotalSales = g.Count(),
                    TotalRevenue = g.Sum(s => s.Amount.Value),
                    CommissionEarned = g.Sum(s => s.Amount.Value)
                })
                .OrderByDescending(p => p.TotalRevenue)
                .Take(5)
                .ToListAsync();

            // Get top selling products
            model.TopSellingProducts = await _context.Sales
                .Include(s => s.Product)
                .GroupBy(s => new { s.ProductId, s.Product.ProductName, s.Product.Quantity })
                .Select(g => new ProductPerformance
                {
                    ProductId = g.Key.ProductId.Value,
                    ProductName = g.Key.ProductName,
                    TotalSales = g.Count(),
                    TotalRevenue = g.Sum(s => s.Amount.Value),
                    StockLevel = g.Key.Quantity.Value
                })
                .OrderByDescending(p => p.TotalSales)
                .Take(5)
                .ToListAsync();

            // // Get recent login activity
            // model.RecentLoginActivity = await _context.LoginLogs
            //     .OrderByDescending(l => l.LoginTime)
            //     .Take(10)
            //     .Select(l => new LoginActivity
            //     {
            //         UserName = l.UserName,
            //         LoginTime = l.LoginTime,
            //         IPAddress = l.IPAddress,
            //         Status = l.Status
            //     })
            //     .ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> Partner()
        {
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var partner = await _context.Partners.FirstOrDefaultAsync(p => p. == userId);

            // if (partner == null)
            // {
            //     return NotFound();
            // }

            // var model = new PartnerDashboardViewModel
            // {
            //     TotalSales = await _context.Sales.CountAsync(s => s.PartnerId == partner.Id),
            //     TotalOrders = await _context.Orders.CountAsync(o => o.PartnerId == partner.Id),
            //     TotalRevenue = await _context.Sales
            //         .Where(s => s.PartnerId == partner.Id)
            //         .SumAsync(s => s.Amount),
            //     PendingPayments = await _context.Sales
            //         .Where(s => s.PartnerId == partner.Id )
            //         .SumAsync(s => s.Amount)
            // };

            // // Get recent sales
            // model.RecentSales = await _context.Sales
            //     .Include(s => s.Product)
            //     .Where(s => s.PartnerId == partner.Id)
            //     .OrderByDescending(s => s.SaleDate)
            //     .Take(5)
            //     .Select(s => new RecentSale
            //     {
            //         Id = s.SaleId,
            //         ProductName = s.Product.Description,
            //         Amount = s.Amount.Value,
            //         Date = s.SaleDate.Value,

            //     })
            //     .ToListAsync();

            // // Get recent orders
            // model.RecentOrders = await _context.Orders
            //     .Include(o => o.OrderItems)
            //         .ThenInclude(oi => oi.Product)
            //     .Where(o => o.PartnerId == partner.Id)
            //     .OrderByDescending(o => o.OrderDate)
            //     .Take(5)
            //     .Select(o => new RecentOrder
            //     {
            //         Id = o.Id,
            //         ProductName = o.OrderItems.FirstOrDefault().Product.Name,
            //         Quantity = o.OrderItems.Sum(oi => oi.Quantity),
            //         TotalAmount = o.TotalAmount,
            //         OrderDate = o.OrderDate,
            //         Status = o.Status
            //     })
            //     .ToListAsync();

            // // Get top selling products
            // model.TopSellingProducts = await _context.Sales
            //     .Include(s => s.Product)
            //     .Where(s => s.PartnerId == partner.Id)
            //     .GroupBy(s => new { s.ProductId, s.Product.Name, s.Product.StockLevel })
            //     .Select(g => new ProductPerformance
            //     {
            //         ProductId = g.Key.ProductId,
            //         ProductName = g.Key.Name,
            //         TotalSales = g.Count(),
            //         TotalRevenue = g.Sum(s => s.Amount),
            //         StockLevel = g.Key.StockLevel
            //     })
            //     .OrderByDescending(p => p.TotalSales)
            //     .Take(5)
            //     .ToListAsync();

            // // Get recent documents
            // model.RecentDocuments = await _context.Documents
            //     .Where(d => d.PartnerId == partner.Id)
            //     .OrderByDescending(d => d.UploadDate)
            //     .Take(5)
            //     .ToListAsync();

            return View();
        }
    }
}
