using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Products = new HashSet<Product>();
        }

        public string Id { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductTypeCode { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
