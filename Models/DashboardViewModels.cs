using System.ComponentModel.DataAnnotations;

namespace CPRM.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalPartners { get; set; }
        public int TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public double TotalRevenue { get; set; }
        public List<RecentSale> RecentSales { get; set; } = new List<RecentSale>();
        public List<RecentOrder> RecentOrders { get; set; } = new List<RecentOrder>();
        public List<PartnerPerformance> TopPerformingPartners { get; set; } = new List<PartnerPerformance>();
        public List<ProductPerformance> TopSellingProducts { get; set; } = new List<ProductPerformance>();
        public List<LoginActivity> RecentLoginActivity { get; set; } = new List<LoginActivity>();
    }

    public class PartnerDashboardViewModel
    {
        public int TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public double TotalRevenue { get; set; }
        public double PendingPayments { get; set; }
        public List<RecentSale> RecentSales { get; set; } = new List<RecentSale>();
        public List<RecentOrder> RecentOrders { get; set; } = new List<RecentOrder>();
        public List<ProductPerformance> TopSellingProducts { get; set; } = new List<ProductPerformance>();
        public List<Document> RecentDocuments { get; set; } = new List<Document>();
    }

    public class RecentSale
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public string ProductName { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }

    public class RecentOrder
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }

    public class PartnerPerformance
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public int TotalSales { get; set; }
        public double TotalRevenue { get; set; }
        public double CommissionEarned { get; set; }
    }

    public class ProductPerformance
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSales { get; set; }
        public double TotalRevenue { get; set; }
        public int StockLevel { get; set; }
    }

    public class LoginActivity
    {
        public string UserName { get; set; }
        public DateTime LoginTime { get; set; }
        public string IPAddress { get; set; }
        public string Status { get; set; }
    }
}