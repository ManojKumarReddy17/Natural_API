using System;
using System.Collections.Generic;
using System.Text;

namespace Natural_Core.Models
{
    public class DistributorLoginReports
    {
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
