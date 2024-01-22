#nullable disable
namespace Natural_API.Resources
{
    public class DsrDetailsByIdResource
    {
        public string Executive { get; set; }
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public string OrderBy { get; set; }

        public DateTime CreatedDate { get; set; }
        //public IEnumerable<DsrProductResource> ProductDetails { get; set; }


    }
}
