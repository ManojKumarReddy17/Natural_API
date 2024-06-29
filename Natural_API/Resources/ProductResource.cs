#nullable disable
namespace Natural_API.Resources
{
    public class ProductResource
    {
     
        public string Id { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public string ProductType { get; set; }


        public IFormFile UploadImage { get; set; }
    }
}
