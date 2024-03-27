using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class DistributorNotification
    {
        public int Id { get; set; }
        public string Distributor { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Distributor DistributorNavigation { get; set; }
    }
}
