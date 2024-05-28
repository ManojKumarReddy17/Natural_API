using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class ExecutiveArea
    {
        public int Id { get; set; }
        public string Executive { get; set; }
        public string Area { get; set; }

        public virtual Area AreaNavigation { get; set; }
        public virtual Executive ExecutiveNavigation { get; set; }
    }
}
