using System;
namespace Natural_API.Resources
{
	public class DsrInsertResource
	{
        public string Executive { get; set; }
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public string? OrderBy { get; set; }
        public string Area { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; } //added 14 
        public DateTime ModifiedDate { get; set; }//added 14 

        public List<DsrdetailProduct> product { get; set; }
    }
}

