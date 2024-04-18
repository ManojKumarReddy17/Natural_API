using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Notification
    {
        public Notification()
        {
            NotificationDistributors = new HashSet<NotificationDistributor>();
            NotificationExecutives = new HashSet<NotificationExecutive>();
        }

        public string Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<NotificationDistributor> NotificationDistributors { get; set; }
        public virtual ICollection<NotificationExecutive> NotificationExecutives { get; set; }
    }
}
