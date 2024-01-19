#nullable disable
namespace Natural_API.Resources
{
    public class ProductResource
    {
        //public string Category { get; set; }
        //public string ProductName { get; set; }
        //public string Grams { get; set; }
        //public string Price { get; set; }
        //public string Quantity { get; set; }
        //public string Amount { get; set; }

        public string Id { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public string Quantity { get; set; }
        public int? Weight { get; set; }
        //public string Image { get; set; }

        //public string PresignedUrl { get; set; }

        public IFormFile UploadImage { get; set; }
    }
}
