using System;
using System.Collections.Generic;
using System.Text;

namespace Natural_Core.Models
{
    public class DistributorReport
    {
        public string Distributor { get; set; }

        public string Retailor { get; set; }


        public DateTime CreatedDate { get; set; }

        public string Product { get; set; }
        public string Product_Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SaleAmount { get; set; }
    }
}
