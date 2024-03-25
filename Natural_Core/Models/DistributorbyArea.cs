using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class DistributorbyArea
    {
        public int Id { get; set; }
        public string DistributorId { get; set; }
        public string AreaId { get; set; }

        public virtual Area Area { get; set; }
        public virtual Distributor Distributor { get; set; }
    }
}
