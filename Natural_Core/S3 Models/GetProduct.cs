using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class GetProduct
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public string PresignedUrl { get; set; }
        public int? Quantity { get; set; }
        public decimal? Weight { get; set; }

    }
}
