using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class NotificationExecutive
    {
        public int Id { get; set; }
        public string Executive { get; set; }
        public string Notification { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ExecutiveGetResourcecs ExecutiveNavigation { get; set; }
        public virtual Notification NotificationNavigation { get; set; }
    }
}
