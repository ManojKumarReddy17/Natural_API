using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class DistributorToExecutive
    {
        public string Id { get; set; }
        public string ExecutiveId { get; set; }
        public string DistributorId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Distributor Distributor { get; set; }
        public virtual Executive Executive { get; set; }
    }
}
