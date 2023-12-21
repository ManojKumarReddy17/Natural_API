using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Product
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string Grams { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
    }
}
