using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class RetailorToDistributor
    {
        public string Id { get; set; }
        public string DistributorId { get; set; }
        public string RetailorId { get; set; }
       

        public virtual Distributor Distributor { get; set; }
        public virtual Retailor Retailor { get; set; }
    }
}
