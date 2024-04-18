using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class DistributorChangePassword
    {
        public string Id { get; set; }
        public string DistributorId { get; set; }
        public string CurrentPassword { get; set; }
        public string Newpassword { get; set; }

        public virtual Distributor Distributor { get; set; }
    }
}
