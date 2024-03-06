using System;
namespace Natural_API.Resources
{
	public class DsrEditResource
	{
        public string Executive { get; set; }
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public string? OrderBy { get; set; }
        public decimal TotalAmount { get; set; }

        public List<DsrProductResource> dsrdetail { get; set; }
    }
}

