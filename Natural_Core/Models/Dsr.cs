using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Dsr
    {
        public string Id { get; set; }
        public string Executive { get; set; }
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public DateTime Date { get; set; }

        public virtual Distributor DistributorNavigation { get; set; }
        public virtual Executive ExecutiveNavigation { get; set; }
        public virtual Retailor RetailorNavigation { get; set; }
    }
}
