using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class GetProduct
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public int? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
