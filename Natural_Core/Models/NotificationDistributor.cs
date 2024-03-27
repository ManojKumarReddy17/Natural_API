using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class NotificationDistributor
    {
        public int Id { get; set; }
        public string Distributor { get; set; }
        public string Notification { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Distributor DistributorNavigation { get; set; }
        public virtual Notification NotificationNavigation { get; set; }
    }
}
