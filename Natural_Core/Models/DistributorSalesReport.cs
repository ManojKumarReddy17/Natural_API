using System;
using System.Collections.Generic;
using System.Text;
#nullable disable
namespace Natural_Core.Models
{
    public class DistributorSalesReport
    {


        public string Area { get; set; }
        public string Executive { get; set; }

        public string Distributor { get; set; }

        public string Retailor { get; set; }


        public DateTime CreatedDate { get; set; }

        public string Product { get; set; }
        public string Product_Name { get; set; }
       public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string productType { get;set; }
        public decimal SaleAmount { get; set; }
    }

    public class DistributorSalesReportResult
    {
        public string Product { get; set; }
        public string Product_Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SaleAmount { get; set; }
    }
}
