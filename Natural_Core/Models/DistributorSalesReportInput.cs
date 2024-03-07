using System;
using System.Collections.Generic;
using System.Text;
#nullable disable
namespace Natural_Core.Models
{
    public class DistributorSalesReportInput
    {
        public string Executive { get; set; }

        public string Distributor { get; set; }

        public string Retailor { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
