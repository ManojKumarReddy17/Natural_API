using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Dsrdetail
    {
        public string Dsr { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Dsr DsrNavigation { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}
