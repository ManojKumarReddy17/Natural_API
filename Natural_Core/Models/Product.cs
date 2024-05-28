using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Product
    {
        public Product()
        {
            Dsrdetails = new HashSet<Dsrdetail>();
        }

        public string Id { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Weight { get; set; }
        public string Image { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual ICollection<Dsrdetail> Dsrdetails { get; set; }
    }
}
