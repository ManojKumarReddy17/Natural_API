#nullable disable
namespace Natural_API.Resources
{
    public class DSRRetailorsListResource
    {
        public string Id { get; set; }

        public string Executive { get; set; }
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public string OrderBy { get; set; }
        public double TotalAmount { get; set; }
        public string Area { get;set; }
        public string Image { get; set; }

        public string Address { get; set; }

        public string Phonenumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
