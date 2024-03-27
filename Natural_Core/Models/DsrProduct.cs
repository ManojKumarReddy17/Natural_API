using System;
namespace Natural_Core.Models
{
	public class DsrProduct
	{
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public string Category { get; set; }
        public string Dsr { get; set; }

        public bool IsDeleted { get; set; }


    }
}

