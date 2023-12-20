using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Dsrdetail
    {
        public string Id { get; set; }
        public string Dsr { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }

        public virtual Dsr DsrNavigation { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}
